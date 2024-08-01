using Godot;
using System;

public partial class MenuPanel : Control
{
	[Export] public TextEdit IP;
	[Export] public TextEdit Port;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
	public string GetIP()
	{
		return IP.Text;
	} 
	
	public string GetPort() => Port.Text;
}
