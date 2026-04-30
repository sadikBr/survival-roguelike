using Game.Autoloads;

using Godot;

namespace Game.UI;

public partial class PauseMenu : CanvasLayer
{

    private static readonly StringName ActionPause = new("pause");
    private static readonly StringName DefaultAnimation = new("default");

    [Export] private PackedScene _optionsScene;

    private AnimationPlayer _animationPlayer;
    private PanelContainer _panelContainer;
    private Button _resumeButton;
    private Button _optionsButton;
    private Button _quitToMenuButton;

    private bool _isClosing = false;

    public override void _Ready()
    {
        GetTree().Paused = true;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.Play(DefaultAnimation);

        _panelContainer = GetNode<PanelContainer>("PanelContainer");
        _resumeButton = GetNode<Button>("%ResumeButton");
        _optionsButton = GetNode<Button>("%OptionsButton");
        _quitToMenuButton = GetNode<Button>("%QuitButton");

        _resumeButton.Pressed += OnResumeButtonPressed;
        _optionsButton.Pressed += OnOptionsButtonPressed;
        _quitToMenuButton.Pressed += OnQuitToMenuButtonPressed;

        var tween = CreateTween();
        tween.TweenProperty(_panelContainer, Control.PropertyName.Scale.ToString(), Vector2.Zero, 0.0f);
        tween.TweenProperty(_panelContainer, Control.PropertyName.Scale.ToString(), Vector2.One, 0.3f)
            .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed(ActionPause))
        {
            Close();
            GetTree().Root.SetInputAsHandled();
        }
    }

    private async void Close()
    {
        if (_isClosing) return;

        _isClosing = true;
        _animationPlayer.PlayBackwards(DefaultAnimation);

        var tween = CreateTween();
        tween.TweenProperty(_panelContainer, Control.PropertyName.Scale.ToString(), Vector2.One, 0.0f);
        tween.TweenProperty(_panelContainer, Control.PropertyName.Scale.ToString(), Vector2.Zero, 0.3f)
            .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Back);

        await ToSignal(tween, Tween.SignalName.Finished);
        GetTree().Paused = false;
        QueueFree();
    }

    private void OnResumeButtonPressed()
    {
        Close();
    }

    private async void OnOptionsButtonPressed()
    {
        ScreenTransition.Instance.Transition();
        await ToSignal(ScreenTransition.Instance, ScreenTransition.SignalName.TransitionedHalfway);
        var optionsInstance = _optionsScene.Instantiate<OptionsMenu>();
        AddChild(optionsInstance);
        optionsInstance.BackButtonPressed += () => OnOptionsBackButtonPressed(optionsInstance);
    }

    private void OnOptionsBackButtonPressed(OptionsMenu menu)
    {
        menu.QueueFree();
    }

    private async void OnQuitToMenuButtonPressed()
    {
        ScreenTransition.Instance.Transition();
        await ToSignal(ScreenTransition.Instance, ScreenTransition.SignalName.TransitionedHalfway);
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/UI/MainMenu/MainMenu.tscn");
    }
}
