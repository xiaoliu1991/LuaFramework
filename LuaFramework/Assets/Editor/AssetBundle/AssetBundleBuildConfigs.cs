using UnityEngine;
using UnityEditor;
public enum CompressType
{
    //不压缩
    NoCompress,
    //LZ4
    LZ4,
    //LZMA
    LZMA
}

/// <summary>
/// 打包配置文件
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "自定义/打包配置文件")]
public class AssetBundleBuildConfigs : ScriptableObject
{
    /// <summary>
    /// 一个文件夹一个AB包
    /// </summary>
    public AssetBundleBuildConfig[] OneFloderOneBundle;
    /// <summary>
    /// 一个子文件夹一个AB包
    /// </summary>
    public AssetBundleBuildConfig[] SubFloderOneBundle;
    /// <summary>
    /// 一个资源一个AB包
    /// </summary>
    public AssetBundleBuildConfig[] OneAssetOneBundle;
}

[System.Serializable]
public enum BuildCompressType
{
    LZ4,
    LZMA
}

[System.Serializable]
public class AssetBundleBuildConfig
{
    /// <summary>
    /// 资源引用
    /// </summary>
    [SerializeField]
    DefaultAsset asset;

//    [SerializeField]
    /// <summary>
    /// 压缩类型
    /// </summary>
    CompressType compressType = CompressType.LZMA;

    public CompressType CompressType
    {
        get
        {
            if (compressType == CompressType.NoCompress)
            {
                Debug.LogError("混合打包模式暂不支持NoCompress");
                return CompressType.LZMA;
            }
            return compressType;
        }
    }

    public string Path
    {
        get
        {
            if (asset == null)
            {
                return null;
            }
            return AssetDatabase.GetAssetPath(asset).Replace("Assets/", "");
        }
    }
}
