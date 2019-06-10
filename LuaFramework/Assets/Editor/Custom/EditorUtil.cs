using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorUtil
{
    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool ShowDialog(string content)
    {
        return ShowDialog("提示", content);
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool ShowDialog(string title, string content)
    {
        return EditorUtility.DisplayDialog(title, content, "确定", "取消");
    }

    /// <summary>
    /// 获取资源路径（Asset/aa/bb）
    /// </summary>
    /// <param name="fullPath">系统路径</param>
    /// <returns>资源路径</returns>
    public static string GetAssetsPath(string fullPath)
    {
        fullPath = FixedWindowsPath(fullPath);
        int index = fullPath.LastIndexOf("Assets");
        return fullPath.Substring(index);
    }

    /// <summary>
    /// 修正windows路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string FixedWindowsPath(string path)
    {
        return path.Replace('\\', '/');
    }

    public static string FixedToWindowsPath(string path)
    {
        return path.Replace('/', '\\');
    }

    /// <summary>
    /// 打开文件系统并选中文件
    /// </summary>
    /// <param name="fullPath">完整的文件路径</param>
    public static void OpenFolderAndSelectFile(string fullPath)
    {
        fullPath = FixedToWindowsPath(fullPath);
        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
        psi.Arguments = "/e,/select," + fullPath;
        System.Diagnostics.Process.Start(psi);
    }

    /// <summary>
    /// 获取系统全路径
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string GetFullPath(string assetPath)
    {
        int index = Application.dataPath.LastIndexOf("Assets");
        string fullPath = Application.dataPath.Substring(0, index) + assetPath;
        return fullPath;
    }

    /// <summary>
    /// 获取传入路径下所有指定查找文件夹
    /// </summary>
    public static List<string> GetDirectories(string path, string searchPattern, SearchOption searchOption)
    {
        var files = new List<string>();
        var tempfiles = Directory.GetDirectories(path, searchPattern, searchOption);
        for (int j = 0; j < tempfiles.Length; ++j)
        {
            files.Add(tempfiles[j].Replace("\\", "/"));
        }
        return files;
    }


    /// <summary>
    /// 获取传入路径下所有指定查找文件
    /// </summary>
    public static List<string> GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        var files = new List<string>();
        var tempfiles = Directory.GetFiles(path, searchPattern, searchOption);
        for (int j = 0; j < tempfiles.Length; ++j)
        {
            if (!tempfiles[j].Contains(".meta"))
                files.Add(tempfiles[j].Replace("\\", "/"));
        }
        return files;
    }
}