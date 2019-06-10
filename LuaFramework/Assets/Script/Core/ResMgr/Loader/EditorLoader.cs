using System;
using UnityEngine.Events;

public class EditorLoader : ResourcesLoader
{
    public override void Init()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
    public override T LoadAsset<T>(string name)
    {
        throw new NotImplementedException();
    }

    public override void LoadAssetAsyn<T>(string name, UnityAction<string, T> action)
    {
        throw new NotImplementedException();
    }

    public override bool HaveAsset(string assetName)
    {
        throw new NotImplementedException();
    }

    public override void UnLoadAsset(string name)
    {
        throw new NotImplementedException();
    }

    public override void ClearFailedMap()
    {
        throw new NotImplementedException();
    }

    public override void UnLoadUnUseAsset()
    {
        throw new NotImplementedException();
    }

    public override void UnLoadAll()
    {
        throw new NotImplementedException();
    }

}