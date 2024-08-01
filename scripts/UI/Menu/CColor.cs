using Godot;
using System;
using System.Linq;
using global::mazetank.scripts.global;

namespace mazetank.scripts.UI.Menu;

public partial class CColor : MenuButton
{
	private PopupMenu _popUp;

	public override void _EnterTree()
	{
		_popUp = GetPopup();
		_popUp.IdPressed += PopUpOnIdPressed;
	}

	private void PopUpOnIdPressed(long id)
	{
		Global.Lobby.GetPlayersList().First().SetColor(id);
		Text = _popUp.GetItemText((int)id);
	}
}


