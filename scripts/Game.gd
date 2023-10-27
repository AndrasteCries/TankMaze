extends Node2D

@onready var Generator = get_node("generator")
@onready var Player = preload("res://scenes/Player.tscn")

var _maze 

# Called when the node enters the scene tree for the first time.
func _ready():
	_maze = Generator.finish_maze()
	spawn_maze()
	spawn_player()
	print("start")
	
	
func _spawn_player():
	print("spawn")

func spawn_maze():
	for i in range(_maze.size()):
		for j in range(_maze[i].size()):
			add_child(_maze[i][j])

func spawn_player():
	var player = Player.instantiate()
	var x = randi_range(1, _maze[0].size() - 2)
	var y = randi_range(1, _maze.size() - 2)
	add_child(player)
	player.global_position = _maze[x][y].get_spawnpoint_position()
	player.rotation = randf_range(0, 2*PI)
