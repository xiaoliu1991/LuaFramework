---
---Created by Administrator on 2019/5/22 0022.
---
---@class BagInfoView:BaseView
BagInfoView = setclass("BagInfoView",BaseView);

function BagInfoView:Awake()
    self.mContent = self:Find("Content",self.transform);
end

function BagInfoView:Start()
    self:BindEvent();
    self:AddListener();
end

function BagInfoView:Update()

end

--绑定事件（用于子类重写）
function BagInfoView:BindEvent()
    self:AddOnClick("Close",self.OnClose,self.mContent);
end

function BagInfoView:OnClose()
    self:CloseChildView(UIWeidgetCofig.BagInfoView)
end


--添加事件监听（用于子类重写）
function BagInfoView:AddListener()

end

--移除事件监听（用于子类重写）
function BagInfoView:RemoveListener()

end

--界面销毁时调用（用于子类重写）
function BagInfoView:OnDestroy()
    self:RemoveListener();
end

return BagInfoView;