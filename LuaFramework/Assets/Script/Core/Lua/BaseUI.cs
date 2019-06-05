
using UnityEngine.UI;

public class BaseUI : BaseMono
{
    private UIInfo uiInfo;
    public override void Awake()
    {
        base.Awake();
        uiInfo = GetComponent<UIInfo>();
        CallLua(uiInfo.View, "Awake");
    }

    public override void Start()
    {
        base.Start();
        CallLua(uiInfo.View, "Start");
    }

    public override void Update()
    {
        base.Update();
        CallLua(uiInfo.View, "Update");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        CallLua(uiInfo.View, "OnDestroy");
    }

    private void CallLua(string module, string func)
    {
        string fun = module + "." + func;
        Main.LuaMgr.CallFunction(fun, LuaManager.Inst.GetLuaTable(module));
    }
}