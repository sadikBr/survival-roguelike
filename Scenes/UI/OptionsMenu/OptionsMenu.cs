using System;

using Game.Autoloads;

using Godot;

namespace Game.UI;

public partial class OptionsMenu : CanvasLayer
{

    [Signal] public delegate void BackButtonPressedEventHandler();

    private HSlider _sfxSlider;
    private HSlider _musicSlider;
    private Button _windowModeButton;
    private Button _backButton;

    public override void _Ready()
    {
        _sfxSlider = GetNode<HSlider>("%SFXSlider");
        _musicSlider = GetNode<HSlider>("%MusicSlider");
        _windowModeButton = GetNode<Button>("%WindowModeButton");
        _backButton = GetNode<Button>("%BackButton");

        UpdateDisplay();

        _sfxSlider.ValueChanged += OnSFXSliderChanged;
        _musicSlider.ValueChanged += OnMusicSliderChanged;
        _windowModeButton.Pressed += OnWindowModeButtonPressed;
        _backButton.Pressed += OnBackButtonPressed;
    }

    public void UpdateDisplay()
    {
        _sfxSlider.Value = GetBusVolumePercent("SFX");
        _musicSlider.Value = GetBusVolumePercent("Music");
        _windowModeButton.Text = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen ? "Fullscreen" : "Windowed";
    }

    private float GetBusVolumePercent(string busName)
    {
        var busIndex = AudioServer.GetBusIndex(busName);
        var volumeDb = AudioServer.GetBusVolumeDb(busIndex);

        return Mathf.DbToLinear(volumeDb);
    }

    private void SetBusVolumePercent(string busName, float percent)
    {
        var busIndex = AudioServer.GetBusIndex(busName);
        AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(percent));
    }

    private void OnSFXSliderChanged(double value)
    {
        SetBusVolumePercent("SFX", (float)value);
    }

    private void OnMusicSliderChanged(double value)
    {
        SetBusVolumePercent("Music", (float)value);
    }

    private void OnWindowModeButtonPressed()
    {
        var mode = DisplayServer.WindowGetMode();
        if (mode != DisplayServer.WindowMode.Fullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else
        {
            DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
        UpdateDisplay();
    }

    private async void OnBackButtonPressed()
    {
        ScreenTransition.Instance.Transition();
        await ToSignal(ScreenTransition.Instance, ScreenTransition.SignalName.TransitionedHalfway);
        EmitSignal(SignalName.BackButtonPressed);
    }

}
