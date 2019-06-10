﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class ABLoader : ResourcesLoader
{
    public override void Init()
    {
        InitResPathConfig();
    }

    /// <summary>
    /// 初始化资源路径配置
    /// </summary>
    void InitResPathConfig()
    {
        string path = string.Format("LZMA/ResConfigs").ToLower() + AssetBundleDef.ABSuffix;
        string fullPath = Paths.GetFullPath(path);
        AssetBundle ab = AssetBundle.LoadFromFile(fullPath);
        ResPathConfig = ab.LoadAsset<ResourcePathConfig>("ResourcePathConfig");
        ab.Unload(true);
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