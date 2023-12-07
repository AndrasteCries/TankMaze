extends Node2D

@onready var World = get_node("/root/Game")
@export var Bullet : PackedScene

var bullets_count = 3


@rpc("any_peer","call_local")
func shoot():
	print("shoot")
	if bullets_count > 0:
		var b = Bullet.instantiate()
		b.name = name + " bullet №" + str(bullets_count)
		
		var timer_to_death = Timer.new()
		timer_to_death.name = b.name
		timer_to_death.wait_time = 2
		
		add_child(timer_to_death)
		World.add_child(b)
		
		timer_to_death.start()
		print(rotation)
		b.start($Sprite2D/Muzzle.global_position, get_parent().rotation)
		
		bullets_count -= 1
		await timer_to_death.timeout
		timer_to_death.queue_free()
		bullets_count += 1
