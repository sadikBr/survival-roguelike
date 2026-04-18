using Game.Managers;

using Godot;

namespace Game.UI;

public partial class ArenaTimeUI : CanvasLayer
{
    [Export]
    private ArenaTimeManager _arenaTimeManager;

    private Label _timeLabel;

    public override void _Ready()
    {
        _timeLabel = GetNode<Label>("%TimeLabel");
    }

    public override void _Process(double delta)
    {
        if (_arenaTimeManager == null) return;

        var timeElapsed = _arenaTimeManager.GetTimeElapsed();
        _timeLabel.Text = FormatTime(timeElapsed);
    }

    private string FormatTime(float seconds)
    {
        var minutes = (int)seconds / 60;
        var secondsRemaining = (int)seconds % 60;
        return $"{minutes:D2}:{secondsRemaining:D2}";
    }
}
