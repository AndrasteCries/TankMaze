extends StaticBody2D

var pos := Vector2i(0, 0)
var bottom_wall = true
var left_wall = true
var visited = false

func _ready():
	pass


func init(x, y):
	pos.x = x
	pos.y = y
	position = pos * 100
	

func destroy_left_wall():
	get_node("LeftWallShape").queue_free()


func destroy_bottom_wall():
	get_node("BottomWallShape").queue_free()


func destroy_collumn_wall():
	print("cogasreogihsrtahbmistor")
	get_node("CollumnShape").queue_free()


func get_spawnpoint_position():
	return get_node("SpawnPoint").global_position
