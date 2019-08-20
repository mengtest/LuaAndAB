--主要印证，Lua脚本调用C#的Class类
--LuaClass
LuaClass = {};
local GameObject = UnityEngine.GameObject;

function LuaClass:new()
	print("LuaClass:new");
	return setmetatable({ set = {} }, { __index = self} );
end


function  LuaClass:Create(Obj)
--创建C#的Class类
local  _xx  =  MyPeople();
print(_xx.Name); 
_xx:Set("123");
print(_xx.Name); 

end


return LuaClass;