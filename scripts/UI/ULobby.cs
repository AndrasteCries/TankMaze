using Godot;

public partial class ULobby : Node2D
{
	public delegate void AdminStartGame();
	public static event AdminStartGame GameStart;

	[Export] private Control _lobbyPanel;
	[Export] private Control _editPlayerPanel;
	[Export] private PlayerItemList _playerList;
	
	public override void _EnterTree()
	{
		Multiplayer.PeerConnected += MultiplayerOnPeerConnected;
		Multiplayer.PeerDisconnected += MultiplayerOnPeerDisconnected;
	}
	
	public override void _ExitTree()
	{
		Multiplayer.PeerConnected -= MultiplayerOnPeerConnected;
		Multiplayer.PeerDisconnected -= MultiplayerOnPeerDisconnected;
	}
	
	private void MultiplayerOnPeerConnected(long id)
	{
		Rpc(nameof(UpdatePlayerList));
	}
	
	private void MultiplayerOnPeerDisconnected(long id)
	{
		Rpc(nameof(UpdatePlayerList));
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void UpdatePlayerList() 
	{
		_playerList.UpdateList();
	}
	
	public void EditPlayer()
	{
		
	}
	
	public void PressedGameStart()
	{
		GameStart?.Invoke();
	}
}
