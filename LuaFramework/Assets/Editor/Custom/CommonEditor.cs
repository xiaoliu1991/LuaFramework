using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CommonEditor
{

    [MenuItem("FUnityExtends/UnityUtils/ChangeActive %#&E")]
    public static void ChangeActive()
    {
        Transform[] transfs = Selection.transforms;
        if (transfs.Length == 0)
        {
            EditorUtility.DisplayDialog("No selected active gameObject", "No selected active gameObject", "Ok");
            return;
        }
        for (int i = 0; i < transfs.Length; i++)
        {
            transfs[i].gameObject.SetActive(!transfs[i].gameObject.activeSelf);
        }

    }
}