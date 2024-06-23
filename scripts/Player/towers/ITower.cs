using Godot;

namespace mazetank.scripts.player.towers;

public interface ITower
{
    PackedScene Bullet { get; set; }
    int BulletsCount { get; set; }
    Node World { get; set; }
    Marker2D Muzzle1 { get; set; }
    void Shoot();
}