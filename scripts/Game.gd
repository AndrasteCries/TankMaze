extends Node2D

@onready var Generator = get_node("generator")
@onready var Player = preload("res://scenes/Player.tscn")

var _maze 

func _ready():
	_maze = Generator.finish_maze()
	spawn_maze()
	print(Lobby.players)
	for player in Lobby.players:
		spawn_player(player)
		print(player)
	EventBus.player_dead.connect(_respawn_player)


func _spawn_player():
	print("spawn")


func spawn_maze():
	for i in range(_maze.size()):
		for j in range(_maze[i].size()):
			add_child(_maze[i][j])


func spawn_player(peer_id):
	var player = Player.instantiate()
	player.name = str(peer_id)
	var x = randi_range(1, _maze[0].size() - 2)
	var y = randi_range(1, _maze.size() - 2)
	add_child(player)
	player.global_position = _maze[x][y].get_spawnpoint_position()
	player.rotation = randf_range(0, 2 * PI)


func _respawn_player(peer_id):
	self.respawn_player.rpc(peer_id)


@rpc("any_peer")
func respawn_player(peer_id):
	var player = get_node(str(peer_id))
	var x = randi_range(1, _maze[0].size() - 2)
	var y = randi_range(1, _maze.size() - 2)
	player.global_position = _maze[x][y].get_spawnpoint_position()
	player.show()
