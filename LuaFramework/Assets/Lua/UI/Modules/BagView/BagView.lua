---
---Created by Administrator on 2019/5/21 0021.
---
---@class BagView:BaseView
BagView = setclass("BagView",BaseView)

function BagView:Awake()
    self.mContent = self:Find("Content",self.transform);
    ---@type UnityEngine.Transform
    self.mScroll = self:Find("Scroll/Content",self.mContent);
end

function BagView:Start()
    self:BindEvent();
    self:AddListener();
end

function BagView:Update()

end

--绑定事件（用于子类重写）
function BagView:BindEvent()
    self:AddOnClick("Close",self.OnClose,self.mContent);
    for i = 1, self.mScroll.childCount do
        local itemBtn = self.mScroll:GetChild(i-1):GetComponent(UIComponent.Button);
        self:AddBtnOnClick(itemBtn,self.OnItemClick)
    end
end

function BagView:OnClose()
    dispatchEvent(UIEvents.BagViewClose);
end


function BagView:OnItemClick()
    self:OpenChildView(UIWeidgetCofig.BagInfoView)
end


--添加事件监听（用于子类重写）
function BagView:AddListener()

end

--移除事件监听（用于子类重写）
function BagView:RemoveListener()

end

--界面销毁时调用（用于子类重写）
function BagView:OnDestroy()
    self:RemoveListener();
end

return BagView;