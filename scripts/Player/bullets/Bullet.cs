using Godot;
using System;
using System.Collections.Generic;


namespace mazetank.scripts.player.bullets;
public partial class Bullet : CharacterBody2D, IBullet
{
	private float _speed = 300f;
	private float _liveTime = 2.0f;
	private Vector2 _velocity;
	private string PlayerNickname { set; get; }

	private AnimationPlayer _animation;
	
	public override void _Ready()
	{
		GetNode<Timer>("TimerToLive").Connect("timeout",  new Callable(this, nameof(_OnTimerToLiveTimeout)));
		GetNode<Timer>("TimerCollide").Connect("timeout", new Callable(this, nameof(_OnTimerCollideTimeout)));
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
		_animation.Play("BulletAnimation");
		
		var timerToLive = GetNode<Timer>("TimerToLive");
		timerToLive.WaitTime = _liveTime;
		Velocity = new Vector2(0, -_speed).Rotated(Rotation);
		timerToLive.Start();
	}
	
	
	
	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			Velocity = Velocity.Bounce(collision.GetNormal());
			if (Velocity.Length() < 1.0f)
			{
				Velocity = Velocity.Normalized() * 10.0f;
			}
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
