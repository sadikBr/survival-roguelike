using Game.Resources.Upgrades;

using Godot;
using Godot.Collections;

using static Game.Managers.UpgradeManager;

namespace Game.Autoloads;

public partial class GameEvents : Node
{
    [Signal] public delegate void ExperienceVialCollectedEventHandler(int amount);
    [Signal] public delegate void AbilityUpgradeAddedEventHandler(AbilityUpgrade upgrade, Dictionary<string, UpgradeState> currentUpgrades);
    [Signal] public delegate void PlayerDamagedEventHandler();

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

    public void EmitAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeState> currentUpgrades)
    {
        EmitSignal(SignalName.AbilityUpgradeAdded, upgrade, currentUpgrades);
    }

    public void EmitPlayerDamaged()
    {
        EmitSignal(SignalName.PlayerDamaged);
    }
}
