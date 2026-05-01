using Game.Autoloads;
using Game.GameObjects.Entities;
using Game.Managers;
using Game.Resources.Upgrades;
using Game.Upgrades;

using Godot;
using Godot.Collections;

namespace Game.Abilities.Controllers;

public partial class AnvilAbilityController : Node
{

    [Export] private PackedScene _anvilAbilityScene;

    private Timer _timer;

    private float _minRange = 20.0f;
    private float _maxRange = 50.0f;
    private float _baseWaitTime;
    private float _baseDamage = 25.0f;
    private int _numberOfAnvilsToSpawn = 1;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _baseWaitTime = (float)_timer.WaitTime;
        _timer.Timeout += OnTimerTimeout;

        GameEvents.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
    }

    private void OnTimerTimeout()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var distanceFromPlayer = (float)GD.RandRange(_minRange, _maxRange);
        var direction = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau));
        var additionalRotation = Mathf.Tau / _numberOfAnvilsToSpawn;

        for (int i = 0; i < _numberOfAnvilsToSpawn; i++)
        {
            var adjustedDirection = direction.Rotated(additionalRotation * i);
            var spawnPosition = player.GlobalPosition + adjustedDirection * distanceFromPlayer;
            var queryParams = new PhysicsRayQueryParameters2D()
            {
                From = player.GlobalPosition,
                To = spawnPosition,
                CollisionMask = 1
            };
            var result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(queryParams);

            if (result.Count != 0)
            {
                spawnPosition = result.TryGetValue("position", out var position) ? (Vector2)position : spawnPosition;
            }

            var anvilInstance = _anvilAbilityScene.Instantiate<AnvilAbility>();
            var foregroundLayer = GetTree().GetFirstNodeInGroup("ForegroundLayer");
            foregroundLayer.AddChild(anvilInstance);
            anvilInstance.SetDamage(_baseDamage);
            anvilInstance.GlobalPosition = spawnPosition;
        }
    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeManager.UpgradeState> currentUpgrades)
    {
        if (upgrade.Id == UpgradeStringNames.AnvilCountUpgrade)
        {
            _numberOfAnvilsToSpawn = 1 + currentUpgrades[upgrade.Id].Quantity;
        }
    }
}
