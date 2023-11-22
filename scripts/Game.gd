extends Node2D

@onready var Generator = get_node("generator")
@onready var Player = preload("res://scenes/Player.tscn")

signal score_refresh

var _maze 

func _ready(): 
	EventBus.player_dead.connect(_respawn_player)
	multiplayer.server_disconnected.connect(_server_disconnected)
	
	_maze = Generator.finish_maze()
	spawn_maze()
	spawn_players()


func spawn_maze():
	for i in range(_maze.size()):
		for j in range(_maze[i].size()):
			add_child(_maze[i][j])


func spawn_players():
	for player in Lobby.players:
			spawn_player(player)


func spawn_player(peer_id):
	var player = Player.instantiate()
	player.name = str(peer_id)
	var x = randi_range(1, _maze.size() - 2)
	var y = randi_range(1, _maze[0].size() - 2)
	add_child(player)
	player.global_position = _maze[x][y].get_spawnpoint_position()
	player.rotation = randf_range(0, 2 * PI)
	score_refresh.emit()


func _respawn_player(peer_id, killer):
	respawn_player.rpc(peer_id, killer)


@rpc("any_peer")
func respawn_player(peer_id, killer):
	var player = get_node(str(peer_id))
	var x = randi_range(1, _maze.size() - 2)
	var y = randi_range(1, _maze[0].size() - 2)
	if  peer_id.to_int() == killer.split(" ")[0].to_int():
		Lobby.players.get(killer.split(" ")[0].to_int())["Score"] -= 1
	else:
		Lobby.players.get(killer.split(" ")[0].to_int())["Score"] += 1
	player.global_position = _maze[x][y].get_spawnpoint_position()
	player.show()
	score_refresh.emit()


func _server_disconnected():
	get_tree().change_scene_to_file("res://scenes/MainMenu.tscn")
