using Game.Autoloads;
using Game.Resources.MetaUpgrades;

using Godot;

namespace Game.UI;

public partial class MetaUpgradeCard : PanelContainer
{

    private static readonly StringName SelectedAnimation = new("selected");

    private Label _titleLabel;
    private Label _descriptionLabel;
    private AnimationPlayer _animationPlayer;
    private Label _costLabel;
    private ProgressBar _costProgress;
    private Button _purchaseButton;

    private MetaUpgrade _upgrade;

    public override void _Ready()
    {
        _titleLabel = GetNode<Label>("%TitleLabel");
        _descriptionLabel = GetNode<Label>("%DescriptionLabel");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _costLabel = GetNode<Label>("%CostLabel");
        _costProgress = GetNode<ProgressBar>("%CostProgress");
        _purchaseButton = GetNode<Button>("%PurchaseButton");

        _purchaseButton.Pressed += OnPurchaseButtonPressed;
    }

    public void SetMetaUpgrade(MetaUpgrade upgrade)
    {
        _upgrade = upgrade;
        _titleLabel.Text = upgrade.Title;
        _descriptionLabel.Text = upgrade.Description;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        var currentProgress = MetaProgression.Instance.SaveData.MetaUpgradeCurrency;
        var target = _upgrade.ExperienceCost;
        var percent = Mathf.Min((float)currentProgress / target, 1.0f);

        _costLabel.Text = $"{Mathf.Min(currentProgress, target)}/{target}";
        _costProgress.Value = percent;

        _purchaseButton.Disabled = percent < 1.0f;
    }

    private void OnPurchaseButtonPressed()
    {
        if (_upgrade == null) return;

        MetaProgression.Instance.AddMetaUpgrade(_upgrade);
        GetTree().CallGroup("MetaUpgradeCard", nameof(UpdateProgress));
        _animationPlayer.Play(SelectedAnimation);
    }

}
