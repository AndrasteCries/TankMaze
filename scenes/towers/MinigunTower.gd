extends Node2D

@onready var World = get_node("/root/Game")
@export var Bullet : PackedScene

var bullets_count = 20

@rpc("any_peer","call_local")
func shoot():
	while bullets_count > 0:
		var b = Bullet.instantiate()
		b.name = name + "Minigun bullet №" + str(bullets_count)
		
		var reload_timer = Timer.new()
		reload_timer.name = b.name
		reload_timer.wait_time = 0.1
		
		add_child(reload_timer)
		World.add_child(b)
		
		reload_timer.start()
		b.start($Sprite2D/Muzzle.global_position, get_parent().rotation)
		
		bullets_count -= 1
		await reload_timer.timeout
		reload_timer.queue_free()
		if bullets_count == 0:
			get_parent().tower_type = "Default"
			get_parent().change_tower()
