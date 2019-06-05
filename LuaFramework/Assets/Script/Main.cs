public class Main : BaseMono
{
    public static Main Inst;

    /// <summary>
    /// Lua管理器
    /// </summary>
    public static LuaManager LuaMgr
    {
        get
        {
            return LuaManager.Inst;
        }
    }

    public LogLevel log;

    public override void Awake()
    {
        base.Awake();
        Inst = this;
        Log.level = log;
        LuaMgr.StartMain();
    }


    /// <summary>
    /// 应用程序获得焦点/失去焦点
    /// </summary>
    /// <param name="hasFocus"></param>
    void OnApplicationFocus(bool hasFocus)
    {
        LuaMgr.CallFunction("OnApplicationFocus", hasFocus);
    }

    /// <summary>
    /// 应用程序暂停/恢复
    /// </summary>
    /// <param name="pauseStatus"></param>
    void OnApplicationPause(bool pauseStatus)
    {
        LuaMgr.CallFunction("OnApplicationPause", pauseStatus);
    }

    /// <summary>
    /// 应用程序退出
    /// </summary>
    void OnApplicationQuit()
    {
        LuaMgr.CallFunction("OnApplicationQuit");
    }
}
