using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ResourcesPathEditor
{
    /// <summary>
    /// 创建资源配置
    /// </summary>
    public static void CreateRsourcePathConfig()
    {
        List<string> dirPaths = new List<string>();
        List<string> abNames = new List<string>();
        Dictionary<string, ResPathInfo> resPaths = new Dictionary<string, ResPathInfo>();
        Dictionary<string, AssetBundleInfo> dependences = new Dictionary<string, AssetBundleInfo>();
        ListDirectories("Assets/Resources/", dirPaths, abNames, resPaths, dependences);

        //初始化配置类
        string assetPath = AssetBundleDef.ResConfigsPath + "ResourceConfig.asset";
        string fullPath = EditorUtil.GetFullPath(assetPath);
        ResourcePathConfig configClass = null;
        if (!File.Exists(fullPath))
        {
            FileUtils.CreateDirectory(fullPath);
            configClass = ScriptableObject.CreateInstance<ResourcePathConfig>();
            configClass.Init(dirPaths, abNames, resPaths, dependences);
            AssetDatabase.CreateAsset(configClass, assetPath);
        }
        else
        {
            configClass = AssetDatabase.LoadAssetAtPath<ResourcePathConfig>(assetPath);
            configClass.Init(dirPaths, abNames, resPaths, dependences);
        }
        EditorUtility.SetDirty(configClass);
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 遍历目录
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dirPaths"></param>
    /// <param name="abNames"></param>
    /// <param name="resPaths"></param>
    /// <param name="ai"></param>
    public static void ListDirectories(string path, List<string> dirPaths, List<string> abNames, Dictionary<string, ResPathInfo> resPaths, Dictionary<string, AssetBundleInfo> dependences, AssetImporter ai = null)
    {
        if (AssetBuilder.IsIgnore(path))
            return;
        string assetPath = EditorUtil.GetAssetsPath(path);
        List<string> files = EditorUtil.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        if (files.Count > 0 && dirPaths.IndexOf(assetPath) == -1)
        {
            dirPaths.Add(assetPath);
        }
        if (ai == null)
            ai = AssetImporter.GetAtPath(assetPath);
        if (IsMarked(ai))
        {
            if (abNames.IndexOf(ai.assetBundleName) == -1)
                abNames.Add(ai.assetBundleName);
            AddDependences(dependences, ai);
        }
        else
        {
            ai = null;
        }
        for (int i = 0; i < files.Count; i++)
        {
            ListFiles(files[i], dirPaths, abNames, resPaths, dependences, ai);
        }
        List<string> directories = EditorUtil.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < directories.Count; i++)
        {
            ListDirectories(directories[i], dirPaths, abNames, resPaths, dependences, ai);
        }
    }
    
    
    /// <summary>
    /// 添加依赖
    /// </summary>
    /// <param name="dependences">依赖</param>
    /// <param name="ai">资源导入</param>
    public static void AddDependences(Dictionary<string, AssetBundleInfo> dependences, AssetImporter ai)
    {
        if (!dependences.ContainsKey(ai.assetBundleName))
        {
            string assetBundleName = ai.assetBundleName + "." + ai.assetBundleVariant;
            AssetBundleInfo info = new AssetBundleInfo(assetBundleName, AssetDatabase.GetAssetBundleDependencies(assetBundleName, false));
            dependences.Add(ai.assetBundleName, info);
            CheckDependences(assetBundleName, assetBundleName, string.Empty, new List<string>());
        }
    }
    public static void CheckDependences(string root, string assetBundleName, string log, List<string> list)
    {
        if (list.Contains(assetBundleName)) return;
        string[] array = AssetDatabase.GetAssetBundleDependencies(assetBundleName, false);
        log += assetBundleName + "=>";
        list.Add(assetBundleName);
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == root)
            {
                Debug.LogErrorFormat("Error=>存在资源循环依赖:{0}", log + array[i]);
            }
            else
            {
                CheckDependences(root, array[i], log, list);
            }
        }
    }

    /// <summary>
    /// 遍历文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="dirPaths"></param>
    /// <param name="abNames"></param>
    /// <param name="resPaths"></param>
    /// <param name="ai"></param>
    public static void ListFiles(string path, List<string> dirPaths, List<string> abNames, Dictionary<string, ResPathInfo> resPaths, Dictionary<string, AssetBundleInfo> dependences, AssetImporter ai = null)
    {
        if (path.Contains(".meta"))
            return;
        if (AssetBuilder.IsIgnore(path))
            return;
        string assetPath = EditorUtil.GetAssetsPath(path);
        if (ai == null)
            ai = AssetImporter.GetAtPath(assetPath);
        if (ai == null)
            return;
        string fileName = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        if (IsMarked(ai))
        {
            if (abNames.IndexOf(ai.assetBundleName) == -1)
                abNames.Add(ai.assetBundleName);
            AddDependences(dependences, ai);
        }
        if (resPaths.ContainsKey(fileName))
        {
            var info = resPaths[fileName];
            string oldPath = string.Format("{0}/{1}{2}", dirPaths[info.resPathIndex], info.resName, info.extension);
            Debug.LogErrorFormat("资源名重复:{0}===>{1}", path, oldPath);
        }
        else
        {
            resPaths.Add(fileName, new ResPathInfo(fileName, extension, dirPaths.Count - 1, abNames.Count - 1));
        }
    }

    /// <summary>
    /// 是否标记过
    /// </summary>
    /// <param name="ai"></param>
    /// <returns></returns>
    public static bool IsMarked(AssetImporter ai)
    {
        return ai != null && !string.IsNullOrEmpty(ai.assetBundleName) && !string.IsNullOrEmpty(ai.assetBundleVariant);
    }
}