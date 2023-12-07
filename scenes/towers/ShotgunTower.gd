extends Node2D

@onready var World = get_node("/root/Game")
@export var Bullet : PackedScene

var bullets_count = 6

var flag = 0


@rpc("any_peer","call_local")
func shoot():
	if bullets_count > 0:
		var b = Bullet.instantiate()
		b.name = name + "ShotgunBullet bullet №" + str(bullets_count)
		
		World.add_child(b)

		if flag == 0:
			b.start($Sprite2D/Muzzle.global_position, get_parent().rotation)
			flag = 1
		else:
			b.start($Sprite2D/Muzzle2.global_position, get_parent().rotation)
			flag = 0
		
		bullets_count -= 1
		if bullets_count == 0:
			get_parent().tower_type = "Default"
			get_parent().change_tower()
