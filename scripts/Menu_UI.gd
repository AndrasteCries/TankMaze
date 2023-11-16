extends Control


var PlayerList : ItemList


func _ready():
	get_node("Menu").show()
	get_node("Party").hide()
	PlayerList = get_node("Party/Players2/PlayersList")


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
	get_node("Party").hide()


func _on_main_menu_server_start():
	get_node("Menu").hide()
	get_node("Party").show()


func _on_start_game_pressed():
	if multiplayer.is_server():
		owner.load_game.rpc("res://scenes/Game.tscn")
	pass


func _on_exit_pressed():
	Lobby.players.clear()
	multiplayer.multiplayer_peer = null
	get_node("Party").hide()
	get_node("Menu").show()


func _on_join_pressed():
	get_node("Menu").hide()
	get_node("Party").show()


func _on_host_pressed():
	get_node("Menu").hide()
	get_node("Party").show()
