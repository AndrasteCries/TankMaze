extends RigidBody2D

var speed = 400

func _physics_process(delta):
	position += (transform.y) * speed * delta


func _on_timer_timeout():
	print("timeout")
	queue_free()
