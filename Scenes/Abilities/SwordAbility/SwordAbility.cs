using Game.Components;

using Godot;

namespace Game.Abilities;

public partial class SwordAbility : Node2D
{

    private HitboxComponent _hitboxComponent;

    public override void _Ready()
    {
        _hitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
    }

    public void SetDamage(int damage)
    {
        _hitboxComponent.SetDamage(damage);
    }

}
