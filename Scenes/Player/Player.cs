using Godot;

namespace Game.Entities;

public partial class Player : CharacterBody2D
{
    private static readonly StringName ActionMoveLeft = new("move_left");
    private static readonly StringName ActionMoveRight = new("move_right");
    private static readonly StringName ActionMoveUp = new("move_up");
    private static readonly StringName ActionMoveDown = new("move_down");

    private const float MaxSpeed = 125.0f;
    private const float Acceleration = 25.0f;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        var direction = Input.GetVector(ActionMoveLeft, ActionMoveRight, ActionMoveUp, ActionMoveDown);
        var targetVelocity = direction * MaxSpeed;

        Velocity = Velocity.Lerp(targetVelocity, 1 - Mathf.Exp((float)delta * -Acceleration));
        MoveAndSlide();
    }
}
