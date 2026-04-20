using System;

using Game.Autoloads;

using Godot;

namespace Game.Managers;

public partial class ExperienceManager : Node
{

    public int CurrentExperience { get; private set; }

    public override void _Ready()
    {
        GameEvents.Instance.ExperienceVialCollected += OnExperienceVialCollected;
    }

    public void IncrementExperience(int amount)
    {
        CurrentExperience += amount;
    }

    private void OnExperienceVialCollected(int amount)
    {
        IncrementExperience(amount);
    }

}
