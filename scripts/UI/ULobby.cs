using global::mazetank.scripts.global;
using Godot;
using mazetank.scripts.player;

public partial class ULobby : Node2D
{
	public delegate void AdminStartGame();
	public static event AdminStartGame GameStart;

	[Export] private Control _lobbyPanel;
	[Export] private EditPlayerPanel _editPlayerPanel;
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
	
	public void PressedGameStart()
	{
		GameStart?.Invoke();
	}
	
	public void EditPlayer()
	{
		_lobbyPanel.Hide();
		_editPlayerPanel.Show();
	}

	public void SavePlayerInfo()
	{
		Rpc(nameof(UpdatePlayer), Multiplayer.MultiplayerPeer.GetUniqueId(), _editPlayerPanel.GetNewNickname(), _editPlayerPanel.GetColor());
		_editPlayerPanel.Hide();
		_lobbyPanel.Show();
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void UpdatePlayer(long id, string nickname, Color color) 
	{
		Player player = Global.Lobby.GetPlayerById(id);
		player.Nickname = nickname;
		player.PlayerColor = color;
		_playerList.UpdateList();
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void UpdatePlayerList() 
	{
		_playerList.UpdateList();
	}
}
