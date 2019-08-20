
--MainSceneUI 
LuaTest = {};

local this = LuaTest;
local GameObject = UnityEngine.GameObject;
local Luacomponent = require "UI.LuaComponent";
local LuaClass = require "UI.LuaClass";

function LuaTest:awake(obj)

	print(MainSceneUI.count);

	MainSceneUI.count = 2344;
	MainSceneUI:printCount();

	--调用其他lua脚本，并实例化
	self.LuaComponent = Luacomponent:new();
	self.LuaComponent:getComponent(obj.gameObject);

	--调用Lua脚本的C#Class类
	self.LuaClass = LuaClass:new();
	self.LuaClass:Create(obj.gameObject);


end



function LuaTest.Init(object)

	print("LuaTest.Init");
	print(object);
	--this.transform = object.transform;

	Util.DebugObj(object);

	this:awake(object);
end


return LuaTest;