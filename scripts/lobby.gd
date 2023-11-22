extends Node2D

var players = {}
var player_info = {"Nickname" : "Player",
					"Color" : 0,
					"Score" : 0
}

var colors = {
	0 : Color(0, 1, 0), #Green
	1 : Color(0.969, 0, 0), #Red
	2 : Color(0.584, 0, 1), #Purple
	3 : Color(0.172, 0.404, 1), #Blue
}

var players_loaded = 0

var lobby_settings = {
	"Size" : Vector2i(10, 10),
	"Seed" : 1
}

