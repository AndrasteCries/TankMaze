using Godot;
using global::mazetank.scripts.global;
using mazetank.scripts.player;
using mazetank.scripts.UI.Menu;

namespace mazetank.scripts;

public partial class MainScene : Node
{
	[Export] private MainMenu _userInterface;
	[Export] private Node _level;
	[Export] private PackedScene _gameScene;
	[Export] private PackedScene _lobbyScene;
	
	private const int Port = 8080;
	
	public override void _Ready()
	{
		if (DisplayServer.GetName() == "headless")
		{
			GD.Print("Automatically starting dedicated server.");
		}
	}
	
	public override void _EnterTree()
	{
		ULobby.GameStart += ULobbyOnGameStart;
		Multiplayer.ConnectedToServer += OnConnectedToServer;
	}

	public override void _ExitTree()
	{
		ULobby.GameStart -= ULobbyOnGameStart;
	}
	
	private void OnHostPressed()
	{
		GD.Print("Try start host");
		Global.MultiplayerManager.StartHost(Port);
		Global.Lobby.AddPlayer(new Player(Multiplayer.MultiplayerPeer.GetUniqueId(), "Admin", new Color(0, 0, 1)));
		_userInterface.Hide();
		ChangeLevel(_lobbyScene.ResourcePath);
	}

	private void OnConnectPressed()
	{ 
		GD.Print("Try connect to host");
		string ip = _userInterface.Menu.GetIP();
		int port = _userInterface.Menu.GetPort().ToInt();
		Global.MultiplayerManager.StartClient(ip, port);
	}

	private void OnConnectedToServer()
	{
		_userInterface.Hide();
		ChangeLevel(_lobbyScene.ResourcePath);
	}
	
	private void ULobbyOnGameStart()
	{
		_userInterface.Hide();
		if (Multiplayer.IsServer())
		{
			Rpc(nameof(ChangeLevel), _gameScene.ResourcePath);
		}
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ChangeLevel(string scenePath)
	{
		
		var scene = ResourceLoader.Load<PackedScene>(scenePath);
		foreach (var child in _level.GetChildren())
		{
			_level.RemoveChild(child);
			child.QueueFree();
		}
		_level.AddChild(scene.Instantiate());
	}
}
