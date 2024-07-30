using Godot;
using System;
using System.Collections.Generic;

namespace mazetank.scripts.player.bullets;
public partial class RayTraceBullet : Node2D
{
[Export] public float Speed { get; set; } = 120.0f * 60;
	[Export] public float SpeedFall { get; set; } = 10.0f;
	[Export] public float FadeTime { get; set; } = 0.5f;
	[Export] public float LifeTime { get; set; } = 0.3f;
	[Export] public int Bounces { get; set; } = 6;
	
	 private Vector2 _pos;
    private Vector2 _dir;
    private float _rot;

    private List<(Vector2, float)> _traceLine = new List<(Vector2, float)>();
    private float _time;
    private Color _col1 = Colors.White;
    private Color _col2 = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    
    public override void _Draw()
    {
        if (_traceLine.Count > 0)
        {
            for (int i = _traceLine.Count - 1; i > 0; i--)
            {
                float t = (_time - _traceLine[i].Item2) / FadeTime;
                if (t <= 1.0f)
                {
                    Color c = _col2.Lerp(_col1, 1.0f - t);
                    Vector2 p1 = _traceLine[i].Item1;
                    Vector2 p2 = _traceLine[i - 1].Item1;
                    float thic = Mathf.Clamp(4 * (1.0f - t * 5.0f), 1.0f, 10.0f);
                    DrawLine(p1, p2, c, thic);
                }
            }
        }
    }

    public void _PhysicsProcess(float delta)
    {
        if (Bounces > 0 && _time < LifeTime && Speed > 0.001f)
        {
            BulletProcess(delta);
            Speed = Mathf.Lerp(Speed, 0.0f, SpeedFall * delta);
        }
        _time += delta;
        if (_time >= LifeTime + FadeTime)
        {
            QueueFree();
        }
    }

    public void _Process(float delta)
    {
    }

    private void BulletProcess(float delta)
    {
        // var spaceState = GetWorld2d().DirectSpaceState;
        // float remainLength = Speed * delta;
        // Vector2 end;
        // _traceLine.Add((_pos, _time));
        // Dictionary<string, object> data;
        //
        // while (remainLength > 0.001f && Bounces > 0)
        // {
        //     end = _pos + _dir * remainLength;
        //     data = spaceState.IntersectRay(_pos, end);
        //     if (data.Count > 0)
        //     {
        //         var collider = data["collider"] as CharacterBody2D;
        //         if (collider != null)
        //         {
        //             collider.Call("get_hit", new Dictionary<string, Vector2> { { "dir", _dir } });
        //             end = (Vector2)data["position"] - ((Vector2)data["position"] - _pos).Normalized() * 0.01f;
        //             Bounces = 0;
        //         }
        //         else
        //         {
        //             end = (Vector2)data["position"] - ((Vector2)data["position"] - _pos).Normalized() * 0.01f;
        //             _dir = _dir.Bounce((Vector2)data["normal"]).Normalized();
        //             Bounces -= 1;
        //         }
        //     }
        //     remainLength -= (end - _pos).Length();
        //     _pos = end;
        //     _traceLine.Add((_pos, _time));
        // }
    }
}
