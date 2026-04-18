using Game.Entities;

using Godot;

namespace Game.Managers;

public partial class EnemyManager : Node
{
    [Export]
    private PackedScene _basicEnemy;

    private const float SpawnRadius = 375.0f;
    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
        var spawnPosition = player.GlobalPosition + randomDirection * SpawnRadius;

        var enemy = _basicEnemy.Instantiate<Node2D>();
        GetParent().AddChild(enemy);
        enemy.GlobalPosition = spawnPosition;
    }
}
