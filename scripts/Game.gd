extends Node2D

@onready var Generator = get_node("generator")
@onready var Player = preload("res://scenes/Player.tscn")
@onready var Buff = preload("res://scenes/buff.tscn")

signal score_refresh

var _maze 

var _rng :=  RandomNumberGenerator.new()

func _ready(): 
	_rng.seed = Lobby.lobby_settings["Seed"]
	EventBus.player_dead.connect(_respawn_player)
	multiplayer.server_disconnected.connect(_server_disconnected)
	$BuffSpawnTimer.timeout.connect(_spawn_buff)
	_maze = Generator.finish_maze()
	spawn_maze()
	spawn_players()


func spawn_maze():
	for i in range(_maze.size()):
		for j in range(_maze[i].size()):
			$generator.add_child(_maze[i][j])


func spawn_players():
	for player in Lobby.players:
			spawn_player(player)


func spawn_player(peer_id):
	var player = Player.instantiate()
	player.name = str(peer_id)
	var random_x_y = random_cell()
	add_child(player)
	player.global_position = _maze[random_x_y.x][random_x_y.y].get_spawnpoint_position()
	player.rotation = randf_range(0, 2 * PI)
	score_refresh.emit()


func _respawn_player(peer_id, killer):
	respawn_player.rpc(peer_id, killer)


@rpc("any_peer")
func respawn_player(peer_id, killer):
	var player = get_node(str(peer_id))
	var random_x_y = random_cell()
	if  peer_id.to_int() == killer.split(" ")[0].to_int():
		Lobby.players.get(killer.split(" ")[0].to_int())["Score"] -= 1
	else:
		Lobby.players.get(killer.split(" ")[0].to_int())["Score"] += 1
	player.global_position = _maze[random_x_y.x][random_x_y.y].get_spawnpoint_position()
	player.show()
	score_refresh.emit()

func _spawn_buff():
	var x = randi_range(0, _maze.size() - 2)
	var y = randi_range(1, _maze[0].size() - 1)
	spawn_buff(x, y)

@rpc("any_peer")
func spawn_buff(x, y):
	var buff = Buff.instantiate()
	buff.set_type(_rng.randi_range(0, 2))
	add_child(buff)
	buff.name = "Buff" + str(buff.type)
	buff.global_position = _maze[x][y].get_spawnpoint_position()
	buff.rotation = randf_range(0, 2 * PI)


func _server_disconnected():
	get_tree().change_scene_to_file("res://scenes/MainMenu.tscn")


func random_cell():
	var x = randi_range(0, _maze.size() - 2)
	var y = randi_range(1, _maze[0].size() - 1)
	return Vector2i(x, y)
