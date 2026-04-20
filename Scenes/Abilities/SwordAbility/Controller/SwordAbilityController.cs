using System.Linq;

using Game.GameObjects.Entities;

using Godot;

namespace Game.Abilities.Controllers;

public partial class SwordAbilityController : Node
{
    [Export]
    private PackedScene _swordAbility;

    private const float MaxRange = 150.0f;
    private int _damage = 5;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;
        var enemies = GetTree().GetNodesInGroup("Enemy").ToList();
        var filteredEnemies = enemies.Where(e =>
        {
            var enemy = e as Node2D;
            return enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) < Mathf.Pow(MaxRange, 2.0f);
        }).ToList();

        if (filteredEnemies.Count == 0) return;

        filteredEnemies.Sort((a, b) =>
        {
            var distanceA = ((Node2D)a).GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
            var distanceB = ((Node2D)b).GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
            return distanceA.CompareTo(distanceB);
        });
        var target = filteredEnemies[0] as Node2D;

        var instance = _swordAbility.Instantiate<SwordAbility>();
        player.GetParent().AddChild(instance);
        instance.SetDamage(_damage);

        instance.GlobalPosition = target.GlobalPosition;
        instance.GlobalPosition += Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau)) * 4.0f;

        var enemyDirection = target.GlobalPosition.DirectionTo(instance.GlobalPosition);
        instance.Rotation = enemyDirection.Angle();
    }
}
