using Game.Components;

using Godot;

namespace Game.GameObjects.Entities;

public partial class BasicEnemy : CharacterBody2D
{
    private const float MaxSpeed = 40.0f;

    private HealthComponent _healthComponent;

    public override void _Ready()
    {
        _healthComponent = GetNode<HealthComponent>("HealthComponent");
    }

    public override void _Process(double delta)
    {
        var direction = GetDirectionToPlayer();

        Velocity = direction * MaxSpeed;
        MoveAndSlide();
    }

    private Vector2 GetDirectionToPlayer()
    {
        return GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player
            ? Vector2.Zero
            : GlobalPosition.DirectionTo(player.GlobalPosition);
    }

}
