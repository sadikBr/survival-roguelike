using Godot;

namespace Game.Managers;

public partial class ArenaTimeManager : Node
{
    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
    }

    public float GetTimeElapsed()
    {
        return (float)_timer.WaitTime - (float)_timer.TimeLeft;
    }

}
