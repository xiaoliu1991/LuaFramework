using UnityEditor;
using UnityEngine;

public class GameManagerWindow : EditorWindow
{
    [MenuItem("FUnityExtends/GM/GMView")]
    public static void Menu()
    {
        GetWindow<GameManagerWindow>(false, "GM工具", true);
    }

    private void Update()
    {
        Repaint();
    }
   


}















