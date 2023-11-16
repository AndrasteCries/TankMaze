extends StaticBody2D

var pos := Vector2i(0, 0)
var bottom_wall = true
var left_wall = true
var visited = false

func _ready():
	var floor_color = randi_range(0, 3)
	match floor_color:
		0:
			get_node("Floor").set_color(Color(0.318, 0.318, 0.318))
		1:
			get_node("Floor").set_color(Color(0.447, 0.447, 0.447))
		2:
			get_node("Floor").set_color(Color(0.408, 0.408, 0.408))
	pass

func init(x, y):
	pos.x = x
	pos.y = y
	position = pos * 100


func destroy_left_wall():
	get_node("LeftWallShape").queue_free()
	left_wall = false


func destroy_bottom_wall():
	get_node("BottomWallShape").queue_free()
	bottom_wall = false


func destroy_collumn_wall():
	get_node("CollumnShape").queue_free()


func destroy_floor():
	get_node("Floor").queue_free()


func get_spawnpoint_position():
	return get_node("SpawnPoint").global_position
