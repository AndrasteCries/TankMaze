extends Node2D

@export var big_shot_buff_texture : Texture2D
@export var laser_buff_texture : Texture2D
@export var minigun_buff_texture : Texture2D
@export var rocket_buff_texture : Texture2D
@export var shotgun_buff_texture : Texture2D

var type = "buff"


func _ready():
	pass


func set_type(rng_type):
	match rng_type:
		0:
			$BuffSprite.texture = big_shot_buff_texture
			type = "Big_shot"
		1:
			$BuffSprite.texture = laser_buff_texture
			type = "Laser"
		2:
			$BuffSprite.texture = minigun_buff_texture
			type = "Minigun"
		3:
			$BuffSprite.texture = rocket_buff_texture
			type = "Rocket"
		4:
			$BuffSprite.texture = shotgun_buff_texture
			type = "Shotgun"

