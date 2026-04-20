using Game.GameObjects.Entities;

using Godot;

namespace Game.Managers;

public partial class EnemyManager : Node
{

    [Export] private PackedScene _basicEnemy;
    [Export] private ArenaTimeManager _arenaTimeManager;

    private const float SpawnRadius = 375.0f;
    private float _baseSpawnInterval;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _baseSpawnInterval = (float)_timer.WaitTime;
        _timer.Timeout += OnTimerTimeout;

        _arenaTimeManager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;
    }

    private void OnTimerTimeout()
    {
        _timer.Start();
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
        var spawnPosition = player.GlobalPosition + randomDirection * SpawnRadius;

        var enemy = _basicEnemy.Instantiate<Node2D>();

        var entitiesLayer = GetTree().GetFirstNodeInGroup("EntitiesLayer");
        entitiesLayer.AddChild(enemy);
        enemy.GlobalPosition = spawnPosition;
    }

    private void OnArenaDifficultyIncreased(int arenaDifficulty)
    {
        var timeOff = (0.1f / 12) * arenaDifficulty;
        timeOff = Mathf.Min(timeOff, 0.7f);
        _timer.WaitTime = _baseSpawnInterval - timeOff;
    }

}
