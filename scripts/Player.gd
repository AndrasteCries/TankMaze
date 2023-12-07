extends CharacterBody2D

@export var speed = 300
@export var rotation_speed = 5

@export var default_tower : PackedScene
@export var big_shot_tower : PackedScene
@export var laser_tower : PackedScene
@export var minigun_tower : PackedScene
@export var rocket_tower : PackedScene
@export var shotgun_tower : PackedScene

@onready var World = get_node("/root/Game")

var steer_angle
var rotation_direction = 0
var alive = true
var respawn_timer = Timer.new()


var tower_type = "Normal"

func _ready():
	add_child(respawn_timer)
	$MultiplayerSynchronizer.set_multiplayer_authority(str(name).to_int())
	var color_id = Lobby.players.get(name.to_int())["Color"]
	modulate = Lobby.colors[color_id]


func get_input(_delta):
	var turn = 0
	if Input.is_action_pressed("D"):
		turn += 1
	if Input.is_action_pressed("A"):
		turn -= 1
	rotation += rotation_speed * turn * _delta
	velocity = Vector2()
	if Input.is_action_pressed("W"):
		velocity = Vector2(0, -speed).rotated(rotation)
	if Input.is_action_pressed("S"):
		velocity = Vector2(0, speed).rotated(rotation)
	
	if Input.is_action_just_released("LMB"):
		$Tower.shoot.rpc()


func _physics_process(delta):
	if $MultiplayerSynchronizer.get_multiplayer_authority() == multiplayer.get_unique_id() and alive:
		move_and_slide()
		get_input(delta)


@rpc("any_peer","call_local")
func change_tower():
	var new_tower
	$Tower.queue_free()
	remove_child($Tower)
	match tower_type:
		"Default":
			new_tower = default_tower.instantiate()
		"Big_shot":
			new_tower = big_shot_tower.instantiate()
		"Laser":
			new_tower = laser_tower.instantiate()
		"Minigun":
			new_tower = minigun_tower.instantiate()
		"Rocket":
			new_tower = rocket_tower.instantiate()
		"Shotgun":
			new_tower = shotgun_tower.instantiate()
	add_child(new_tower)
	new_tower.name = "Tower"

func _on_area_2d_area_entered(area):
	if area.name == "BulletArea" and alive:
		var killer = area.get_parent().name
		area.get_parent().queue_free()
		hide()
		alive = false
		respawn_timer.wait_time = 1.5
		respawn_timer.start()
		await respawn_timer.timeout
		alive = true
		EventBus.player_dead.emit(name, killer)
	elif area.name == "BuffArea" and alive:
		tower_type = area.get_parent().type
		change_tower()
		area.get_parent().queue_free()

