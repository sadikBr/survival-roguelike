using Godot;

namespace Game.Entities;

public partial class BasicEnemy : CharacterBody2D
{
    private const float MaxSpeed = 75.0f;

    public override void _Ready()
    {
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
