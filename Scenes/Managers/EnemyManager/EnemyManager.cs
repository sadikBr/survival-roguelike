using Game.GameObjects.Entities;
using Game.Utils;

using Godot;

namespace Game.Managers;

public partial class EnemyManager : Node
{

    [Export] private PackedScene _basicEnemyScene;
    [Export] private PackedScene _wizardEnemyScene;
    [Export] private PackedScene _batEnemyScene;
    [Export] private ArenaTimeManager _arenaTimeManager;

    private const float SpawnRadius = 375.0f;
    private float _baseSpawnInterval;

    private WeightedTable<PackedScene> _enemyTable = new();

    private Timer _timer;

    public override void _Ready()
    {
        _enemyTable.AddItem(_basicEnemyScene, 10);

        _timer = GetNode<Timer>("Timer");
        _baseSpawnInterval = (float)_timer.WaitTime;
        _timer.Timeout += OnTimerTimeout;

        _arenaTimeManager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;
    }

    private void OnTimerTimeout()
    {
        _timer.Start();
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var scene = _enemyTable.PickItem();
        var enemy = scene.Instantiate<Node2D>();

        var entitiesLayer = GetTree().GetFirstNodeInGroup("EntitiesLayer");
        entitiesLayer.AddChild(enemy);
        enemy.GlobalPosition = GetSpawnPosition();
    }

    private Vector2 GetSpawnPosition()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return Vector2.Zero;

        var spawnPosition = Vector2.Zero;
        var randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));

        for (int i = 0; i < 4; i++)
        {
            spawnPosition = player.GlobalPosition + randomDirection * SpawnRadius;
            var additionalOffset = randomDirection * 20;

            var queryParams = new PhysicsRayQueryParameters2D()
            {
                From = player.GlobalPosition,
                To = spawnPosition + additionalOffset,
                CollisionMask = 1
            };
            var result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(queryParams);

            if (result.Count == 0) break;
            else
            {
                randomDirection = randomDirection.Rotated(Mathf.DegToRad(90));
            }
        }

        return spawnPosition;
    }

    private void OnArenaDifficultyIncreased(int arenaDifficulty)
    {
        var timeOff = 0.1f * (arenaDifficulty / 12);
        timeOff = Mathf.Min(timeOff, 0.7f);
        _timer.WaitTime = _baseSpawnInterval - timeOff;

        if (arenaDifficulty == 12)
        {
            _enemyTable.AddItem(_wizardEnemyScene, 20);
        }
        else if (arenaDifficulty == 24)
        {
            _enemyTable.AddItem(_batEnemyScene, 30);
        }
    }

}
