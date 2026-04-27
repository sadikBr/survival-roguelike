using System;
using System.Linq;
using System.Threading.Tasks;

using Game.Resources.Upgrades;

using Godot;

namespace Game.UI;

public partial class AbilityUpgradeCard : PanelContainer
{

    [Signal] public delegate void CardSelectedEventHandler();

    private static readonly StringName InAnimation = new("in");
    private static readonly StringName HoverAnimation = new("hover");
    private static readonly StringName SelectedAnimation = new("selected");
    private static readonly StringName DiscardAnimation = new("discard");

    private static readonly StringName ActionLeftClick = new("left_click");

    private Label _nameLabel;
    private Label _descriptionLabel;
    private AnimationPlayer _animationPlayer;
    private AnimationPlayer _hoverAnimationPlayer;

    private bool _isDisabled = false;

    public override void _Ready()
    {
        _nameLabel = GetNode<Label>("%NameLabel");
        _descriptionLabel = GetNode<Label>("%DescriptionLabel");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _hoverAnimationPlayer = GetNode<AnimationPlayer>("HoverAnimationPlayer");

        MouseEntered += OnMouseEntered;
    }

    public override async void _GuiInput(InputEvent @event)
    {
        if (_isDisabled) return;
        if (@event.IsActionPressed(ActionLeftClick))
        {
            _isDisabled = true;
            _animationPlayer.Play(SelectedAnimation);

            foreach (var card in GetTree().GetNodesInGroup("UpgradeCard").Cast<AbilityUpgradeCard>())
            {
                if (card != this)
                {
                    card.PlayDiscardAnimation();
                }
            }

            await ToSignal(_animationPlayer, AnimationMixer.SignalName.AnimationFinished);
            EmitSignal(SignalName.CardSelected);
        }
    }

    public void SetAbilityUpgrade(AbilityUpgrade upgrade)
    {
        _nameLabel.Text = upgrade.Name;
        _descriptionLabel.Text = upgrade.Description;
    }

    public async void PlayInAnimation(float delay = 0.0f)
    {
        Modulate = Colors.Transparent;
        await Task.Delay((int)(delay * 1000));
        _animationPlayer.Play(InAnimation);
    }

    public void PlayDiscardAnimation()
    {
        _animationPlayer.Play(DiscardAnimation);
    }

    private void OnMouseEntered()
    {
        if (_isDisabled) return;
        _hoverAnimationPlayer.Play(HoverAnimation);
    }

}
