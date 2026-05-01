using Game.Components;

using Godot;

namespace Game.Abilities;

public partial class AnvilAbility : Node2D
{

    private HitboxComponent _hitboxComponent;

    public override void _Ready()
    {
        _hitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
    }

    public void SetDamage(float damage)
    {
        _hitboxComponent.SetDamage(damage);
    }
}
