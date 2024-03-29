﻿using System;
using LuaInterface;
using UnityEngine.SceneManagement;

public class LuaManager : UnitySingleton<LuaManager>
{
    protected LuaState luaState = null;
    protected LuaLooper loop = null;

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    protected void Init()
    {
        InitLoader();
        luaState = new LuaState();
        OpenLibs();
        luaState.LuaSetTop(0);
        Bind();
    }

   
    public void StartMain()
    {
        luaState.Start();
        StartLooper();
        luaState.DoFile("Main.lua");
        CallFunction("Main");
    }

    protected LuaFileUtils InitLoader()
    {
        return LuaFileUtils.Instance;
    }
    protected void OpenLibs()
    {
        luaState.OpenLibs(LuaDLL.luaopen_pb);
        luaState.OpenLibs(LuaDLL.luaopen_struct);
        luaState.OpenLibs(LuaDLL.luaopen_lpeg);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        luaState.OpenLibs(LuaDLL.luaopen_bit);
#endif
    }

    protected void StartLooper()
    {
        loop = gameObject.AddComponent<LuaLooper>();
        loop.luaState = luaState;
    }

    protected virtual void Bind()
    {
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();
        LuaCoroutine.Register(luaState, this);
    }

    public LuaTable GetLuaTable(string funcName)
    {
        return luaState.GetTable(funcName);
    }

    public void CallMethod(string module, string func, params object[] args)
    {
        CallFunction(module + "." + func, args);
    }

    public void CallFunction(string funcName, params object[] args)
    {
        LuaFunction func = luaState.GetFunction(funcName);
        if (func != null)
        {
            func.BeginPCall();
            func.PushArgs(args);
            func.PCall();
            func.EndPCall();
            func = null;
        }
    }
   
    public override void OnDestroy()
    {
        base.OnDestroy();
        if (luaState != null)
        {
            LuaState state = luaState;
            luaState = null;
            if (loop != null)
            {
                loop.Destroy();
                loop = null;
            }
            state.Dispose();
        }
    }
}
