#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using Object = UnityEngine.Object;
using System.IO;
using System.Collections.Generic;
using System.Linq;


[CustomEditor(typeof(CustomBuildSetting))]
public class CustomBuildSettingEditor : UnityEditor.Editor
{
    GUIContent buildSettingModeLabel = new GUIContent("Build Setting Mode : ", "모드에 따라 UI가 자동 조정됨");
    GUIContent deviceLoginLabel = new GUIContent("Device Login : ");
#if !DISABLESTEAMWORKS
    GUIContent steamEnableLabel = new GUIContent("Steam Enable : ", "스팀 로그인 활성화");
    GUIContent sandboxModeLabel = new GUIContent("Sandbox Mode Enable : ", "테스트 지갑 결제 활성화");
#endif
    private const string unityAssetFolder = "Assets";

    public static CustomBuildSetting GetOrCreateSettingAsset()
    {
        string fullPath = Path.Combine(Path.Combine(unityAssetFolder, CustomBuildSetting.customBuildSettingPath), CustomBuildSetting.customBuildSettingAssetName + CustomBuildSetting.customBuildSettingExtension);
        CustomBuildSetting instance = AssetDatabase.LoadAssetAtPath(fullPath, typeof(CustomBuildSetting)) as CustomBuildSetting;

        if (instance == null)
        {
            if (!Directory.Exists(Path.Combine(unityAssetFolder, CustomBuildSetting.customBuildSettingPath)))
            {
                AssetDatabase.CreateFolder(Path.Combine(unityAssetFolder, "CustomBuildSetting"), "Resources");
            }

            instance = CreateInstance<CustomBuildSetting>();
            AssetDatabase.CreateAsset(instance, fullPath);
            AssetDatabase.SaveAssets();
        }

        return instance;
    }

    [MenuItem("Custom Build Setting/Edit Setting")]
    public static void Edit()
    {
        Selection.activeObject = GetOrCreateSettingAsset();

        ShowInspector();
    }

    private static void ShowInspector()
    {
        try
        {
            var editorAsm = typeof(UnityEditor.Editor).Assembly;
            var type = editorAsm.GetType("UnityEditor.InspectorWindow");
            Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);

            if (findObjectsOfTypeAll.Length > 0)
            {
                ((EditorWindow)findObjectsOfTypeAll[0]).Focus();
            }
            else
            {
                EditorWindow.GetWindow(type);
            }
        }
        catch
        {
        }
    }

    public override void OnInspectorGUI()
    {
        CustomBuildSetting setting = (CustomBuildSetting)target;
        CustomBuildSetting.SetInstance(setting);

        GUILayout.TextArea("Current Build Target Platform : " + EditorUserBuildSettings.selectedBuildTargetGroup.ToString(), EditorStyles.boldLabel);
        GUILayout.TextArea("Current Application Version : " + Application.version, EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        CustomBuildSetting.builSettingdMode = (BuildSettingMode)EditorGUILayout.EnumPopup(buildSettingModeLabel, CustomBuildSetting.builSettingdMode);
        EditorGUILayout.EndHorizontal();

        string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = defineSymbols.Split(';').ToList();
        string targetDefine = "DEV";
        if (CustomBuildSetting.builSettingdMode == BuildSettingMode.Developer || CustomBuildSetting.builSettingdMode == BuildSettingMode.GM)
        {
            if (!allDefines.Contains(targetDefine))
            {
                allDefines.Add(targetDefine);
            }
        }
        else if (CustomBuildSetting.builSettingdMode == BuildSettingMode.Release)
        {
            if (allDefines.Contains(targetDefine))
            {
                allDefines.Remove(targetDefine);
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));

        EditorGUILayout.BeginHorizontal();
        CustomBuildSetting.deviceLogin = EditorGUILayout.Toggle(deviceLoginLabel, CustomBuildSetting.deviceLogin);
        EditorGUILayout.EndHorizontal();

#if !DISABLESTEAMWORKS
        EditorGUILayout.Space();
        GUILayout.TextArea("For Steam Platform", EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        CustomBuildSetting.steamEnable = EditorGUILayout.Toggle(steamEnableLabel, CustomBuildSetting.steamEnable);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        CustomBuildSetting.sandboxMode = EditorGUILayout.Toggle(sandboxModeLabel, CustomBuildSetting.sandboxMode);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
#endif

        EditorGUILayout.Space();
        if (GUILayout.Button("Addressable Build"))
        {
            AddressableAssetSettings.BuildPlayerContent();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Open Project Settings"))
        {
            SettingsService.OpenProjectSettings("Project/Player");
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(setting);
            AssetDatabase.SaveAssets();
        }
    }
}
