using Godot;

namespace Game.UI;

public partial class MainMenu : CanvasLayer
{

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
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

}
