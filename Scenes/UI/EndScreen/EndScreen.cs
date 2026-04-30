using Game.Autoloads;

using Godot;

namespace Game.UI;

public partial class EndScreen : CanvasLayer
{

    private PanelContainer _panelContainer;
    private Label _titleLabel;
    private Label _subtitleLabel;
    private Button _restartButton;
    private Button _quitButton;
    private AudioStreamPlayer _victoryAudioStreamPlayer;
    private AudioStreamPlayer _defeatAudioStreamPlayer;

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
        _victoryAudioStreamPlayer = GetNode<AudioStreamPlayer>("VictoryAudioStreamPlayer");
        _defeatAudioStreamPlayer = GetNode<AudioStreamPlayer>("DefeatAudioStreamPlayer");

        _restartButton.Pressed += OnRestartButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    public void SetDefeat()
    {
        _titleLabel.Text = "Defeat";
        _subtitleLabel.Text = "You Lost!";
        PlayJingle(true);
    }

    public void PlayJingle(bool defeat = false)
    {
        if (defeat)
        {
            _defeatAudioStreamPlayer.Play();
        }
        else
        {
            _victoryAudioStreamPlayer.Play();
        }
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private async void OnRestartButtonPressed()
    {
        ScreenTransition.Instance.Transition();
        await ToSignal(ScreenTransition.Instance, ScreenTransition.SignalName.TransitionedHalfway);
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/Main/Main.tscn");
    }
}
