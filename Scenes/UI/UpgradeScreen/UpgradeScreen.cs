using System;

using Game.Resources.Upgrades;

using Godot;

namespace Game.UI;

public partial class UpgradeScreen : CanvasLayer
{

    [Signal] public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

    [Export] private PackedScene _upgradeCardScene;

    private HBoxContainer _cardContainer;

    public override void _Ready()
    {
        GetTree().Paused = true;
        _cardContainer = GetNode<HBoxContainer>("%CardContainer");
    }

    public void SetAbilityUpgrades(AbilityUpgrade[] upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            var cardInstance = _upgradeCardScene.Instantiate<AbilityUpgradeCard>();
            _cardContainer.AddChild(cardInstance);

            cardInstance.SetAbilityUpgrade(upgrade);
            cardInstance.CardSelected += () => OnCardSelected(upgrade);
        }
    }

    private void OnCardSelected(AbilityUpgrade upgrade)
    {
        EmitSignal(SignalName.UpgradeSelected, upgrade);
        GetTree().Paused = false;
        QueueFree();
    }
}
