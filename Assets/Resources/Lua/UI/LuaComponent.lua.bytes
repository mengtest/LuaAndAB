--主要用于组件的获取 组件的测试
--LuaComponent 
LuaComponent = {};
local GameObject = UnityEngine.GameObject;

function LuaComponent:new()
	print("LuaComponent:new");
	return setmetatable({ set = {} }, { __index = self} );
end


function  LuaComponent:getComponent(Obj)
	print("LuaComponent:getComponent");
	print(Obj);
	--添加绑定组件
	--Obj:AddComponent(typeof(AudioSource));
	Obj:AddComponent(typeof(UnityEngine.AudioSource));


end


return LuaComponent;