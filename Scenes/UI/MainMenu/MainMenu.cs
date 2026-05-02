using Game.Autoloads;

using Godot;

namespace Game.UI;

public partial class MainMenu : CanvasLayer
{

    [Export] private PackedScene _optionsScene;

    private Button _playButton;
    private Button _upgradesButton;
    private Button _optionsButton;
    private Button _quitButton;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("%PlayButton");
        _upgradesButton = GetNode<Button>("%UpgradesButton");
        _optionsButton = GetNode<Button>("%OptionsButton");
        _quitButton = GetNode<Button>("%QuitButton");

        _playButton.Pressed += OnPlayButtonPressed;
        _upgradesButton.Pressed += OnUpgradesButtonPressed;
        _optionsButton.Pressed += OnOptionsButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    private async void OnPlayButtonPressed()
    {
        ScreenTransition.Instance.TransitionToScene("res://Scenes/Main/Main.tscn");
    }

    private async void OnUpgradesButtonPressed()
    {
        ScreenTransition.Instance.TransitionToScene("res://Scenes/UI/MetaMenu/MetaMenu.tscn");
    }

    private async void OnOptionsButtonPressed()
    {
        ScreenTransition.Instance.Transition();
        await ToSignal(ScreenTransition.Instance, ScreenTransition.SignalName.TransitionedHalfway);
        var instance = _optionsScene.Instantiate<OptionsMenu>();
        AddChild(instance);
        instance.BackButtonPressed += () => OnOptionsScreenBackButtonPressed(instance);
    }

    private void OnOptionsScreenBackButtonPressed(OptionsMenu optionsMenu)
    {
        optionsMenu.QueueFree();
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

}
