using System.Linq;

using Game.Autoloads;
using Game.GameObjects.Entities;
using Game.Managers;
using Game.Resources.Upgrades;

using Godot;
using Godot.Collections;

namespace Game.Abilities.Controllers;

public partial class SwordAbilityController : Node
{
    [Export]
    private PackedScene _swordAbility;

    private const float MaxRange = 150.0f;
    private int _damage = 5;
    private float _baseWaitTime;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        _baseWaitTime = (float)_timer.WaitTime;

        GameEvents.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
    }

    private void OnTimerTimeout()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;
        var enemies = GetTree().GetNodesInGroup("Enemy").ToList();
        var filteredEnemies = enemies.Where(e =>
        {
            var enemy = e as Node2D;
            return enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) < Mathf.Pow(MaxRange, 2.0f);
        }).ToList();

        if (filteredEnemies.Count == 0) return;

        filteredEnemies.Sort((a, b) =>
        {
            var distanceA = ((Node2D)a).GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
            var distanceB = ((Node2D)b).GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
            return distanceA.CompareTo(distanceB);
        });
        var target = filteredEnemies[0] as Node2D;

        var instance = _swordAbility.Instantiate<SwordAbility>();
        var foregroundLayer = GetTree().GetFirstNodeInGroup("ForegroundLayer");
        foregroundLayer.AddChild(instance);
        instance.SetDamage(_damage);

        instance.GlobalPosition = target.GlobalPosition;
        instance.GlobalPosition += Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau)) * 4.0f;

        var enemyDirection = target.GlobalPosition.DirectionTo(instance.GlobalPosition);
        instance.Rotation = enemyDirection.Angle();
    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeManager.UpgradeState> currentUpgrades)
    {
        if (upgrade.Id != "sword_rate") return;

        var percentReduction = currentUpgrades[upgrade.Id].Quantity * 0.1f;
        _timer.WaitTime = Mathf.Max(_baseWaitTime * (1.0f - percentReduction), 0.1f);
        _timer.Start();
    }

}
