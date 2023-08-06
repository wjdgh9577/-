using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class CustomInterestManagement : InterestManagement
    {
        [Tooltip("The maximum range that objects will be visible at. Add DistanceInterestManagementCustomRange onto NetworkIdentities for custom ranges.")]
        public int visRange = 10;

        [Tooltip("Rebuild all every 'rebuildInterval' seconds.")]
        public float rebuildInterval = 1;
        double lastRebuildTime;

        // Use Scene instead of string scene.name because when additively
        // loading multiples of a subscene the name won't be unique
        readonly Dictionary<Scene, HashSet<NetworkIdentity>> sceneObjects =
            new Dictionary<Scene, HashSet<NetworkIdentity>>();

        readonly Dictionary<NetworkIdentity, Scene> lastObjectScene =
            new Dictionary<NetworkIdentity, Scene>();

        HashSet<Scene> dirtyScenes = new HashSet<Scene>();

        [ServerCallback]
        public override void Reset()
        {
            lastRebuildTime = 0D;
        }

        // helper function to get vis range for a given object, or default.
        int GetVisRange(NetworkIdentity identity)
        {
            DistanceInterestManagementCustomRange custom = identity.GetComponent<DistanceInterestManagementCustomRange>();
            return custom != null ? custom.visRange : visRange;
        }

        public override void OnSpawned(NetworkIdentity identity)
        {
            Scene currentScene = identity.gameObject.scene;
            lastObjectScene[identity] = currentScene;
            // Debug.Log($"SceneInterestManagement.OnSpawned({identity.name}) currentScene: {currentScene}");
            if (!sceneObjects.TryGetValue(currentScene, out HashSet<NetworkIdentity> objects))
            {
                objects = new HashSet<NetworkIdentity>();
                sceneObjects.Add(currentScene, objects);
            }

            objects.Add(identity);
        }

        public override void OnDestroyed(NetworkIdentity identity)
        {
            Scene currentScene = lastObjectScene[identity];
            lastObjectScene.Remove(identity);
            if (sceneObjects.TryGetValue(currentScene, out HashSet<NetworkIdentity> objects) && objects.Remove(identity))
                RebuildSceneObservers(currentScene);
        }

        // internal so we can update from tests
        [ServerCallback]
        internal void Update()
        {
            // for each spawned:
            //   if scene changed:
            //     add previous to dirty
            //     add new to dirty
            foreach (NetworkIdentity identity in NetworkServer.spawned.Values)
            {
                Scene currentScene = lastObjectScene[identity];
                Scene newScene = identity.gameObject.scene;
                if (newScene == currentScene)
                    continue;

                // Mark new/old scenes as dirty so they get rebuilt
                dirtyScenes.Add(currentScene);
                dirtyScenes.Add(newScene);

                // This object is in a new scene so observers in the prior scene
                // and the new scene need to rebuild their respective observers lists.

                // Remove this object from the hashset of the scene it just left
                sceneObjects[currentScene].Remove(identity);

                // Set this to the new scene this object just entered
                lastObjectScene[identity] = newScene;

                // Make sure this new scene is in the dictionary
                if (!sceneObjects.ContainsKey(newScene))
                    sceneObjects.Add(newScene, new HashSet<NetworkIdentity>());

                // Add this object to the hashset of the new scene
                sceneObjects[newScene].Add(identity);
            }

            // rebuild all dirty scenes
            foreach (Scene dirtyScene in dirtyScenes)
                RebuildSceneObservers(dirtyScene);

            dirtyScenes.Clear();

            // rebuild all spawned NetworkIdentity's observers every interval
            if (NetworkTime.localTime >= lastRebuildTime + rebuildInterval)
            {
                RebuildAll();
                lastRebuildTime = NetworkTime.localTime;
            }
        }

        void RebuildSceneObservers(Scene scene)
        {
            foreach (NetworkIdentity netIdentity in sceneObjects[scene])
                if (netIdentity != null)
                    NetworkServer.RebuildObservers(netIdentity, false);
        }

        public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnection newObserver)
        {
            int range = GetVisRange(newObserver.identity);
            return identity.gameObject.scene == newObserver.identity.gameObject.scene && Vector3.Distance(identity.transform.position, newObserver.identity.transform.position) < range;
        }

        public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnection> newObservers, bool initialize)
        {
            if (!sceneObjects.TryGetValue(identity.gameObject.scene, out HashSet<NetworkIdentity> objects))
                return;

            HashSet<NetworkConnection> newSceneObservers = new HashSet<NetworkConnection>();
            // Add everything in the hashset for this object's current scene
            foreach (NetworkIdentity networkIdentity in objects)
                if (networkIdentity != null && networkIdentity.connectionToClient != null)
                    newSceneObservers.Add(networkIdentity.connectionToClient);

            HashSet<NetworkConnection> newDistanceObservers = new HashSet<NetworkConnection>();
            // cache range and .transform because both call GetComponent.
            //int range = GetVisRange(identity);
            Vector3 position = identity.transform.position;

            // brute force distance check
            // -> only player connections can be observers, so it's enough if we
            //    go through all connections instead of all spawned identities.
            // -> compared to UNET's sphere cast checking, this one is orders of
            //    magnitude faster. if we have 10k monsters and run a sphere
            //    cast 10k times, we will see a noticeable lag even with physics
            //    layers. but checking to every connection is fast.
            foreach (NetworkIdentity networkIdentity in objects)
            {
                // authenticated and joined world with a player?
                if (networkIdentity != null && networkIdentity.connectionToClient != null && networkIdentity.connectionToClient.isAuthenticated)
                {
                    int range = GetVisRange(networkIdentity);
                    // check distance
                    if (identity.visible == Visibility.ForceShown || Vector3.Distance(networkIdentity.transform.position, position) < range)
                    {
                        newDistanceObservers.Add(networkIdentity.connectionToClient);
                    }
                }
            }

            foreach (var observer in newSceneObservers)
                if (newDistanceObservers.Contains(observer))
                    newObservers.Add(observer);
        }
    }
}