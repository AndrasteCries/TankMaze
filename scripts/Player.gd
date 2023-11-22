extends CharacterBody2D

@export var speed = 300
@export var rotation_speed = 5
@export var Bullet : PackedScene
@onready var World = get_node("/root/Game")

var steer_angle
var rotation_direction = 0
var alive = true
var respawn_timer = Timer.new()

var bullets_count = 3

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
	if(bullets_count > 0):
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

func _physics_process(delta):
	if $MultiplayerSynchronizer.get_multiplayer_authority() == multiplayer.get_unique_id() and alive:
		move_and_slide()
		get_input(delta)


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
	pass

