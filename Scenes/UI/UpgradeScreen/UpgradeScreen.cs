using System;

using Game.Resources.Upgrades;

using Godot;

namespace Game.UI;

public partial class UpgradeScreen : CanvasLayer
{

    [Signal] public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

    [Export] private PackedScene _upgradeCardScene;

    private static readonly StringName OutAnimation = new("out");

    private HBoxContainer _cardContainer;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        GetTree().Paused = true;
        _cardContainer = GetNode<HBoxContainer>("%CardContainer");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void SetAbilityUpgrades(AbilityUpgrade[] upgrades)
    {
        var delay = 0.0f;
        foreach (var upgrade in upgrades)
        {
            var cardInstance = _upgradeCardScene.Instantiate<AbilityUpgradeCard>();
            _cardContainer.AddChild(cardInstance);

            cardInstance.SetAbilityUpgrade(upgrade);
            cardInstance.PlayInAnimation(delay);
            cardInstance.CardSelected += () => OnCardSelected(upgrade);
            delay += .2f;
        }
    }

    private async void OnCardSelected(AbilityUpgrade upgrade)
    {
        EmitSignal(SignalName.UpgradeSelected, upgrade);
        _animationPlayer.Play(OutAnimation);
        await ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);

        GetTree().Paused = false;
        QueueFree();
    }
}
