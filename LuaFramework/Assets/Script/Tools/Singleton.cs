using UnityEngine;

public abstract class Singleton<T> where T : class, new()
{
    private static T instance;

    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}


public class UnitySingleton<T> : BaseMono where T : BaseMono
{
    private static T instance;

    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                if ((instance = Object.FindObjectOfType<T>()) == null)
                {
                    GameObject go = new GameObject(typeof(T).ToString());
                    instance = go.AddComponent<T>();
                }
                if (Application.isPlaying) DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
}
