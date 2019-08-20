require "Common.define"
require "Common.globe"


local BaseView = require "View.BaseView"
MainSceneUI = {};-- BaseView:new();

local this = MainSceneUI;
local GameObject = UnityEngine.GameObject;

function MainSceneUI:awake(obj)
	-- body
 
-- print(obj);
 print(obj.name);
print("self.count");
  print(self.count);

self.count = 100;



end

function MainSceneUI:printCount()
  print(self.count);

end


	

function MainSceneUI.Init(object)

  
	print("MainSceneUI.Init");
	print(object);
	--this.transform = object.transform;

Util.DebugObj(object);

	 this:awake(object);
end


return MainSceneUI;