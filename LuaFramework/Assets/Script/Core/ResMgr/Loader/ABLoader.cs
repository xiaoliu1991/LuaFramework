using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class ABLoader : ResourcesLoader
{
    public class ResData
    {
        /// <summary>
        /// 加载完成回调
        /// </summary>
        public UnityEvent OnLoadFinish = new UnityEvent();
        /// <summary>
        /// 资源名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资源文件
        /// </summary>
        public Object Asset { get; set; }
        /// <summary>
        /// 引用数量
        /// </summary>
        public int RefCount { get; set; }
        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsFinish { get; set; }
        /// <summary>
        /// AB文件处理器
        /// </summary>
        public AssetHandler Loader { get; set; }

        /// <summary>
        /// 加载成功
        /// </summary>
        /// <param name="asset"></param>
        public void LoadSuccess(AssetHandler loader, Object asset)
        {
            this.Asset = asset;
            this.Loader = loader;
            this.IsFinish = true;
#if UNITY_EDITOR
            DealEditorMode();
#endif
//            loader.AddRefResData(this.Name);
            OnLoadFinish.Invoke();
            OnLoadFinish.RemoveAllListeners();
        }

        /// <summary>
        /// 加载失败
        /// </summary>
        /// <param name="loader"></param>
        public void LoadFailed(AssetHandler loader)
        {
            this.Loader = loader;
            this.IsFinish = true;
            OnLoadFinish.Invoke();
            OnLoadFinish.RemoveAllListeners();
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void UnLoad()
        {
//            Loader.RemoveRefResData(this.Name);
            if (Asset != null)
            {
                if (Asset is GameObject)
                    return;
                Resources.UnloadAsset(Asset);
            }
        }

#if UNITY_EDITOR
        private void DealEditorMode()
        {
            if (Asset == null || !(Asset is GameObject)) return;
            var renderers = (Asset as GameObject).GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].sharedMaterials == null) continue;
                foreach (var each in renderers[i].sharedMaterials)
                {
                    if (each == null || each.shader == null) continue;
                    var shaderName = each.shader.name;
                    var newShader = Shader.Find(shaderName);
                    if (newShader != null)
                    {
                        each.shader = newShader;
                    }
                }
            }

            var graphics = (Asset as GameObject).GetComponentsInChildren<UnityEngine.UI.Graphic>(true);
            for (int i = 0; i < graphics.Length; i++)
            {
                if (graphics[i].material == null) continue;
                graphics[i].material.shader = Shader.Find(graphics[i].material.shader.name);
            }
        }
#endif


        /// <summary>
        /// 回收
        /// </summary>
        public void Release()
        {
            RefCount = 0;
            Asset = null;
            Loader = null;
            Name = string.Empty;
            IsFinish = false;
            OnLoadFinish.RemoveAllListeners();
        }
    }

    static ObjectPool<ResData> resDataPool = new ObjectPool<ResData>(null, (resdata) => { resdata.Release(); });
    /// <summary>
    /// 当前资源列表
    /// </summary>
    Dictionary<string, ResData> resMap = new Dictionary<string, ResData>();
    /// <summary>
    /// 加载失败列表
    /// </summary>
    Dictionary<string, ResData> resFailedMap = new Dictionary<string, ResData>();

    public override void Init()
    {
        InitResPathConfig();
    }

    /// <summary>
    /// 初始化资源路径配置
    /// </summary>
    void InitResPathConfig()
    {
        string path = string.Format("ResConfigs").ToLower() + AssetBundleDef.ABSuffix;
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
        T asset = null;
        ResData resData = null;
        if (resFailedMap.TryGetValue(name, out resData))
        {
            return null;
        }
        if (resMap.TryGetValue(name, out resData))
        {
            resData.RefCount++;
            asset = resData.Asset as T;
        }
        else
        {
            string path = ResPathConfig.GetABName(name);
            if (string.IsNullOrEmpty(path))
            {
                Log.error("没有找到包含资源{0}的AssetBundle！", name);
                return null;
            }
            /*AssetHandler loader = AssetHandler.Instance.LoadAssetBundle(path, ResPathConfig);
            resData = resDataPool.Get();
            resData.Name = name;
            resData.RefCount++;
            resMap.Add(name, resData);
            asset = loader.LoadAsset<T>(name);
            resData.LoadSuccess(loader, asset);*/
        }
        return asset;
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