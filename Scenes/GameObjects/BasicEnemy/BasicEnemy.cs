using Game.Components;

using Godot;

namespace Game.GameObjects.Entities;

public partial class BasicEnemy : CharacterBody2D
{
    private const float MaxSpeed = 40.0f;

    private HealthComponent _healthComponent;
    private Node2D _visuals;

    public override void _Ready()
    {
        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        _visuals = GetNode<Node2D>("Visuals");
    }

    public override void _Process(double delta)
    {
        var direction = GetDirectionToPlayer();

        Velocity = direction * MaxSpeed;
        MoveAndSlide();

        var moveSign = Mathf.Sign(direction.X);
        _visuals.Scale = moveSign == 0 ? Vector2.One : new Vector2(-moveSign, 1);
    }

    private Vector2 GetDirectionToPlayer()
    {
        return GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player
            ? Vector2.Zero
            : GlobalPosition.DirectionTo(player.GlobalPosition);
    }

}
