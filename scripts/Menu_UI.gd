extends Control

var PlayerList : ItemList

# Called when the node enters the scene tree for the first time.
func _ready():
	get_node("Menu").show()
	get_node("Panel").hide()	
	PlayerList = get_node("Panel/Players2/PlayersList")


func list_refresh():
	PlayerList.clear()
	for player in Lobby.players.values():
		PlayerList.add_item(player.get("Nickname"))


func _on_main_menu_player_connected(id, peer_info):
	list_refresh()


func _on_main_menu_player_disconnected():
	list_refresh()


func _on_main_menu_server_disconnected():
	get_node("Menu").show()
	get_node("Panel").hide()


func _on_main_menu_server_start():
	get_node("Menu").hide()
	get_node("Panel").show()


func _on_start_game_pressed():
	owner.load_game.rpc("res://scenes/Game.tscn")
	pass


func _on_exit_pressed():
	Lobby.players.clear()
	multiplayer.multiplayer_peer = null
	get_node("Panel").hide()
	get_node("Menu").show()
