extends CanvasLayer


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_game_score_refresh():
	$Score/PlayersScore.clear()
	for player in Lobby.players.values():
#		print(player)
		var row = player["Nickname"] + " : " + str(player["Score"])
		$Score/PlayersScore.add_item(row)
