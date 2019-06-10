using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 文件工具
/// </summary>
public static class FileUtils
{
    /// <summary>
    /// 拷贝文件
    /// </summary>
    public static void CopyFile(string inPath, string outPath)
    {
        try
        {
            string directoryPath = Path.GetDirectoryName(outPath);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            File.Copy(inPath, outPath, true);
        }
        catch (Exception e)
        {
            Log.error("拷贝文件报错:" + e.Message + "  StackTrace::" + e.StackTrace);
        }
    }

    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="path"></param>
    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

    }

    /// <summary>
    /// 写入所有text
    /// </summary>
    public static void WriteAllText(string path, string text)
    {
        try
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            File.WriteAllText(path, text);
        }
        catch (Exception e)
        {
            Log.error("写文本报错:" + e.Message + "  StackTrace::" + e.StackTrace);
        }
    }
    /// <summary>
    /// 写入所有Bytes
    /// </summary>
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        try
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            File.WriteAllBytes(path, bytes);
        }
        catch (Exception e)
        {
            Log.error("写文本报错:" + e.Message + "  StackTrace::" + e.StackTrace);
        }
    }

    /// <summary>
    /// 移动文件或者目录
    /// </summary>
    /// <param name="inFile"></param>
    /// <param name="outFile"></param>
    public static void MoveFile(string inFile, string outFile)
    {
        string dir = Path.GetDirectoryName(outFile);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        if (File.Exists(outFile)) File.Delete(outFile);
        File.Move(inFile, outFile);
    }

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="fromDir"></param>
    /// <param name="toDir"></param>
    public static void CopyDir(string fromDir, string toDir)
    {
        if (!Directory.Exists(fromDir))
            return;
        if (Directory.Exists(toDir))
        {
            Directory.Delete(toDir, true);
        }
        Directory.CreateDirectory(toDir);
        string[] files = Directory.GetFiles(fromDir);
        foreach (string formFileName in files)
        {
            string fileName = Path.GetFileName(formFileName);
            string toFileName = Path.Combine(toDir, fileName);
            File.Copy(formFileName, toFileName);
        }
        string[] fromDirs = Directory.GetDirectories(fromDir);
        foreach (string fromDirName in fromDirs)
        {
            string dirName = Path.GetFileName(fromDirName);
            string toDirName = Path.Combine(toDir, dirName);
            CopyDir(fromDirName, toDirName);
        }
    }

    public static void GetAllFilesInPath(string dir, List<string> allFiles, List<string> allDics)
    {
        DirectoryInfo di = new DirectoryInfo(dir);
        if (!di.Exists) return;//如果目录不存在,退出

        var currentDirFiles = di.GetFiles().Select(p => p.FullName);//获取当前目录所有文件
        allFiles.AddRange(currentDirFiles);//将当前目录文件放到allFiles中
        var currentDirSubDirs = di.GetDirectories().ToList();//获取子目录
        if (currentDirSubDirs.Count == 0)
            allDics.Add(dir);
        else
        {
            currentDirSubDirs.ForEach(p =>
            {
                GetAllFilesInPath(p.FullName, allFiles, allDics);
                if (!allDics.Contains(p.FullName))
                    allDics.Add(p.FullName);
            });//将子目录中的文件放入allFiles中
        }
    }

    public static void DeletePath(string path)
    {
        DirectoryInfo dirs = new DirectoryInfo(path);
        if (dirs == null || (!dirs.Exists))
        {
            return;
        }

        DirectoryInfo[] subDir = dirs.GetDirectories();
        if (subDir != null)
        {
            for (int i = 0; i < subDir.Length; i++)
            {
                if (subDir[i] != null)
                {
                    DeletePath(subDir[i].FullName);
                }
            }
            subDir = null;
        }

        FileInfo[] files = dirs.GetFiles();
        if (files != null)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != null)
                {
                    files[i].Delete();
                    files[i] = null;
                }
            }
            files = null;
        }

        dirs.Delete();
    }

    public static void Delete(string v)
    {
        if (File.Exists(v))
            File.Delete(v);
    }
}
