using System.Linq;
using Godot;
using mazetank.scripts.global;
using mazetank.scripts.maze;
using mazetank.scripts.player;

namespace mazetank.scripts.util
{
	public partial class Game : Node2D
	{
		[Export] public Timer BuffSpawnTimer { get; set; }
		[Export] private double _respawnTime;
		[Export] private ItemList _playerScore;

		private Maze _maze;
		private RandomNumberGenerator _rng = new RandomNumberGenerator();

		public override void _Ready()
		{
			_rng.Seed = (ulong)Global.GameSettings.GetSeed();
			_respawnTime = Global.GameSettings.GetRespawnTime();
			_maze = GetNode<Maze>("Maze");
			SpawnMaze();
			SpawnPlayers();
			UpdateList();
			//BuffSpawnTimer.Timeout += _SpawnBuff;
			// multiplayer.server_disconnected += _ServerDisconnected;
		}

		private void _playerWasKilled(string killerNickname, string victimNickname)
		{
			Player killer = Global.Lobby.GetPlayerByNickName(killerNickname);
			killer?.ChangeScore(victimNickname);
			UpdateList();
			GetTree().CreateTimer(_respawnTime).Timeout += () => RespawnPlayer(victimNickname);
		}

		private void SpawnMaze()
		{
			_maze.FinishMaze();
			for (int i = 0; i < _maze.GetMaze().Count; i++)
			{
				for (int j = 0; j < _maze.GetMaze()[i].Count; j++)
				{
					_maze.AddChild(_maze.GetMaze()[i][j]);
				}
			}
		}

		private void SpawnPlayers()
		{
			var players = Global.Lobby.GetPlayersList();
			foreach (Player player in players)
			{
				_maze.AddTank(player.Id, player.Nickname, player.PlayerColor);
			}
		}
		
		private void RespawnPlayer(string nickname)
		{
			Tank victim = _maze.FindTankByNickname(nickname);
			_maze.RespawnTank(victim);
			victim.SetState(true);
		}

		private void _SpawnBuff()
		{
			if (_maze.GetBuffCount() < Global.GameSettings.GetBuffsCount())
			{
				_maze.AddBuff();
			}
		}

		private void UpdateList()
		{
			_playerScore.Clear();

			foreach (var player in Global.Lobby.GetPlayersList())
			{
				_playerScore.AddItem(player.Nickname + ": " + player.Score);
			}
		}
		
		public override void _EnterTree()
		{
			Tank.PlayerWasKilled += _playerWasKilled;
		}

		public override void _ExitTree()
		{
			Tank.PlayerWasKilled += _playerWasKilled;
		}
	}
}


