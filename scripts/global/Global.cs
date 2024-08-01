using Godot;
using System;

namespace mazetank.scripts.global;

public partial class Global : Node
{
	public static GameSettings GameSettings {get; private set; }
	public static Lobby Lobby {get; private set; }
	public static MultiplayerManager MultiplayerManager {get; private set; }
	// public static UI UserInterface {get; private set; }
		
	public override void _Ready()
	{
		// UserInterface = GetNode<UI>("/root/UI");
		MultiplayerManager = GetNode<MultiplayerManager>("/root/MultiplayerManager");
		GameSettings = GetNode<GameSettings>("/root/GameSettings");
		Lobby = GetNode<Lobby>("/root/Lobby");
	}
}
