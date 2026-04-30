using Game.Autoloads;

using Godot;

namespace Game.UI;

public partial class Vignette : CanvasLayer
{

    private static readonly StringName HitAnimation = new("hit");

    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        GameEvents.Instance.PlayerDamaged += OnPlayerDamaged;
    }

    public override void _ExitTree()
    {
        GameEvents.Instance.PlayerDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged()
    {
        _animationPlayer.Play(HitAnimation);
    }
}
