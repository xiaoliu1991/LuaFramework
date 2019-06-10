using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Paths
{
    /// <summary>
    /// StringBuilder
    /// </summary>
    static StringBuilder stringBuilder = new StringBuilder();

    /// <summary>
    /// 获取完整路径
    /// </summary>
    /// <returns>The full path.</returns>
    /// <param name="keyPath">Key path.</param>
    public static string GetFullPath(string keyPath)
    {
        stringBuilder.Length = 0;
        string path = stringBuilder.Append(AssetBundleDef.PersistentDataPath).Append(keyPath).ToString();
        if (!File.Exists(path))
        {
            stringBuilder.Length = 0;
            path = stringBuilder.Append(AssetBundleDef.StreamPath).Append(keyPath).ToString();
        }
        return path;
    }
}