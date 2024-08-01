using Godot;
using mazetank.scripts.player;

namespace mazetank.scripts.global;

public partial class MultiplayerManager : Node
{
	public bool PeerStatus = false;
	
	private bool _debug = true;
	private int _port = 8080;
	private string _ip = "127.0.0.1";

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

	private void MultiplayerOnPeerDisconnected(long id)
	{
		Global.Lobby.DeletePlayer(id);
		if (Multiplayer.IsServer())
		{
			GD.Print("Server was disconnected! Id: " + id);
		}
		else
		{
			GD.Print("Player was disconnected! Id: " + id);
		}
	}

	private void MultiplayerOnPeerConnected(long id)
	{
		if (Multiplayer.IsServer())
		{
			var players = Global.Lobby.GetPlayersList();
			foreach (var player in players)
			{
				RpcId(id, nameof(AddPlayer), player.Id, player.Nickname, player.PlayerColor);
			}
			Rpc(nameof(AddPlayer), id, "Player", new Color(1, 1, 1));
		}
	}

	public void StartHost(int port)
	{
		ENetMultiplayerPeer peer = new ENetMultiplayerPeer();

		peer.CreateServer(_debug ? _port : port);

		if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			OS.Alert("Failed to start multiplayer server.");
			return;
		}

		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Host started, Port = " + peer.Host.GetLocalPort());
	}
	
	public void StartClient(string ip, int port)
	{
		if (ip == "")
		{
			OS.Alert("Need a remote to connect to.");
			return;
		}
		
		ENetMultiplayerPeer clientPeer = new ENetMultiplayerPeer();
		
		if (_debug) clientPeer.CreateClient(_ip, _port);
		else clientPeer.CreateClient(ip, port);
		
		if (clientPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			OS.Alert("Failed to start multiplayer client.");
			return;
		}
		Multiplayer.MultiplayerPeer = clientPeer;
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void AddPlayer(long id, string nickname, Color color)
	{
		Global.Lobby.AddPlayer(new Player(id, nickname, color));
	}
}
