using Godot;
using System;


namespace mazetank.scripts.player.bullets;
public partial class Bullet : CharacterBody2D, IBullet
{
	private float _speed = 200;
	private float _liveTime = 2.0f;
	private Vector2 _velocity;
	private string PlayerNickname { set; get; }

	private AnimationPlayer _animation;

	public override void _Ready()
	{
		GetNode<Timer>("TimerToLive").Connect("timeout",  new Callable(this, nameof(_OnTimerToLiveTimeout)));
		GetNode<Timer>("TimerCollide").Connect("timeout", new Callable(this, nameof(_OnTimerCollideTimeout)));
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void Start(Vector2 position, float direction)
	{
		_animation.Play("BulletAnimation");
		Rotation = direction;
		Position = position;
		_velocity = new Vector2(0, -_speed).Rotated(Rotation);
		var timerToLive = GetNode<Timer>("TimerToLive");
		timerToLive.WaitTime = _liveTime;
		timerToLive.Start();
	}
	
	public override void _Process(double delta)
	{
		var collision = MoveAndCollide(_velocity * (float)delta);
		if (collision != null)
		{
			_velocity = _velocity.Bounce(collision.GetNormal());
		}
	}
	
	private void _OnTimerToLiveTimeout()
	{
		QueueFree();
	}

	private void _OnTimerCollideTimeout()
	{
		GetNode<CollisionShape2D>("BulletArea/CollisionShape2D").Disabled = false;
	}
	
	public void SetNickname(string nickname)
	{
		PlayerNickname = nickname;
	}

	public string GetNickname() => PlayerNickname;
}
