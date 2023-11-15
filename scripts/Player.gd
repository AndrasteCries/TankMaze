extends CharacterBody2D

@export var speed = 300
@export var rotation_speed = 5
@export var Bullet : PackedScene

@onready var World = get_node("/root")

var steer_angle
var rotation_direction = 0

var respawn_timer = Timer.new()

func _ready():
	add_child(respawn_timer)
	$MultiplayerSynchronizer.set_multiplayer_authority(str(name).to_int())


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
	var b = Bullet.instantiate()
	World.add_child(b)
	b.start($Muzzle.global_position, rotation)


func _physics_process(delta):
	if $MultiplayerSynchronizer.get_multiplayer_authority() == multiplayer.get_unique_id():
		move_and_slide()
		get_input(delta)



func _on_area_2d_area_entered(area):
	if area.name == "BulletArea":
		area.get_parent().queue_free()
		self.hide()
		respawn_timer.wait_time = 1.5
		respawn_timer.start()
		await respawn_timer.timeout
		EventBus.player_dead.emit(self.name)
	pass
