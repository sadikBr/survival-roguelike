using Game.Autoloads;
using Game.Resources.Upgrades;
using Game.UI;
using Game.Utils;

using Godot;
using Godot.Collections;

namespace Game.Managers;

public partial class UpgradeManager : Node
{

    [Export] private ExperienceManager _experienceManager;
    [Export] private PackedScene _upgradeScreenScene;
    [Export] private int _numberOfUpgradesToPick = 2;

    private ResourcePreloader _resourcePreloader;

    private WeightedTable<AbilityUpgrade> _upgradePool;

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
        _upgradePool = new();
        _resourcePreloader = GetNode<ResourcePreloader>("ResourcePreloader");
        _experienceManager.LevelUp += OnLevelUp;

        _upgradePool.AddItem((AbilityUpgrade)_resourcePreloader.GetResource("axe"), 10);
        _upgradePool.AddItem((AbilityUpgrade)_resourcePreloader.GetResource("sword_rate"), 10);
        _upgradePool.AddItem((AbilityUpgrade)_resourcePreloader.GetResource("sword_damage"), 10);
    }

    private void OnLevelUp(int newLevel)
    {
        var chosenUpgrades = PickUpgrades();

        var upgradeScreenInstance = _upgradeScreenScene.Instantiate<UpgradeScreen>();
        AddChild(upgradeScreenInstance);
        upgradeScreenInstance.SetAbilityUpgrades(chosenUpgrades);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }

    private AbilityUpgrade[] PickUpgrades()
    {
        return _upgradePool.Count < _numberOfUpgradesToPick
            ? _upgradePool.ToArray()
            : _upgradePool.PickItems(_numberOfUpgradesToPick);
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

        if (upgrade.MaxQuantity > 0)
        {
            var currentQuantity = _currentUpgrades[upgrade.Id].Quantity;
            if (currentQuantity == upgrade.MaxQuantity)
            {
                _upgradePool.RemoveItem(upgrade);
            }
        }

        UpdateUpgradePool(upgrade);
        GameEvents.Instance.EmitAbilityUpgradeAdded(upgrade, _currentUpgrades);
    }

    private void OnUpgradeSelected(AbilityUpgrade upgrade)
    {
        ApplyUpgrade(upgrade);
    }

    private void UpdateUpgradePool(AbilityUpgrade upgrade)
    {
        if (upgrade.Id == "axe")
        {
            _upgradePool.AddItem((AbilityUpgrade)_resourcePreloader.GetResource("axe_damage"), 10);
        }
    }

}
