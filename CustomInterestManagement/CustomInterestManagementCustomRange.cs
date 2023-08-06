using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/CustomInterestManagementCustomRange")]
    public class CustomManagementCustomRange : NetworkBehaviour
    {
        [Tooltip("The maximum range that objects will be visible at.")]
        [SyncVar, HideInInspector]
        public int visRange = 50;

        [SerializeField]
        private int newVisRange = 50;

        [Command]
        public void CmdSetVisRange(int visRange)
        {            
            var before = this.visRange;
            this.visRange = visRange;
            var after = this.visRange;
            TargetSetVisRange(before, after);

        }

        [TargetRpc]
        public void TargetSetVisRange(int before, int after)
        {
            if (newVisRange == after)
                Debug.Log($"Visible range changed. {before} -> {after}");
            else
                Debug.LogError($"Failed to change visible range from {visRange} to {newVisRange}.");
        }

        public void OnValidate()
        {   
            if(visRange != newVisRange)
                CmdSetVisRange(newVisRange);  
        }
    }
}
