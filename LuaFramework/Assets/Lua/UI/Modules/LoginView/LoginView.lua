---
---Created by Administrator on 2019/5/17 0017.
---

---@class LoginView:BaseView
LoginView = setclass("LoginView",BaseView)

function LoginView:Awake()
    self.mContent = self:Find("Content",self.transform);
    self.mLoginBtnText = self:GetComponent(UIComponent.Text,"Login/Text",self.mContent);
end

function LoginView:Start()
    self.mLoginBtnText.text = "Login";
    self:BindEvent();
    self:AddListener();
end

function LoginView:Update()

end

--绑定事件（用于子类重写）
function LoginView:BindEvent()
    log(self.cfg.name .. " BindEvent")
    self:AddOnClick("Login",self.OnLogin,self.mContent)
end

function LoginView:OnLogin(go)
    error("登陆成功~~~~~~~~~~~~~~~~~")
    dispatchEvent(UIEvents.MainView)
end

--添加事件监听（用于子类重写）
function LoginView:AddListener()
    log(self.cfg.name .. " AddListener")
end

--移除事件监听（用于子类重写）
function LoginView:RemoveListener()
    log(self.cfg.name .. " RemoveListener")
end

--界面销毁时调用（用于子类重写）
function LoginView:OnDestroy()
    self:RemoveListener();
end

return LoginView;