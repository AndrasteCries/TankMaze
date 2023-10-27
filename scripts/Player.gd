extends CharacterBody2D

@export var speed = 500
@export var rotation_speed = 5
@export var Bullet : PackedScene

@onready var World = get_node("/root")

var steer_angle
var rotation_direction = 0

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
		shoot()

func shoot():
	var b = Bullet.instantiate()
	World.add_child(b)
	b.start($Muzzle.global_position, rotation)

func _physics_process(delta):
	move_and_slide()
	get_input(delta)
