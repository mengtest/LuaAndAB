

local Parent = {
    

};
Parent.__index = Parent;--相当于self

function Parent:set()
	-- body
	self.name = "124";
end

function Parent:new()
	-- body
	--setmetatable({ isLoaded = false, isLoading = false }, self);
	--return setmetatable({ name = "789", }, { __index = self });
	--return setmetatable({ jj= 1, },  { __index = self } );
	return setmetatable({},  self  );
	--return self;
	
end

return Parent;