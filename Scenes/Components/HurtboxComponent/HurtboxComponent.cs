using Game.UI;

using Godot;

namespace Game.Components;

public partial class HurtboxComponent : Area2D
{

    [Signal] public delegate void HitEventHandler();

    [Export] private PackedScene _floatingTextScene;
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

        var floatingText = _floatingTextScene.Instantiate<FloatingText>();
        GetTree().GetFirstNodeInGroup("ForegroundLayer").AddChild(floatingText);
        floatingText.GlobalPosition = GlobalPosition + (Vector2.Up * 16);
        floatingText.Start(hitbox.Damage.ToString(Mathf.Round(hitbox.Damage) == hitbox.Damage ? "F0" : "F1"));

        EmitSignal(SignalName.Hit);
    }
}
