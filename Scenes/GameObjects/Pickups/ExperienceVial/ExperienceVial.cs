using Godot;

namespace Game.GameObjects.Pickups;

public partial class ExperienceVial : Node2D
{
    private Area2D _area2D;

    public override void _Ready()
    {
        _area2D = GetNode<Area2D>("Area2D");
        _area2D.AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        QueueFree();
    }
}
