extends Node2D

signal player_connected(peer_id, player_info)
signal player_disconnected(peer_id)
signal server_disconnected
signal server_start

var NicknameEdit : TextEdit
var Ip : TextEdit
var Port : TextEdit
var ColorList : ItemList


func _ready():
	multiplayer.peer_connected.connect(_on_player_connected)
	multiplayer.peer_disconnected.connect(_on_player_disconnected)
	multiplayer.connected_to_server.connect(_on_connected_ok)
	multiplayer.connection_failed.connect(_on_connected_fail)
	multiplayer.server_disconnected.connect(_on_server_disconnected)
	NicknameEdit = get_node("UI/Menu/NickLabel/Nickname")
	Ip = get_node("UI/Menu/IpLabel/IP")
	Port = get_node("UI/Menu/PortLabel/Port")
	ColorList = get_node("UI/Menu/ColorLabel/Colors")


func _on_join_pressed():
	var peer = ENetMultiplayerPeer.new()
	var error = peer.create_client(Ip.text, Port.text.to_int())
	if error:
		print(error)
		return error
	Lobby.player_info["Nickname"] = NicknameEdit.text
	var ChColor = ColorList.get_selected_items()
	if ChColor.size() > 0:
		Lobby.player_info["Color"] = ChColor[0]
	multiplayer.multiplayer_peer = peer


func _on_host_pressed():
	var peer = ENetMultiplayerPeer.new()
	var error = peer.create_server(Port.text.to_int(), 4)
	if error:
		print(error)
		return error
	multiplayer.set_multiplayer_peer(peer)
	Lobby.player_info["Nickname"] = NicknameEdit.text
	var ChColor = ColorList.get_selected_items()
	if ChColor.size() > 0:
		Lobby.player_info["Color"] = ChColor[0]
		
	Lobby.players[1] = Lobby.player_info
	server_start.emit()
	player_connected.emit(1, Lobby.player_info)

 
func remove_multiplayer_peer():
	multiplayer.multiplayer_peer = null


@rpc("any_peer", "call_local")
func load_game(game_scene_path):
	get_tree().change_scene_to_file(game_scene_path)


@rpc("any_peer", "call_local", "reliable")
func player_loaded():
	if multiplayer.is_server():
		Lobby.players_loaded += 1
		if Lobby.players_loaded == Lobby.players.size():
			$/root/Game.start_game()
			Lobby.players_loaded = 0


func _on_player_connected(id):
	_register_player.rpc_id(id, Lobby.player_info)


@rpc("any_peer", "reliable")
func _register_player(new_player_info):
	var new_player_id = multiplayer.get_remote_sender_id()
	Lobby.players[new_player_id] = new_player_info
	player_connected.emit(new_player_id, new_player_info)


func _on_connected_ok():
	var peer_id = multiplayer.get_unique_id()
	Lobby.players[peer_id] = Lobby.player_info
	player_connected.emit(peer_id, Lobby.player_info)


func _on_player_disconnected(id):
	Lobby.players.erase(id)
	player_disconnected.emit()


func _on_server_disconnected():
	Lobby.players.clear()
	multiplayer.multiplayer_peer = null
	server_disconnected.emit()


func _on_connected_fail():
	multiplayer.multiplayer_peer = null
