using Game.Autoloads;
using Game.Resources.Upgrades;
using Game.UI;

using Godot;
using Godot.Collections;

namespace Game.Managers;

public partial class UpgradeManager : Node
{

    [Export] private AbilityUpgrade[] _upgradePool = [];
    [Export] private ExperienceManager _experienceManager;
    [Export] private PackedScene _upgradeScreenScene;

    public partial class UpgradeState : Resource
    {
        public AbilityUpgrade Resource { get; set; }
        public int Quantity { get; set; }

        public void UpdateQuantity(int amount)
        {
            Quantity += amount;
        }
    }
    private readonly Dictionary<string, UpgradeState> _currentUpgrades = [];

    public override void _Ready()
    {
        _experienceManager.LevelUp += OnLevelUp;
    }

    private void OnLevelUp(int newLevel)
    {
        var chosenUpgrade = _upgradePool[GD.Randi() % _upgradePool.Length];
        if (chosenUpgrade == null) return;

        var upgradeScreenInstance = _upgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreenInstance);
        upgradeScreenInstance.SetAbilityUpgrades([chosenUpgrade]);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }

    private void ApplyUpgrade(AbilityUpgrade upgrade)
    {
        if (!_currentUpgrades.TryGetValue(upgrade.Id, out var data))
        {
            _currentUpgrades.Add(upgrade.Id, new UpgradeState()
            {
                Resource = upgrade,
                Quantity = 1
            });
        }
        else
        {
            data.UpdateQuantity(1);
            _currentUpgrades[upgrade.Id] = data;
        }

        GameEvents.Instance.EmitAbilityUpgradeAdded(upgrade, _currentUpgrades);
    }

    private void OnUpgradeSelected(AbilityUpgrade upgrade)
    {
        ApplyUpgrade(upgrade);
    }

}
