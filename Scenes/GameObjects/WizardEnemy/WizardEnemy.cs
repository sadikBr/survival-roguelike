using Game.Components;

using Godot;

namespace Game.GameObjects.Entities;

public partial class WizardEnemy : CharacterBody2D
{

    private VelocityComponent _velocityComponent;
    private Node2D _visuals;

    public override void _Ready()
    {
        _velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        _visuals = GetNode<Node2D>("Visuals");
    }

    public override void _Process(double delta)
    {
        _velocityComponent.AccelerateToPlayer();
        _velocityComponent.Move(this);

        var moveSign = Mathf.Sign(Velocity.X);
        _visuals.Scale = moveSign == 0 ? Vector2.One : new Vector2(-moveSign, 1);
    }

}
