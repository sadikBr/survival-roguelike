using Game.Autoloads;
using Game.Resources.MetaUpgrades;

using Godot;

namespace Game.UI;

public partial class MetaMenu : CanvasLayer
{

    [Export] private PackedScene _metaUpgradeCardScene;
    [Export] private MetaUpgrade[] _upgrades;

    private GridContainer _container;
    private Button _backButton;

    public override void _Ready()
    {
        _container = GetNode<GridContainer>("%GridContainer");
        _backButton = GetNode<Button>("%BackButton");
        _backButton.Pressed += OnBackButtonPressed;

        foreach (var upgrade in _upgrades)
        {
            var card = _metaUpgradeCardScene.Instantiate<MetaUpgradeCard>();
            _container.AddChild(card);
            card.SetMetaUpgrade(upgrade);
        }
    }

    private async void OnBackButtonPressed()
    {
        ScreenTransition.Instance.TransitionToScene("res://Scenes/UI/MainMenu/MainMenu.tscn");
    }

}
