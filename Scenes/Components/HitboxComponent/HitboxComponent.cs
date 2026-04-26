using Godot;

namespace Game.Components;

public partial class HitboxComponent : Area2D
{

    public float Damage { get; private set; }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

}
