local MyCar = {};

function MyCar:new()
	return setmetatable({ transform = nil, }, { __index = self });
end

function MyCar:print()
	print("MyCar");
end

return MyCar;