using Godot;
using System;

public partial class EditPlayerPanel : Control
{
	[Export] private TextureRect _preview1;
	[Export] private TextureRect _preview2;
	[Export] private LineEdit _nickname;
	[Export] private ColorPicker _color;

	private void ColorPickerColorChanged(Color color)
	{
		_preview1.Modulate = color;
		_preview2.Modulate = color;
	}

	public Color GetColor() => _color.Color;
		
	public string GetNewNickname() => _nickname.Text;
	
}



