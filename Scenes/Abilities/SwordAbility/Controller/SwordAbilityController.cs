using System.Linq;

using Game.Autoloads;
using Game.GameObjects.Entities;
using Game.Managers;
using Game.Resources.Upgrades;
using Game.Upgrades;

using Godot;
using Godot.Collections;

namespace Game.Abilities.Controllers;

public partial class SwordAbilityController : Node
{
    [Export]
    private PackedScene _swordAbility;

    private const float MaxRange = 150.0f;
    private float _baseWaitTime;
    private int _numberOfTargets = 1;
    private float _baseDamage = 5.0f;
    private float _damageMultiplier = 1.0f;

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
        var targets = filteredEnemies.Take(_numberOfTargets).Cast<Node2D>().ToArray();

        foreach (var target in targets)
        {
            var instance = _swordAbility.Instantiate<SwordAbility>();
            var foregroundLayer = GetTree().GetFirstNodeInGroup("ForegroundLayer");
            foregroundLayer.AddChild(instance);
            instance.SetDamage(_baseDamage * _damageMultiplier);

            instance.GlobalPosition = target.GlobalPosition;
            instance.GlobalPosition += Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau)) * 4.0f;

            var enemyDirection = instance.GlobalPosition.DirectionTo(target.GlobalPosition);
            instance.Rotation = enemyDirection.Angle();
        }
    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeManager.UpgradeState> currentUpgrades)
    {
        if (upgrade.Id == UpgradeStringNames.SwordRateUpgrade)
        {
            var percentReduction = currentUpgrades[upgrade.Id].Quantity * 0.1f;
            _timer.WaitTime = Mathf.Max(_baseWaitTime * (1.0f - percentReduction), 0.1f);
            _timer.Start();
        }
        else if (upgrade.Id == UpgradeStringNames.SwordDamageUpgrade)
        {
            _damageMultiplier = 1.0f + 0.15f * currentUpgrades[upgrade.Id].Quantity;
        }
        else if (upgrade.Id == UpgradeStringNames.SwordTargetsUpgrade)
        {
            _numberOfTargets = 1 + currentUpgrades[upgrade.Id].Quantity;
        }
    }

}
