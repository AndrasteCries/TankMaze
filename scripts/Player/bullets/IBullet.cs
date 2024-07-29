using Godot;

namespace mazetank.scripts.player.bullets;

public interface IBullet
{
    void Start(Vector2 position, float direction);
    void SetNickname(string nickname);
    string GetNickname();
}