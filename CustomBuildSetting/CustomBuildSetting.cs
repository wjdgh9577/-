#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildSettingMode
{
    Developer,
    Release,
    GM
}

public class CustomBuildSetting : ScriptableObject
{
    public const string customBuildSettingAssetName = "CustomBuildSetting";
    public const string customBuildSettingPath = "CustomBuildSetting/Resources";
    public const string customBuildSettingExtension = ".asset";
    private static CustomBuildSetting _instance;

    public static void SetInstance(CustomBuildSetting setting)
    {
        _instance = setting;
    }

    public static CustomBuildSetting instance
    {
        get
        {
            if (ReferenceEquals(_instance, null))
            {
                _instance = Resources.Load<CustomBuildSetting>(customBuildSettingAssetName);
                if (ReferenceEquals(_instance, null))
                {
                    _instance = CreateInstance<CustomBuildSetting>();
                }
            }
            return _instance;
        }
    }

    [Space]

    [SerializeField] private BuildSettingMode _buildSettingMode = BuildSettingMode.Developer;
    public static BuildSettingMode builSettingdMode
    {
        get { return instance._buildSettingMode; }
        set { instance._buildSettingMode = value; }
    }

    [SerializeField] private bool _deviceLogin = false;
    public static bool deviceLogin
    {
        get { return instance._deviceLogin; }
        set { instance._deviceLogin = value; }
    }

#if !DISABLESTEAMWORKS
    [SerializeField] private bool _steamEnable = false;
    public static bool steamEnable
    {
        get { return instance._steamEnable; }
        set { instance._steamEnable = value; }
    }

    [SerializeField] private bool _sandboxMode = true;
    public static bool sandboxMode
    {
        get { return instance._sandboxMode; }
        set { instance._sandboxMode = value; }
    }
#endif
}
