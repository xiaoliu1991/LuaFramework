using System;
using UnityEngine;

public class ResourcesManager
{
    public static GameObject LoadUI(string name)
    {
        return Resources.Load<GameObject>(name);
    }
}