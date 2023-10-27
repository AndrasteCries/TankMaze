extends Node2D

signal generate_started
signal generate_finished 

@onready var _tile_map :TileMap = get_node("TileMap")

enum Cell {OBSTACLE, GROUND, OUTER}

@export var inner_size := Vector2(5, 5)
@export var perimetr_size := Vector2(1, 1)
@export var ground_probability := 0.1

var size := inner_size + 2 * perimetr_size

var _rng :=  RandomNumberGenerator.new()

# Called when the node enters the scene tree for the first time.
func _ready():
	setup()
	generate()
	
func setup():
	var map_size_px := size * _tile_map.cell_quadrant_size
	
func generate():
	emit_signal("generate_started")
#	_generate_perimetr()
#	_generate_inner()
	emit_signal("generate_finished")

#func _generate_perimetr():
#	var path = []
#	for x in [0, size.x - 2]:
#		for y in range(0, size.y - 1):
#			path.append(Vector2(x, y))
#			#_tile_map.set_cell(0, Vector2(x, y), 0, Vector2(0,0))
#	for y in [0, size.y - 2]:
#		for x in range(1, size.x - 2):
#			path.append(Vector2(x, y))
#
#	_tile_map.set_cells_terrain_connect(0, path, 0, 0)
#
#func _generate_inner():
#	var path = []
#	for x in range(1, size.x - 2):
#		for y in range(1, size.y - 2):
#			path.append(Vector2(x, y))
#			#_tile_map.set_cell(0, Vector2(x, y), 0, Vector2(0,0))
#	_tile_map.set_cells_terrain_connect(0, path, 0, 0, false)
