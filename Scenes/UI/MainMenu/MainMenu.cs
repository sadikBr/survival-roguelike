using Godot;

namespace Game.UI;

public partial class MainMenu : CanvasLayer
{

    [Export] private PackedScene _optionsScene;

    private Button _playButton;
    private Button _optionsButton;
    private Button _quitButton;

    public override void _Ready()
    {
        _playButton = GetNode<Button>("%PlayButton");
        _optionsButton = GetNode<Button>("%OptionsButton");
        _quitButton = GetNode<Button>("%QuitButton");

        _playButton.Pressed += OnPlayButtonPressed;
        _optionsButton.Pressed += OnOptionsButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    private void OnPlayButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Main/Main.tscn");
    }

    private void OnOptionsButtonPressed()
    {
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
