extends CharacterBody2D

@export var triangles_count = 20
@export var Triangle : PackedScene

@onready var World = get_node("/root/Game")

var speed = 200

var prcnt = 1
var live_time = 1.0

var _rng :=  RandomNumberGenerator.new()

func start(_position, _direction):
	_rng.seed = Lobby.lobby_settings["Seed"]
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
	boom()
	queue_free()


@rpc("any_peer","call_local")
func boom():
	for i in range(triangles_count):
		var triangle = Triangle.instantiate()
		triangle.name = "trinagle"
		World.add_child(triangle)
		
		triangle.start(global_position, _rng.randi_range(0, 100))

func _on_timer_collide_timeout():
	$BulletArea/CollisionShape2D.disabled = false
