using Godot;

namespace Game.Autoloads;

public partial class MusicPlayer : AudioStreamPlayer
{

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");

        Finished += OnMusicFinished;
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnMusicFinished()
    {
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        Play();
    }
}
