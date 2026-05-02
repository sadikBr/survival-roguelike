using Game.Resources.MetaUpgrades;

using Godot;

namespace Game.UI;

public partial class MetaMenu : CanvasLayer
{

    [Export] private PackedScene _metaUpgradeCardScene;
    [Export] private MetaUpgrade[] _upgrades;

    private GridContainer _container;

    public override void _Ready()
    {
        _container = GetNode<GridContainer>("%GridContainer");

        foreach (var upgrade in _upgrades)
        {
            var card = _metaUpgradeCardScene.Instantiate<MetaUpgradeCard>();
            _container.AddChild(card);
            card.SetMetaUpgrade(upgrade);
        }
    }



}
