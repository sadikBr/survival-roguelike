using Godot;
using Godot.Collections;

namespace Game.Components;

public partial class RandomUIAudioStreamPlayerComponent : AudioStreamPlayer
{

    [Export] private Array<AudioStream> _streams;
    [Export] private bool _randomizePitch = true;
    [Export] private float _minPitch = 0.9f;
    [Export] private float _maxPitch = 1.1f;

    public void PlayRandom()
    {
        if (_streams == null || _streams.Count == 0) return;

        PitchScale = _randomizePitch ? (float)GD.RandRange(_minPitch, _maxPitch) : 1.0f;

        Stream = _streams.PickRandom();
        Play();
    }

}
