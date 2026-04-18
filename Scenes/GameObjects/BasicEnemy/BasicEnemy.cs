using Godot;

namespace Game.GameObjects.Entities;

public partial class BasicEnemy : CharacterBody2D
{
    private const float MaxSpeed = 40.0f;

    private Area2D _hitbox;

    public override void _Ready()
    {
        _hitbox = GetNode<Area2D>("Hitbox");
        _hitbox.AreaEntered += OnAreaEntered;
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

    private void OnAreaEntered(Area2D other)
    {
        QueueFree();
    }
}
