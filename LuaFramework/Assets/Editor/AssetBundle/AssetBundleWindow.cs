using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleWindow : EditorWindow {

    [MenuItem("Build/AssetBundleWindow")]
    static void Init()
    {
        AssetBundleWindow window = (AssetBundleWindow)GetWindow(typeof(AssetBundleWindow));
        window.Show();
    }

    public void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        DrawGUI();
    }

    public void DrawGUI()
    {
        //一键打AB包
        if (GUILayout.Button("增量打包", GUILayout.Height(40)))
        {
            if (EditorUtil.ShowDialog("提示", "确定要开始增量打包吗?"))
            {
                AssetBuilder.ExcuteBuild();
            }
        }

        //一键打AB包
        if (GUILayout.Button("全量打包", GUILayout.Height(40)))
        {
            if (EditorUtil.ShowDialog("提示", "确定要开始全量打包吗?"))
            {
                string exportPath = Application.streamingAssetsPath;
                if (Directory.Exists(exportPath)) Directory.Delete(exportPath, true);
                AssetBuilder.ExcuteBuild();
            }
        }
    }
}
