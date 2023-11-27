extends CharacterBody2D

@export var speed = 300
@export var rotation_speed = 5

@export var Bullet : PackedScene
@export var big_shot_tower_texture : Texture2D
@export var laser_tower_texture : Texture2D
@export var minigun_tower_texture : Texture2D
@export var rocket_tower_texture : Texture2D
@export var shotgun_tower_texture : Texture2D

@onready var World = get_node("/root/Game")

var steer_angle
var rotation_direction = 0
var alive = true
var respawn_timer = Timer.new()

var bullets_count = 3
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
		self.shoot.rpc()


@rpc("any_peer","call_local")
func shoot():
	if bullets_count > 0 and tower_type == "Normal":
		var b = Bullet.instantiate()
		b.name = name + " bullet №" + str(bullets_count)
		
		var timer_to_death = Timer.new()
		timer_to_death.name = b.name
		timer_to_death.wait_time = 2
		
		add_child(timer_to_death)
		World.add_child(b)
		
		timer_to_death.start()
		b.start($Muzzle.global_position, rotation)
		
		bullets_count -= 1
		await timer_to_death.timeout
		timer_to_death.queue_free()
		bullets_count += 1
	elif bullets_count > 0 and tower_type == "Big_shot":
		pass
	elif bullets_count > 0 and tower_type == "Laser":
		pass
	elif bullets_count > 0 and tower_type == "Minigun":
		pass
	elif bullets_count > 0 and tower_type == "Rocket":
		pass
	elif bullets_count > 0 and tower_type == "Shotgun":
		pass


func _physics_process(delta):
	if $MultiplayerSynchronizer.get_multiplayer_authority() == multiplayer.get_unique_id() and alive:
		move_and_slide()
		get_input(delta)


@rpc("any_peer","call_local")
func _change_tower():
	match tower_type:
			"Big_shot":
				$Tower.texture = big_shot_tower_texture
			"Laser":
				$Tower.texture = laser_tower_texture
			"Minigun":
				$Tower.texture = minigun_tower_texture
				bullets_count = 20
			"Rocket":
				$Tower.texture = rocket_tower_texture
			"Shotgun":
				$Tower.texture = shotgun_tower_texture


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
		bullets_count = 1
		_change_tower()
		area.get_parent().queue_free()

