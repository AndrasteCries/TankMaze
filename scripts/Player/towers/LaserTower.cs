using System.Collections.Generic;
using Godot;
using mazetank.scripts.player.bullets;

namespace mazetank.scripts.player.towers;

public partial class LaserTower : Node2D, ITower
{
	[Export] public PackedScene Bullet { get; set; }
	public int BulletsCount { get; set; } = 4;
	public Node World { get; set; }
	public Marker2D Muzzle1 { get; set; }
	private string _playerNickname;

	private List<Vector2> rayPoints = new List<Vector2>();
	
	public override void _Ready()
	{
		_playerNickname = GetParent().Name;
		World = GetNode("/root/Game");
		Muzzle1 = GetNode<Marker2D>("Sprite2D/Muzzle");
	}

	public async void Shoot()
	{
		if (BulletsCount > 0)
		{
			var b = (Bullet)Bullet.Instantiate();
			b.SetNickname(_playerNickname);
			b.Name = _playerNickname + " bullet";
			var timerToDeath = new Timer
			{
				WaitTime = 2
			};
			AddChild(timerToDeath);

			b.Rotation = GetParent<CharacterBody2D>().Rotation;
			b.Position = Muzzle1.GlobalPosition;
			
			World.AddChild(b);

			timerToDeath.Start();

			BulletsCount -= 1;
			await ToSignal(timerToDeath, "timeout");
			timerToDeath.QueueFree();
			BulletsCount += 1;
		}
	}

	public override void _Process(double delta)
	{
		UpdateRay();
		
	}

	private void UpdateRay()
	{
		Vector2 origin = new Vector2(GlobalPosition.X + 30f, GlobalPosition.Y + 30f) ; // Используйте глобальную позицию пули
		Vector2 direction = -Transform.Y.Normalized(); // Используйте направление Transform.Y для определения направления
		rayPoints.Clear();
		rayPoints.Add(origin);
		Vector2 currentOrigin = origin;
		Vector2 currentDirection = direction;
		var spaceState = GetWorld2D().DirectSpaceState;
		for (int i = 0; i < 5; i++)
		{
			Vector2 end = currentOrigin + currentDirection * 1000f; // Увеличьте длину луча до 1000
			var query = PhysicsRayQueryParameters2D.Create(currentOrigin, end);
			var result = spaceState.IntersectRay(query);
		
			GD.Print("Origin: ", currentOrigin, " End: ", end, " Direction: ", currentDirection);

			if (result.Count > 0)
			{
				GD.Print("Hit detected at position: ", result["position"]);
				end = (Vector2)result["position"];
				rayPoints.Add(end);
				Vector2 normal = ((Vector2)result["normal"]).Normalized();
				if (normal.LengthSquared() == 0)
				{
					GD.PrintErr("Invalid normal detected");
					break;
				}
				currentOrigin = end + normal * 0.1f;
				currentDirection = currentDirection.Bounce(normal).Normalized();
			}
			else
			{
				GD.Print("No hit detected");
				rayPoints.Add(end);
				break;
			}
		}
		QueueRedraw();
	}

	public override void _Draw()
	{
		for (int i = 0; i < rayPoints.Count - 1; i++) 
		{ 
			DrawLine(rayPoints[i], rayPoints[i+1], Colors.YellowGreen, 2);
		}
		DrawLine(GetLocalMousePosition(), rayPoints[1], Colors.YellowGreen, 2);
		DrawCircle(GetLocalMousePosition(), 2f, Colors.Aqua);
		DrawLine(new Vector2(0, 0), new Vector2(0, GlobalPosition.Y * -10f), Colors.Red, 2);
	}
}
