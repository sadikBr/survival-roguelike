using Game.Autoloads;
using Game.MetaUpgrades;

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
        var adjustedDropPercent = _dropPercent;

        var upgradeCount = MetaProgression.Instance.GetMetaUpgradeCount(MetaUpgradeStringNames.ExperienceGain);
        GD.Print(upgradeCount);
        if (upgradeCount > 0)
        {
            adjustedDropPercent += upgradeCount * 0.1f;
        }

        if (GD.Randf() > adjustedDropPercent) return;

        if (_vialScene == null) return;

        if (Owner is not Node2D owner) return;

        var spawnPosition = owner.GlobalPosition;
        var instance = _vialScene.Instantiate<Node2D>();

        var entitiesLayer = GetTree().GetFirstNodeInGroup("EntitiesLayer");
        entitiesLayer.AddChild(instance);
        instance.GlobalPosition = spawnPosition;
    }
}
