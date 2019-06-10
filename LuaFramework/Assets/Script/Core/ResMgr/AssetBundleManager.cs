using UnityEngine;

public class AssetBundleManager : UnitySingleton<AssetBundleManager>
{
    /// <summary>
    /// AssetBundleLoader缓存池
    /// </summary>
    SimplePool loaderPool;
    /// <summary>
    /// 当前使用的AB包
    /// </summary>
    Transform ab;
    /// <summary>
    /// 回收的AB包
    /// </summary>
    Transform abRelease;
    /// <summary>
    /// 加载失败的AB文件
    /// </summary>
    Transform abLoadFailed;

    public override void Awake()
    {
        base.Awake();
        ab = new GameObject("ab").transform;
        ab.SetParent(transform, false);
        abRelease = new GameObject("abRelease").transform;
        abRelease.SetParent(transform,false);
        abLoadFailed = new GameObject("abLoadFailed").transform;
        abLoadFailed.SetParent(transform, false);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pathPrefix">路径前缀</param>
    public void Init()
    {
        //InvokeRepeating("UpdateAssetBundles", 1, 1);
    }

}