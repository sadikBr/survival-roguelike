using Game.Autoloads;
using Game.GameObjects.Entities;
using Game.Managers;
using Game.Resources.Upgrades;
using Game.Upgrades;

using Godot;
using Godot.Collections;

namespace Game.Abilities.Controllers;

public partial class AxeAbilityController : Node
{

    [Export] private PackedScene _axeAbilityScene;

    private float _baseDamage = 10.0f;
    private float _damageMultiplier = 1.0f;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;

        GameEvents.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
    }

    private void OnTimerTimeout()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var axeAbilityInstance = _axeAbilityScene.Instantiate<AxeAbility>();
        var foregroundLayer = GetTree().GetFirstNodeInGroup("ForegroundLayer");
        foregroundLayer.AddChild(axeAbilityInstance);

        axeAbilityInstance.SetDamage(_baseDamage * _damageMultiplier);
        axeAbilityInstance.GlobalPosition = player.GlobalPosition;
    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeManager.UpgradeState> currentUpgrades)
    {
        if (upgrade.Id == UpgradeStringNames.AxeDamageUpgrade)
        {
            _damageMultiplier = 1.0f + 0.1f * currentUpgrades[upgrade.Id].Quantity;
        }
    }
}
