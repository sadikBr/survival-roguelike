using Godot;

namespace Game.UI;

public partial class EndScreen : CanvasLayer
{

    private PanelContainer _panelContainer;
    private Label _titleLabel;
    private Label _subtitleLabel;
    private Button _restartButton;
    private Button _quitButton;

    public override void _Ready()
    {
        _panelContainer = GetNode<PanelContainer>("%PanelContainer");
        _panelContainer.PivotOffsetRatio = Vector2.One / 2.0f;
        var tween = CreateTween();
        tween.TweenProperty(_panelContainer, Control.PropertyName.Scale.ToString(), Vector2.Zero, 0.0f);
        tween.TweenProperty(_panelContainer, Control.PropertyName.Scale.ToString(), Vector2.One, 0.3f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);

        GetTree().Paused = true;
        _titleLabel = GetNode<Label>("%TitleLabel");
        _subtitleLabel = GetNode<Label>("%SubtitleLabel");
        _restartButton = GetNode<Button>("%RestartButton");
        _quitButton = GetNode<Button>("%QuitButton");

        _restartButton.Pressed += OnRestartButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    public void SetDefeat()
    {
        _titleLabel.Text = "Defeat";
        _subtitleLabel.Text = "You Lost!";
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnRestartButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/Main/Main.tscn");
    }
}
