using System;

using Godot;

namespace Game.Components;

public partial class HurtboxComponent : Area2D
{

    [Export] private HealthComponent _healthComponent;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not HitboxComponent hitbox) return;

        if (_healthComponent == null) return;

        _healthComponent.Damage(hitbox.Damage);
    }
}
