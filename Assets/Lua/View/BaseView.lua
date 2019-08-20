local BaseView = {};

function BaseView:new()
	return setmetatable({ transform = nil, }, { __index = self });
end

function BaseView:Term()
end

function BaseView:Show()
	if self.transform then
		self.transform.gameObject:SetActive(true);
	end
end

function BaseView:Hide()
	if self.transform then
		self.transform.gameObject:SetActive(false);
	end
end

return BaseView;
