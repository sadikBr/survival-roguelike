using System;

using Game.Components;

using Godot;

namespace Game.UI;

public partial class SoundButton : Button
{

    private RandomUIAudioStreamPlayerComponent _randomUIAudioStreamPlayerComponent;

    public override void _Ready()
    {
        _randomUIAudioStreamPlayerComponent = GetNode<RandomUIAudioStreamPlayerComponent>("RandomUIAudioStreamPlayerComponent");
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        _randomUIAudioStreamPlayerComponent.PlayRandom();
    }
}
