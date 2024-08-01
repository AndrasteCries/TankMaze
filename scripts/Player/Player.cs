using System.Text.Json;
using Godot;
using Godot.Collections;

namespace mazetank.scripts.player;

public partial class Player : Node2D
{
	public delegate void PlayerInfoWasChanged();
	public static event PlayerInfoWasChanged PlayerWasChanged;
	
	public long Id { get; set; }
	public string Nickname { get; set; }
	public Color PlayerColor { get; set; }
	public int Score { get; set; }
	
	public Player(long id)
	{
		Id = id;
		Nickname = "Player";
		PlayerColor = new Color(1f, 1f, 1);
	}
	
	public Player(long id, string nickname, Color color)
	{
		Id = id;
		Nickname = nickname;
		PlayerColor = color;
	}
	
	public override void _Process(double delta)
	{
		GD.Print(Score);
	}


	public void SetColor(Color color)
	{
		// PlayerWasChanged?.Invoke();
		PlayerColor = color;
	}
	
	//todo
	public void ChangeScore(string nickname)
	{
		if (Nickname == nickname)
		{
			Score--;
		}
		else
		{
			Score++;
		}
	}
	// public void SetScore(int score)
	// {
	// 	PlayerWasChanged?.Invoke();
	// 	Score += score;
	// }
	// public void SetNickname(string nickname)
	// {
	// 	PlayerWasChanged?.Invoke();
	// 	Nickname = nickname;
	// }
}


