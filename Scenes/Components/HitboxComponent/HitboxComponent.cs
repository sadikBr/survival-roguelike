using Godot;

namespace Game.Components;

public partial class HitboxComponent : Area2D
{

    public int Damage { get; private set; }

    public void SetDamage(int damage)
    {
        Damage = damage;
    }

}
