using Godot;

namespace Game.Resources.MetaUpgrades;

[GlobalClass]
public partial class MetaUpgrade : Resource
{

    [Export] public string Id;
    [Export] public int MaxQuantity = 1;
    [Export] public string Title;
    [Export(PropertyHint.MultilineText)] public string Description;

}
