require "Common.define"
require "Common.globe"

--MainSceneUI 
LuaTest = {};

local this = LuaTest;
local GameObject = UnityEngine.GameObject;
local Luacomponent = require "UI.LuaComponent";
local LuaClass = require "UI.LuaClass";
local People = require "UI.PeopleData";
local  Parent =  require "View.Parent";

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

	--调用peopledata父子关系类--
	self.People = People:new();
	self.People:load();

	--找到物体，修改值
	local go =  GameObject.Find("Canvas/Button/Text");
	print(go);
	--获取该物体的组件
	local text = go:GetComponent(typeof(UnityEngine.UI.Text));
	text.text = "Lua脚本修改的";

	--绑定按钮的事件
	local btn =  GameObject.Find("Canvas/Button");
	UIEvent.AddButtonClick(btn,function(game)
		-- 点击按钮时执行
		print("按钮被点击了");
	end);

	--非setmetatable 的实例化
	self.Parent = Parent:new();
	self.Parent:set();
	print(self.Parent.name);

	self.Parent = Parent:new();
	print(self.Parent.name);

	--Lua脚本的GameObject的实例化
	 local _luax = GameObject("_luax");
	 local _mainLuax = _luax:AddComponent(typeof(MainBehaviourLua));
     _mainLuax:Set("LuaTest2");


end



function LuaTest.Init(object)

	print("LuaTest.Init");
	print(object);
	--this.transform = object.transform;

	Util.DebugObj(object);

	this:awake(object);
end


return LuaTest;