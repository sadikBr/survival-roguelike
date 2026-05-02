using Godot;

namespace Game.Resources.MetaUpgrades;

public partial class MetaUpgradeState : Resource
{
    [Export] public MetaUpgrade MetaUpgrade { get; set; }
    [Export] public int Quantity { get; set; } = 0;
}
