using Godot;

namespace Game.Resources.Upgrades;

[GlobalClass]
public partial class AbilityUpgrade : Resource
{

    [Export] public string Id;
    [Export] public string Name;
    [Export(PropertyHint.MultilineText)] public string Description;

}
