--定义
require('Common/Define')
require('Common/Functions')
require('Common/Class')
require('UI/EventDef')
require('Event/GameEvent')
--Config
require('Config/UIConfig')

local Game = require('Game')

function Main()
	log("logic start")
	Game.Init()
end


function OnApplicationFocus(state)
	warn("OnApplicationFocus: " .. tostring(state))
end

function OnApplicationPause(state)
	warn("OnApplicationPause： " .. tostring(state))
end

function OnApplicationQuit()
	warn("OnApplicationQuit")
end