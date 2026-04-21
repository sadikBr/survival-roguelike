using Game.GameObjects.Entities;

using Godot;

namespace Game.Abilities.Controllers;

public partial class AxeAbilityController : Node
{

    [Export] private PackedScene _axeAbilityScene;

    private int _damage = 10;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        var axeAbilityInstance = _axeAbilityScene.Instantiate<AxeAbility>();
        var foregroundLayer = GetTree().GetFirstNodeInGroup("ForegroundLayer");
        foregroundLayer.AddChild(axeAbilityInstance);

        axeAbilityInstance.SetDamage(_damage);
        axeAbilityInstance.GlobalPosition = player.GlobalPosition;
    }
}
