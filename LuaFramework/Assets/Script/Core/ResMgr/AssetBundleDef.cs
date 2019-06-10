using System;
using UnityEngine;

public class AssetBundleDef
{
    public static string ABName = ".XAsset";
    public static string ABSuffix = "XAsset";
    /// <summary>
    /// 资源配置文件路径
    /// </summary>
    public static string ResConfigsPath = "Assets/Resources/ResConfig/";
    /// <summary>
    /// 持久化目录
    /// </summary>
#if UNITY_EDITOR
    public static string PersistentDataPath = Application.dataPath + "/../AssetBundles/";
#elif UNITY_ANDROID
    public static string PersistentDataPath = Application.persistentDataPath + "/Android/";
#elif UNITY_IOS
    public static string PersistentDataPath = Application.persistentDataPath + "/IOS/";
#elif UNITY_STANDALONE_WIN
    public static string PersistentDataPath = Application.persistentDataPath + "/Windows/";
#else
    public static string PersistentDataPath = Application.persistentDataPath + "/Other/";
#endif

    /// <summary>
    /// 流媒体目录
    /// </summary>
#if UNITY_ANDROID
    public static string StreamPath = Application.streamingAssetsPath + "/Android/";
#elif UNITY_IOS
    public static string StreamPath = Application.streamingAssetsPath + "/IOS/";
#elif UNITY_STANDALONE_WIN
    public static string StreamPath = Application.streamingAssetsPath + "/Windows/";
#else
    public static string StreamPath = Application.streamingAssetsPath + "/Other/";
#endif
}