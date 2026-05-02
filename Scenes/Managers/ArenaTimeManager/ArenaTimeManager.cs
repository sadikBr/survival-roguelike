using Game.Autoloads;
using Game.UI;

using Godot;

namespace Game.Managers;

public partial class ArenaTimeManager : Node
{

    [Signal] public delegate void ArenaDifficultyIncreasedEventHandler(int arenaDifficulty);

    [Export] private PackedScene _endScreenScene;

    private const int DifficultyInterval = 5;
    private int _arenaDifficulty;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    public override void _Process(double delta)
    {
        var nextTimeTarget = _timer.WaitTime - (_arenaDifficulty + 1) * DifficultyInterval;
        if (_timer.TimeLeft <= nextTimeTarget)
        {
            _arenaDifficulty += 1;
            EmitSignal(SignalName.ArenaDifficultyIncreased, _arenaDifficulty);
        }
    }

    public float GetTimeElapsed()
    {
        return (float)_timer.WaitTime - (float)_timer.TimeLeft;
    }

    private void OnTimerTimeout()
    {
        var endScreenInstance = _endScreenScene.Instantiate<EndScreen>();
        AddChild(endScreenInstance);
        endScreenInstance.PlayJingle();
        MetaProgression.Instance.Save();
    }

}
