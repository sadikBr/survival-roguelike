using Game.Components;
using Game.GameObjects.Entities;

using Godot;

namespace Game.Abilities;

public partial class AxeAbility : Node2D
{

    private const int MaxRadius = 100;

    private Vector2 _baseRotation = Vector2.Right;

    private HitboxComponent _hitboxComponent;

    public override void _Ready()
    {
        _baseRotation = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
        _hitboxComponent = GetNode<HitboxComponent>("HitboxComponent");

        var tween = CreateTween();
        tween.TweenMethod(Callable.From((float rotations) => SpiralAxe(rotations)), 0.0f, 2.0f, 3.0f);
        tween.TweenCallback(Callable.From(QueueFree));
    }

    public void SetDamage(int damage)
    {
        _hitboxComponent.SetDamage(damage);
    }

    private void SpiralAxe(float rotations)
    {
        var percentage = rotations / 2.0f;
        var currentRadius = percentage * MaxRadius;
        var currentDirection = _baseRotation.Rotated(rotations * Mathf.Tau);

        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        GlobalPosition = player.GlobalPosition + currentDirection * currentRadius;
    }

}
