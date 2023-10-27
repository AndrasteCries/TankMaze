extends Node


func _init() -> void:
	# Set your game's Steam app ID here
	OS.set_environment("SteamAppId", str(480))
	OS.set_environment("SteamGameId", str(480))
