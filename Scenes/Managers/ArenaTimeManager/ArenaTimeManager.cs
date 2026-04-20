using Game.UI;

using Godot;

namespace Game.Managers;

public partial class ArenaTimeManager : Node
{

    [Export] private PackedScene _endScreenScene;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    public float GetTimeElapsed()
    {
        return (float)_timer.WaitTime - (float)_timer.TimeLeft;
    }

    private void OnTimerTimeout()
    {
        var endScreenInstance = _endScreenScene.Instantiate<EndScreen>();
        AddChild(endScreenInstance);
    }

}
