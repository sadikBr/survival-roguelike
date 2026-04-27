using Game.GameObjects.Entities;

using Godot;

namespace Game.Components;

public partial class VelocityComponent : Node
{

    [Export] public float MaxSpeed { get; private set; } = 40;
    [Export] private float _acceleration = 5.0f;

    private Vector2 _velocity = Vector2.Zero;

    public void AccelerateToPlayer()
    {
        if (Owner is not Node2D owner) return;
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var direction = owner.GlobalPosition.DirectionTo(player.GlobalPosition);
        AccelerateInDirection(direction);
    }

    public void AccelerateInDirection(Vector2 direction)
    {
        var desiredVelocity = direction * MaxSpeed;
        _velocity = _velocity.Lerp(desiredVelocity, 1 - Mathf.Exp(-_acceleration * (float)GetProcessDeltaTime()));
    }

    public void Move(CharacterBody2D character)
    {
        character.Velocity = _velocity;
        character.MoveAndSlide();
        _velocity = character.Velocity;
    }

    public void UpdateMaxSpeed(float maxSpeed)
    {
        MaxSpeed = maxSpeed;
    }

}
