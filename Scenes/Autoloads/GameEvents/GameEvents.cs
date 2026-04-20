using Godot;

namespace Game.Autoloads;

public partial class GameEvents : Node
{
    [Signal] public delegate void ExperienceVialCollectedEventHandler(int amount);

    public static GameEvents Instance { get; private set; }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            Instance = this;
        }
    }

    public void EmitExperienceVialCollected(int amount)
    {
        EmitSignal(SignalName.ExperienceVialCollected, amount);
    }
}
