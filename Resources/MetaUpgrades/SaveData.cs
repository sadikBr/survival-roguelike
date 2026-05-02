using Godot;
using Godot.Collections;

namespace Game.Resources.MetaUpgrades;

public partial class SaveData : Resource
{
    [Export] public int MetaUpgradeCurrency { get; set; } = 0;
    [Export] public Dictionary<string, MetaUpgradeState> CurrentMetaUpgrades { get; set; } = [];
}
