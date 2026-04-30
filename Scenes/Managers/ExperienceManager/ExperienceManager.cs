using Game.Autoloads;
using Game.Components;

using Godot;

namespace Game.Managers;

public partial class ExperienceManager : Node
{

    [Signal] public delegate void ExperienceUpdatedEventHandler(int currentExp, int targetExp);
    [Signal] public delegate void LevelUpEventHandler(int newLevel);

    private const int TargetExperienceIncrement = 5;

    public int CurrentExperience { get; private set; }
    public int TargetExperience { get; private set; } = 1;
    public int CurrentLevel { get; private set; } = 1;

    public override void _Ready()
    {
        GameEvents.Instance.ExperienceVialCollected += OnExperienceVialCollected;
    }

    public void IncrementExperience(int amount)
    {
        CurrentExperience = Mathf.Clamp(CurrentExperience + amount, 0, TargetExperience);
        EmitSignal(SignalName.ExperienceUpdated, CurrentExperience, TargetExperience);

        if (CurrentExperience == TargetExperience)
        {
            CurrentLevel += 1;
            TargetExperience += TargetExperienceIncrement;
            CurrentExperience = 0;
            EmitSignal(SignalName.ExperienceUpdated, CurrentExperience, TargetExperience);
            EmitSignal(SignalName.LevelUp, CurrentLevel);
        }
    }

    private void OnExperienceVialCollected(int amount)
    {
        IncrementExperience(amount);
    }

}
