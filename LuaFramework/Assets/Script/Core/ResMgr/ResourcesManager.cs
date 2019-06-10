using UnityEngine;
using UnityEngine.Events;

public class ResourcesManager : UnitySingleton<ResourcesManager>
{
    private ResourcesLoader mLoader;

    public void Init(bool useAb)
    {
        if (useAb)
            mLoader = new ABLoader();
        else
            mLoader = new EditorLoader();

        mLoader.Init();
    }

    public static GameObject LoadUI(string name)
    {
        return Resources.Load<GameObject>(name);
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="game">游戏名</param>
    /// <param name="assetName">资源名</param>
    /// <returns></returns>
    public Object LoadAsset(string assetName)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            Log.error("load assetname is null,Please check.");
        }
        return LoadAsset<Object>(assetName);
    }

    public T LoadAsset<T>(string assetName) where T : Object
    {
        return mLoader.LoadAsset<T>(assetName);
    }

    public void LoadAssetAsync<T>(string assetName, UnityAction<string, T> action) where T : Object
    {
        mLoader.LoadAssetAsyn<T>(assetName, action);
    }
}