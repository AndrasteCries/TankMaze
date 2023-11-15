extends CharacterBody2D

var speed = 200


func start(_position, _direction):
	rotation = _direction
	position = _position
	velocity = Vector2(0, -speed).rotated(rotation)

func _physics_process(delta):
	var collision = move_and_collide(velocity * delta)
	if collision:
		velocity = velocity.bounce(collision.get_normal())

func _on_timer_timeout():
	queue_free()
