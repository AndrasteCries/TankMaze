extends Node2D

@export var size := Vector2i(10, 10)
@export var Cell_scene : PackedScene
@export var random_walls_count := 10
@onready var World = get_node("/root")

var Player = preload("res://scenes/Player.tscn") 

var _rng = RandomNumberGenerator.new()
var _maze = []


func _ready():
	_rng.seed = Lobby.lobby_settings["Seed"]
	size = Lobby.lobby_settings["Size"]


func create_array_maze():
	var maze = []
	
	#create
	for i in range(0, size.x + 1):
		maze.append([])
		for j in range(0, size.y + 1):
			var cell = Cell_scene.instantiate()
			cell.init(i, j)
			cell.name = str(i) + " " + str(j)
			maze[i].append(cell)

	#destroy extra
	for i in range(0, size.x + 1):
		maze[i][0].destroy_left_wall()
		maze[i][0].destroy_floor()
	for i in range(0, size.y + 1):
		maze[maze.size() - 1][i].destroy_bottom_wall()
		maze[maze.size() - 1][i].destroy_floor()

	return maze

func backtracking_path():
	var current_cell = _maze[0][1]
	current_cell.visited = true
	var stack = []
	while true:
		var unvisited_cells = []
		var x = current_cell.pos.x
		var y = current_cell.pos.y

		if x > 0 && not _maze[x - 1][y].visited:
			unvisited_cells.append(_maze[x - 1][y])
		if y > 1 && not _maze[x][y - 1].visited:
			unvisited_cells.append(_maze[x][y - 1])
		if x < size.x - 1 && not _maze[x + 1][y].visited:
			unvisited_cells.append(_maze[x + 1][y])
		if y < size.y && not _maze[x][y + 1].visited:
			unvisited_cells.append(_maze[x][y + 1])

		if unvisited_cells.size() > 0:
			var chosen_cell = unvisited_cells[_rng.randi_range(0, unvisited_cells.size() - 1)]
			remove_wall(current_cell, chosen_cell)
			chosen_cell.visited = true
			stack.append(current_cell)
			current_cell = chosen_cell
		else:
			if stack.size() > 0:
				current_cell = stack.pop_back()
			else:
				break

func remove_wall(current, chosen):
	if (current.pos.x == chosen.pos.x):
		if (chosen.pos.y > current.pos.y):
			current.destroy_bottom_wall()
		else:
			chosen.destroy_bottom_wall()
	else:
		if (current.pos.x > chosen.pos.x):
			current.destroy_left_wall()
		else:
			chosen.destroy_left_wall()

func remove_random_wall():
	var q = 0
	while q < random_walls_count:
		var x = _rng.randi_range(1, _maze.size() - 2)
		var y = _rng.randi_range(1, _maze[0].size() - 2)
		if _maze[x][y].left_wall:
			_maze[x][y].destroy_left_wall()
		elif _maze[x][y].bottom_wall:
			_maze[x][y].destroy_bottom_wall()
		else: q -= 1
		q += 1


func remove_extra_collumn():
	for x in range(0, size.x + 1):
		for y in range(0, size.y + 1):
			if !_maze[x][y].left_wall and !_maze[x][y].bottom_wall:
				if !_maze[x - 1][y].bottom_wall and !_maze[x][y + 1].left_wall:
					_maze[x][y].destroy_collumn_wall()

func finish_maze():
	_maze = create_array_maze()
	backtracking_path()
	remove_random_wall()
	remove_extra_collumn()
	return _maze

