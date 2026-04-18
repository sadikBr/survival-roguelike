using Game.Entities;

using Godot;

namespace Game.Camera;

public partial class GameCamera : Camera2D
{
    private Vector2 _targetPosition = Vector2.Zero;

    public override void _Ready()
    {
        MakeCurrent();
    }

    public override void _Process(double delta)
    {
        AquireTarget();
        // This is not necessary because we have the position smoothing in the Camera2D node??
        // GlobalPosition = GlobalPosition.Lerp(_targetPosition, 1.0f - Mathf.Exp((float)delta * -10));
        GlobalPosition = _targetPosition;
    }

    private void AquireTarget()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        _targetPosition = player.GlobalPosition;
    }
}
