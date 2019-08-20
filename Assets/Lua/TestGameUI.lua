local BaseView = require "View.BaseView"
local TestGameUI = BaseView:new();

function TestGameUI:Init(transform)
   print("初始化");
end

function TestGameUI:Term()
   
end

function TestGameUI:Show()
  
end



return TestGameUI;