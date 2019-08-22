require "Common.define"
require "Common.globe"

LuaTest2 = {};
local this = LuaTest2;
local GameObject = UnityEngine.GameObject;
function LuaTest2:awake(obj)
	print("这是LuaTest2");
	StartCoroutine(function() this:loadIE(); end);
end


--协成
function LuaTest2:loadIE()

	--找到物体，修改值
	local go =  GameObject.Find("Canvas/Button/Text");
	print(go);
	--获取该物体的组件
	local text = go:GetComponent(typeof(UnityEngine.UI.Text));
	text.text = "Lua脚本修改的";

	Yield(0);
	Yield(null);
	WaitForFixedUpdate();
	WaitForSeconds(1);
	print("11");
	WaitForSeconds(1);
	print("22");
	local x = 1;
	while x<10 do
		WaitForSeconds(1);
		print(x);
		x = x+1;
		text.text = "周忠年0000:"..x;
	end

	for i=1,10 do
		WaitForSeconds(1);
		print(i);
		text.text = "周忠年3333:"..i;
	end

end


function LuaTest2.Init(object)
	this:awake(object);
end

return LuaTest2;