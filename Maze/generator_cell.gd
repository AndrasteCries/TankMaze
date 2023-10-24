extends Node2D

var size := Vector2i(10, 10)
@export var Cell_scene : PackedScene
@export var _seed := 1


var _rng = RandomNumberGenerator.new()
var _maze = []
# Called when the node enters the scene tree for the first time.
func _ready():
	_rng.seed = _seed
	_maze = create_array_maze()
	backtracking_path()

func create_array_maze():
	var maze = []
	
	#create
	for i in range(0, size.x + 1):
		maze.append([])
		for j in range(0, size.y + 1):
			var cell = Cell_scene.instantiate()
			cell.init(i, j)
			maze[i].append(cell)
			add_child(cell)
			
	#destroy extra
	for i in range(0, size.x + 1):
		maze[i][0].destroy_left_wall()
	for i in range(0, size.y + 1):
		maze[maze.size() - 1][i].destroy_bottom_wall()
	
	return maze

func backtracking_path():
	var current_cell = _maze[9][9]
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
			print("cell")
			print(chosen_cell.pos.x)
			print(chosen_cell.pos.y)
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
			current.bottom_wall = false
			current.destroy_bottom_wall()
		else:
			chosen.bottom_wall = false
			chosen.destroy_bottom_wall()
	else:
		if (current.pos.x > chosen.pos.x):
			current.left_wall = false
			current.destroy_left_wall()
		else:
			chosen.left_wall = false
			chosen.destroy_left_wall()
	
	
	


