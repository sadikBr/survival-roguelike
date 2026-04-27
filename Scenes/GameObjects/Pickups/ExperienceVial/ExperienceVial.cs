using Game.Autoloads;
using Game.GameObjects.Entities;

using Godot;

namespace Game.GameObjects.Pickups;

public partial class ExperienceVial : Node2D
{

    private Area2D _area2D;
    private CollisionShape2D _collisionShape;
    private Sprite2D _sprite;

    public override void _Ready()
    {
        _area2D = GetNode<Area2D>("Area2D");
        _area2D.AreaEntered += OnAreaEntered;

        _collisionShape = GetNode<CollisionShape2D>("%CollisionShape2D");
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }

    private void OnAreaEntered(Area2D area)
    {
        Callable.From(DisableCollision).CallDeferred();

        var startPosition = GlobalPosition;

        var tween = CreateTween();
        tween.SetParallel();
        tween.TweenMethod(Callable.From((float percent) => TweenCollect(percent, startPosition)), 0.0f, 1.0f, .5f)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Back);
        tween.TweenProperty(_sprite, Node2D.PropertyName.Scale.ToString(), Vector2.Zero, 0.05f).SetDelay(0.45f);
        tween.Chain(); // this makes the tween wait for the previous animations to complete before starting the next ones.
        tween.TweenCallback(Callable.From(Collect));
    }

    private void DisableCollision()
    {
        _collisionShape.Disabled = true;
    }

    private void Collect()
    {
        GameEvents.Instance.EmitExperienceVialCollected(1);
        QueueFree();
    }

    private void TweenCollect(float percent, Vector2 startPosition)
    {
        if (GetTree().GetFirstNodeInGroup(nameof(Player)) is not Player player) return;

        GlobalPosition = startPosition.Lerp(player.GlobalPosition, percent);

        var directionFromStart = startPosition.DirectionTo(player.GlobalPosition);
        var targetRotation = directionFromStart.Angle() + (Mathf.Pi / 2);
        Rotation = Mathf.LerpAngle(Rotation, targetRotation, 1 - Mathf.Exp(-2.0f * (float)GetProcessDeltaTime()));
    }
}
