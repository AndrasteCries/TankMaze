extends CharacterBody2D

var speed = 200

var prcnt = 1
var live_time = 2.0

func start(_position, _direction):
	rotation = _direction
	position = _position
	velocity = Vector2(0, -speed).rotated(rotation)
	$Timer_to_live.wait_time = live_time
	$Timer_to_live.start()


func _physics_process(delta):
	if $Timer_to_live.time_left < 0.5:
		prcnt -= delta * 2
	modulate = Color(1, 1, 1, prcnt)
	var collision = move_and_collide(velocity * delta)
	if collision:
		velocity = velocity.bounce(collision.get_normal())


func _on_timer_to_live_timeout():
	queue_free()


func _on_timer_collide_timeout():
	$BulletArea/CollisionShape2D.disabled = false

