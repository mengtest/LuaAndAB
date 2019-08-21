local BaseData = {};

function BaseData:new()
	return setmetatable({ transform = nil, }, { __index = self });
end

function BaseData:load()
	print(self:fileName());
end

function BaseData:fileName()
return "";
end



return BaseData;