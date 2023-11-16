extends Node2D

var players = {}
var player_info = {"Nickname" : "Player",
					"Color" : 0,
					"Score" : 0}
var colors = {
	0 : Color(1, 1, 1, 1), #Normal
	1 : Color(0.969, 0, 0, 1), #Red
	2 : Color(0.584, 0, 1), #Purple
	3 : Color(0.172, 0.404, 1), #Blue
}
var players_loaded = 0

