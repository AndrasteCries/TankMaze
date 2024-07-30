using Godot;

namespace mazetank.scripts.player.bullets;

public interface IBullet
{
    void SetNickname(string nickname);
    string GetNickname();
}