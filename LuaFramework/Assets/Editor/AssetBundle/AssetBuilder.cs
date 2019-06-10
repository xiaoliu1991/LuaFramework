using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetBuilder
{
    /// <summary>
    /// 块压缩
    /// </summary>
    public static BuildAssetBundleOptions LZ4Options
    {
        get
        {
            return BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
        }
    }

    /// <summary>
    /// 默认LZMA压缩
    /// </summary>
    public static BuildAssetBundleOptions LZMAOptions
    {
        get
        {
            return BuildAssetBundleOptions.DeterministicAssetBundle;
        }
    }

    public static List<string> IgnoreList = new List<string>(){"~"};

    public static void ExcuteBuild()
    {
        ClearMarks();
        MarkAsset();

        string exportPath = GetExportPath(EditorUserBuildSettings.activeBuildTarget);
        BuildAssetBundles(exportPath, EditorUserBuildSettings.activeBuildTarget, LZ4Options);

        EditorUtil.OpenFolderAndSelectFile(exportPath);
    }


    public static void MarkAsset()
    {
        AssetBundleBuildConfigs configs = Resources.Load<AssetBundleBuildConfigs>("BuildConfig");
        AssetBundleBuildConfig[] paths = configs.OneFloderOneBundle;
        for (int i = 0; i < paths.Length; i++)
        {
            if (string.IsNullOrEmpty(paths[i].Path))
            {
                Debug.LogErrorFormat("MarkGame error:BuildConfigs contain null  floder.");
                continue;
            }
            MarkOneFloderOneBundle(paths[i].Path, paths[i].CompressType);
        }
        paths = configs.SubFloderOneBundle;
        for (int i = 0; i < paths.Length; i++)
        {
            if (string.IsNullOrEmpty(paths[i].Path))
            {
                Debug.LogErrorFormat("MarkGame error:BuildConfigs contain null  floder.");
                continue;
            }
            MarkSubFloderOneBundle(paths[i].Path, paths[i].CompressType);
        }

        paths = configs.OneAssetOneBundle;
        for (int i = 0; i < paths.Length; i++)
        {
            if (string.IsNullOrEmpty(paths[i].Path))
            {
                Debug.LogErrorFormat("MarkGame error:BuildConfigs contain null  floder.");
                continue;
            }
            MarkOneAssetOneBundle(paths[i].Path, true, paths[i].CompressType);
        }
    }

    /// <summary>
    /// 清空AB文件的标记
    /// </summary>
    public static void ClearMarks()
    {
        string[] names = AssetDatabase.GetAllAssetBundleNames();
        if (names.Length < 1)
            return;
        int startIndex = 0;
        for (int i = 0; i < names.Length; i++)
        {
            string name = names[startIndex];
            EditorUtility.DisplayProgressBar("清理标记中", name, (float)(startIndex + 1) / (float)names.Length);
            AssetDatabase.RemoveAssetBundleName(name, true);
            startIndex++;
            if (startIndex >= names.Length)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
                startIndex = 0;
                break;
            }
        }
    }


    /// <summary>
    /// 标记一个文件夹一个资源包
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="prefix">AB包路径前缀</param>
    public static void MarkOneFloderOneBundle(string path, CompressType type = CompressType.LZMA)
    {
        string fullPath = EditorUtil.FixedWindowsPath(Application.dataPath + "/" + path);
        if (Directory.Exists(fullPath))
        {
            path = GetAssetBundleName(path, type);
            string assetPath = EditorUtil.GetAssetsPath(fullPath);
            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            ai.SetAssetBundleNameAndVariant(path, AssetBundleDef.ABSuffix);
        }
        else
        {
            Debug.LogError("标记文件夹出错,不存在的文件路径:" + fullPath);
        }
    }

    /// <summary>
    /// 标记指定路径下的所有子文件夹
    /// </summary>
    /// <param name="path">指定路径</param>
    /// <param name="prefix">AB包路径前缀</param>
    public static void MarkSubFloderOneBundle(string path, CompressType type = CompressType.LZMA)
    {
        string fullPath = EditorUtil.FixedWindowsPath(Application.dataPath + "/" + path);
        if (Directory.Exists(fullPath))
        {
            string[] paths = Directory.GetDirectories(fullPath);
            for (int i = 0; i < paths.Length; i++)
            {
                string tmpPath = EditorUtil.FixedWindowsPath(paths[i]).Replace(Application.dataPath + "/", string.Empty);
                MarkOneFloderOneBundle(tmpPath, type);
            }
        }
        else
        {
            Debug.LogError("标记子文件夹出错,不存在的文件路径:" + fullPath);
        }
    }


    /// <summary>
    /// 标记文件夹下的所有指定类型资源
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="types">类型白名单</param>
    /// <param name="includeChildDirectory">是否包含子文件夹</param>
    /// <param name="prefix">AB包路径前缀</param>
    public static void MarkOneAssetOneBundle(string path, bool includeChildDirectory = false, CompressType type = CompressType.LZMA)
    {
        string fullPath = EditorUtil.FixedWindowsPath(Application.dataPath + "/" + path);
        if (Directory.Exists(fullPath))
        {
            MarkOneAssetOneBundleWithFullPath(fullPath, includeChildDirectory, type);
        }
        else
        {
            Debug.LogError("标记文件夹下的资源出错,不存在的文件路径:" + fullPath);
        }
    }

    /// <summary>
    /// 标记一个文件夹下的资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="includeChildDirectory">是否包含子文件夹</param>
    public static void MarkOneAssetOneBundleWithFullPath(string path, bool includeChildDirectory, CompressType type = CompressType.LZMA)
    {
        if(IsIgnore(path))return;
        string[] files = Directory.GetFiles(path);
        foreach (var file in files.Where(file => !file.Contains(".meta")))
        {
            string fullPath = EditorUtil.FixedWindowsPath(file);
            string assetPath = EditorUtil.GetAssetsPath(fullPath);
            string extension = Path.GetExtension(fullPath);
            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
            if (ai == null) continue;
            string assetBundlePath = assetPath.Replace("Assets/", "").Replace(extension, string.Empty);
            string assetBundleName = GetAssetBundleName(assetBundlePath, type);
            ai.SetAssetBundleNameAndVariant(assetBundleName, AssetBundleDef.ABSuffix);
        }
        if (includeChildDirectory)
        {
            string[] infos = Directory.GetDirectories(path);
            for (int i = 0; i < infos.Length; i++)
            {
                MarkOneAssetOneBundleWithFullPath(infos[i], includeChildDirectory);
            }
        }
    }

    /// <summary>
    /// 获取AB包的名字
    /// </summary>
    /// <param name="path"></param>
    /// <param name="compressType"></param>
    /// <returns></returns>
    public static string GetAssetBundleName(string path, CompressType compressType)
    {
        string assetBundleName = path.Replace("Resources/", string.Empty);
        assetBundleName = assetBundleName.Insert(assetBundleName.IndexOf("/") + 1, compressType.ToString() + "/");
        return assetBundleName;
    }

    /// <summary>
    /// 开始打包
    /// </summary>
    /// <param name="buildTarget"></param>
    public static void BuildAssetBundles(string exportPath, BuildTarget buildTarget, BuildAssetBundleOptions options)
    {
        if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);
        DateTime oldTime = DateTime.Now;
        BuildPipeline.BuildAssetBundles(exportPath, options, buildTarget);
        TimeSpan span = DateTime.Now.Subtract(oldTime);
        Debug.Log(exportPath + "打包完毕，本次打包耗时:" + span.TotalSeconds + "秒!");
    }


    public static void CreateServerFiles()
    {
        string exportPath = GetExportPath(EditorUserBuildSettings.activeBuildTarget);
        int index = exportPath.LastIndexOf("/");
        string savePath = exportPath.Insert(index, "/ServerFiles");
        if (Directory.Exists(savePath)) Directory.Delete(savePath, true);
        Directory.CreateDirectory(savePath);

//        CopyToServerFiles(exportPath, savePath, games[i], GetVersion());
        ClearManifest(savePath);
    }

    static void CopyToServerFiles(string from, string to, string game, int version)
    {
        from = Path.Combine(from, game.ToLower());
        string format = "{0}/{1}/{2}";
        to = Path.Combine(to, string.Format(format, game, game + version, game.ToLower()));
        if (Directory.Exists(from))
        {
            FileUtils.CopyDir(from, to);
        }
    }

    /// <summary>
    /// 清理多余的Manifest
    /// </summary>
    public static void ClearManifest(string path)
    {
        string fileName = Path.GetFileName(path);
        string fullPath = Path.Combine(path, fileName);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        string[] files = Directory.GetFiles(path, "*.manifest", SearchOption.AllDirectories);
        foreach (var each in files)
        {
            File.Delete(each);
        }
    }


    /// <summary>
    /// 获取导出路径
    /// </summary>
    /// <returns>The export path.</returns>
    /// <param name="target">Target.</param>
    public static string GetExportPath(BuildTarget target)
    {
        string floder = "Other";
        switch (target)
        {
            case BuildTarget.Android:
                floder = "Android";
                break;
            case BuildTarget.iOS:
                floder = "IOS";
                break;
            case BuildTarget.StandaloneWindows:
                floder = "Windows";
                break;
            case BuildTarget.StandaloneWindows64:
                floder = "Windows";
                break;
        }
        return Application.dataPath.Replace("Assets", "BuildABs/" + floder);
    }



    /// <summary>
    /// 是否忽略掉
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static bool IsIgnore(string path)
    {
        for (int i = 0; i < IgnoreList.Count; i++)
        {
            if (path.Contains(IgnoreList[i]))
                return true;
        }
        return false;
    }
}