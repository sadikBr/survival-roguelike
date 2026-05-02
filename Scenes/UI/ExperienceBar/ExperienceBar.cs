using Game.Managers;

using Godot;

namespace Game.UI;

public partial class ExperienceBar : CanvasLayer
{

    [Export] private ExperienceManager _experienceManager;

    private ProgressBar _progressBar;

    public override void _Ready()
    {
        _progressBar = GetNode<ProgressBar>("%ProgressBar");
        _progressBar.Value = 0.0f;

        _experienceManager.ExperienceUpdated += OnExperienceUpdated;
    }

    private void OnExperienceUpdated(int currentExp, int targetExp)
    {
        var progress = (float)currentExp / targetExp;
        _progressBar.Value = progress;
    }

}
