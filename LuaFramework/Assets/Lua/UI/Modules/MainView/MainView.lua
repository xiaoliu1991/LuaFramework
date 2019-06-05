---
---Created by Administrator on 2019/5/20 0020.
---

---@class MainView:BaseView
MainView = setclass("MainView",BaseView)

function MainView:Awake()
    self.mContent = self:Find("Content",self.transform);
    self.mText = self:GetComponent(UIComponent.Text,"Text",self.mContent);
end

function MainView:Start()
    self.mText.text = "欢迎进入游戏！！！！！";
    self:BindEvent();
    self:AddListener();
end

function MainView:Update()

end

--绑定事件（用于子类重写）
function MainView:BindEvent()
    self:AddOnClick("Close",self.OnExitMain,self.mContent);
    self:AddOnClick("Bag",self.OnBagClick,self.mContent);
end

function MainView:OnExitMain()
    dispatchEvent(UIEvents.MainViewClose);
end

function MainView:OnBagClick()
    dispatchEvent(UIEvents.BagView);
end


--添加事件监听（用于子类重写）
function MainView:AddListener()

end

--移除事件监听（用于子类重写）
function MainView:RemoveListener()

end

--界面销毁时调用（用于子类重写）
function MainView:OnDestroy()
    self:RemoveListener();
end

return MainView;