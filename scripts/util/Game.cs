using Godot;
using mazetank.scripts.global;
using mazetank.scripts.maze;
using mazetank.scripts.player;
using mazetank.scripts.player.buff;

namespace mazetank.scripts.util;

public partial class Game : Node2D
	{
		[Export] public Timer BuffSpawnTimer { get; set; }

		private Maze _maze;
		private RandomNumberGenerator _rng = new RandomNumberGenerator();

		//[Signal]
		//public delegate void ScoreRefresh();

		public override void _Ready()
		{
			_maze = GetNode<Maze>("Maze");
			_rng.Seed = (ulong)Global.GameSettings.GetSeed();
			SpawnMaze();
			SpawnPlayers();
			
			// EventBus.player_dead += _RespawnPlayer;
			// multiplayer.server_disconnected += _ServerDisconnected;
			BuffSpawnTimer.Timeout += _SpawnBuff;
		}

		private void SpawnMaze()
		{
			_maze.FinishMaze();
			for (int i = 0; i < _maze._maze.Count; i++)
			{
				for (int j = 0; j < _maze._maze[i].Count; j++)
				{
					_maze.AddChild(_maze._maze[i][j]);
				}
			}
		}

		private void SpawnPlayers()
		{
			foreach (Player player in Global.Lobby.Players)
			{
				var cell = _maze.GetRandomCell();
				cell.SpawnPlayer(_rng);
			}
		}
	
		private void _SpawnBuff()
		{
			var cell = _maze.GetRandomCell();
			int buffType = _rng.RandiRange(0, 1);
			cell.SpawnBuff(buffType, _rng);
		}
		
		// private void _RespawnPlayer(int peerId, string killer)
		// {
		//     RespawnPlayer(peerId, killer);
		// }

		// [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
		// private void RespawnPlayerRpc(int peerId, string killer)
		// {
		//     var player = GetNode<Node2D>(peerId.ToString());
		//     var randomXY = RandomCell();
		//     if (peerId == int.Parse(killer.Split(" ")[0]))
		//     {
		//         Lobby.players[int.Parse(killer.Split(" ")[0])]["Score"] -= 1;
		//     }
		//     else
		//     {
		//         Lobby.players[int.Parse(killer.Split(" ")[0])]["Score"] += 1;
		//     }
		//     player.GlobalPosition = _maze[randomXY.x][randomXY.y].GetSpawnPointPosition();
		//     player.Show();
		//     EmitSignal(nameof(ScoreRefresh));
		// }
	// 	private void _ServerDisconnected()
	// {
	//     GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	// }
}
