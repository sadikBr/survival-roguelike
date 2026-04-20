using Godot;

namespace Game.Components;

public partial class VialDropComponent : Node
{

    [Export(PropertyHint.Range, "0,1")] private float _dropPercent = 0.5f;
    [Export] private HealthComponent _healthComponent;
    [Export] private PackedScene _vialScene;

    public override void _Ready()
    {
        _healthComponent.Died += OnDied;
    }

    private void OnDied()
    {
        if (GD.Randf() > _dropPercent) return;

        if (_vialScene == null) return;

        if (Owner is not Node2D owner) return;

        var spawnPosition = owner.GlobalPosition;
        var instance = _vialScene.Instantiate<Node2D>();

        owner.GetParent().AddChild(instance);
        instance.GlobalPosition = spawnPosition;
    }
}
