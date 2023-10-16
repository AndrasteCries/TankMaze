extends RigidBody2D

@export var speed = 5
@export var rotation_speed = 3
@export var Bullet : PackedScene

var rotation_direction = 0
var velocity = 0

func get_input():
	var move_direction = Input.get_axis("Forward", "Back")
	rotating(1)
	if move_direction != 0:
		rotating(-move_direction)
	position += transform.y * move_direction * speed
	if Input.is_action_just_released("Shoot"):
		shoot()

func rotating(moving):
	rotation_direction = 0
	if Input.is_action_pressed("Rotate_right"):
		rotation_direction = 1 * moving
	if Input.is_action_pressed("Rotate_left"):
		rotation_direction = -1 * moving

func shoot():
	var b = Bullet.instantiate()
	owner.add_child(b)
	b.transform = $Muzzle.global_transform

func _process(delta):
	get_input()
	rotation += rotation_direction * rotation_speed * delta
