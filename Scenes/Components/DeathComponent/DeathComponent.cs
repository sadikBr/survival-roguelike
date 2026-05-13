using Game.Autoloads;

using Godot;

namespace Game.Components;

public partial class DeathComponent : Node2D
{
    private static readonly StringName AnimationPlayerDefault = new("default");

    [Export] private HealthComponent _healthComponent;
    [Export] private Sprite2D _sprite;

    private AnimationPlayer _animationPlayer;
    private GpuParticles2D _particles;
    private RandomAudioStreamPlayerComponent _randomAudioStreamPlayerComponent;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _particles = GetNode<GpuParticles2D>("GPUParticles2D");
        _randomAudioStreamPlayerComponent = GetNode<RandomAudioStreamPlayerComponent>("RandomAudioStreamPlayerComponent");

        _particles.Texture = _sprite.Texture;
        _healthComponent.Died += OnDied;
    }

    private void OnDied()
    {
        if (Owner is not Node2D owner) return;
        var spawnPosition = owner.GlobalPosition;

        var entitiesLayer = GetTree().GetFirstNodeInGroup("EntitiesLayer");
        GetParent().RemoveChild(this);
        entitiesLayer.AddChild(this);
        GlobalPosition = spawnPosition;

        _animationPlayer.Play(AnimationPlayerDefault);
        _randomAudioStreamPlayerComponent.PlayRandom();

        GameEvents.Instance.EmitEnemyKilled();
    }
}
