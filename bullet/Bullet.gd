extends CharacterBody2D

var speed = 100
#var collision: KinematicCollision2D

func start(_position, _direction):
	rotation = _direction
	position = _position
	velocity = Vector2(0, -speed).rotated(rotation)

func _physics_process(delta):
	var collision = move_and_collide(velocity * delta)
	if collision:
		velocity = velocity.bounce(collision.get_normal())

func _on_timer_timeout():
	print("timeout")
	queue_free()
