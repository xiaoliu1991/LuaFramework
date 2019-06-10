﻿using System;
using UnityEditor;

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

}