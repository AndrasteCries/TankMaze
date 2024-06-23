extends Node2D

@onready var World = get_node("/root/Game")
@export var Bullet : PackedScene

var bullets_count = 1


@rpc("any_peer","call_local")
func shoot():
	if bullets_count > 0:
		var b = Bullet.instantiate()
		b.name = name + "BigBullet bullet №" + str(bullets_count)
		
		var timer_to_death = Timer.new()
		timer_to_death.name = b.name
		timer_to_death.wait_time = 2
		
		add_child(timer_to_death)
		World.add_child(b)
		
		timer_to_death.start()
		b.start($Sprite2D/Muzzle.global_position, rotation)
		
		bullets_count -= 1
		await timer_to_death.timeout
		timer_to_death.queue_free()
		bullets_count += 1
