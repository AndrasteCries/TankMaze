using Godot;
using System;
using global::mazetank.scripts.global;

public partial class PlayerItemList : ItemList
{
	public override void _Ready()
	{
		var players = Global.Lobby.GetPlayersList();
		foreach (var player in players)
		{
			var item = player.Nickname + " " + player.PlayerColor;
			AddItem(item);
		}
	}

	public void UpdateList()
	{
		Clear();
		var players = Global.Lobby.GetPlayersList();
		foreach (var player in players)
		{
			var item = player.Nickname + " " + player.PlayerColor;
			AddItem(item);
		}
	}
}
