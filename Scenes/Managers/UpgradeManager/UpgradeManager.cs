using System.Collections.Generic;

using Game.Resources.Upgrades;

using Godot;

namespace Game.Managers;

public partial class UpgradeManager : Node
{

    [Export] private AbilityUpgrade[] _upgradePool = [];
    [Export] private ExperienceManager _experienceManager;

    private readonly Dictionary<string, UpgradeState> _currentUpgrades = [];
    private struct UpgradeState
    {
        public AbilityUpgrade Resource { get; set; }
        public int Quantity { get; set; }

        public void UpdateQuantity(int amount)
        {
            Quantity += amount;
        }
    }

    public override void _Ready()
    {
        _experienceManager.LevelUp += OnLevelUp;
    }

    private void OnLevelUp(int newLevel)
    {
        var chosenUpgrade = _upgradePool[GD.Randi() % _upgradePool.Length];
        if (chosenUpgrade == null) return;

        if (!_currentUpgrades.TryGetValue(chosenUpgrade.Id, out var data))
        {
            _currentUpgrades.Add(chosenUpgrade.Id, new UpgradeState()
            {
                Resource = chosenUpgrade,
                Quantity = 1
            });
        }
        else
        {
            data.UpdateQuantity(1);
            _currentUpgrades[chosenUpgrade.Id] = data;
        }
    }

}
