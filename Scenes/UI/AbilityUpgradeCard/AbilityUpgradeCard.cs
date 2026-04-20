using Game.Resources.Upgrades;

using Godot;

namespace Game.UI;

public partial class AbilityUpgradeCard : PanelContainer
{

    [Signal] public delegate void CardSelectedEventHandler();

    private static readonly StringName ActionLeftClick = new("left_click");

    private Label _nameLabel;
    private Label _descriptionLabel;

    public override void _Ready()
    {
        _nameLabel = GetNode<Label>("%NameLabel");
        _descriptionLabel = GetNode<Label>("%DescriptionLabel");
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed(ActionLeftClick))
        {
            EmitSignal(SignalName.CardSelected);
        }
    }

    public void SetAbilityUpgrade(AbilityUpgrade upgrade)
    {
        _nameLabel.Text = upgrade.Name;
        _descriptionLabel.Text = upgrade.Description;
    }

}
