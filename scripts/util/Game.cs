using Godot;
using mazetank.scripts.global;
using mazetank.scripts.maze;
using mazetank.scripts.player;

namespace mazetank.scripts.util;

public partial class Game : Node2D
	{
		[Export] public PackedScene PlayerScene { get; set; }
		[Export] public PackedScene BuffScene { get; set; }
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
			// BuffSpawnTimer.Timeout += _SpawnBuff;
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
				GD.Print("Player");
				Tank tank = (Tank)PlayerScene.Instantiate();
				Vector2I randomXY = _maze.RandomCell();
				AddChild(tank);
				tank.GlobalPosition = _maze._maze[randomXY.X][randomXY.Y].GetSpawnpointPosition();
				tank.Rotation = _rng.RandfRange(0, 2 * Mathf.Pi);
				
				// SpawnPlayer(player);
			}
		}

		// private void SpawnPlayer(int peerId)
		// {
		//     var player = (Node2D)PlayerScene.Instantiate();
		//     player.Name = peerId.ToString();
		//     var randomXY = RandomCell();
		//     AddChild(player);
		//     player.GlobalPosition = _maze[randomXY.x][randomXY.y].GetSpawnPointPosition();
		//     player.Rotation = _rng.RandfRange(0, 2 * Mathf.Pi);
		//     EmitSignal(nameof(ScoreRefresh));
		// }

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

	//     private void _SpawnBuff()
	//     {
	//         var x = _rng.RandiRange(0, _maze.Count - 2);
	//         var y = _rng.RandiRange(1, _maze[0].Count - 1);
	//         SpawnBuffRpc(x, y);
	//     }
	//
	//     [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	//     private void SpawnBuffRpc(int x, int y)
	//     {
	//         var buff = (Node2D)BuffScene.Instantiate();
	//         buff.SetType(_rng.RandiRange(0, 2));
	//         AddChild(buff);
	//         buff.Name = "Buff" + buff.Type;
	//         buff.GlobalPosition = _maze[x][y].GetSpawnPointPosition();
	//         buff.Rotation = _rng.RandfRange(0, 2 * Mathf.Pi);
	//     }
	//
	//     private void _ServerDisconnected()
	//     {
	//         GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	//     }
	//
	// }
}
