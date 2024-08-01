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
	
	private Dictionary<long, Color> _colors = new Dictionary<long, Color>()
	{
		{ 0, new Color(1f, 1f, 1) }, // White
		{ 1, new Color(0, 1, 0) }, // Green
		{ 2, new Color(0.969f, 0, 0) }, // Red
		{ 3, new Color(0.584f, 0, 1) }, // Purple
		{ 4, new Color(0.172f, 0.404f, 1) } // Blue
	};

	public Player(long id)
	{
		Id = id;
		Nickname = "Player";
		PlayerColor = new Color(1f, 1f, 1);
	}
	
	public Player(long id, string nickname)
	{
		Id = id;
		Nickname = nickname;
		PlayerColor = new Color(1f, 1f, 1);
	}
	
	public Player(long id, string nickname, Color color)
	{
		Id = id;
		Nickname = nickname;
		PlayerColor = color;
	}
	
	public Player(Dictionary data)
	{
		Id = (long)data["Id"];
		Nickname = (string)data["Nickname"];
		PlayerColor = (Color)data["Color"];
	}
	
	public override void _Process(double delta)
	{
		GD.Print(Score);
	}

	public void SetScore(int score)
	{
		PlayerWasChanged?.Invoke();
		Score += score;
	}
	
	public void SetNickname(string nickname)
	{
		PlayerWasChanged?.Invoke();
		Nickname = nickname;
	}

	public void SetColor(long id)
	{
		PlayerWasChanged?.Invoke();
		PlayerColor = _colors[id];
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
}


