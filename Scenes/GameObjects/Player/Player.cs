using System;

using Game.Autoloads;
using Game.Components;
using Game.Managers;
using Game.Resources.Upgrades;
using Game.Upgrades;

using Godot;
using Godot.Collections;

namespace Game.GameObjects.Entities;

public partial class Player : CharacterBody2D
{
    private static readonly StringName ActionMoveLeft = new("move_left");
    private static readonly StringName ActionMoveRight = new("move_right");
    private static readonly StringName ActionMoveUp = new("move_up");
    private static readonly StringName ActionMoveDown = new("move_down");

    private static readonly StringName WalkAnimation = new("walk");
    private static readonly StringName ResetAnimation = new("RESET");

    private int _numberOfCollidingBodies;
    private float _baseSpeed;

    private AnimationPlayer _animationPlayer;
    private Node2D _visuals;
    private Area2D _collisionArea;
    private HealthComponent _healthComponent;
    private VelocityComponent _velocityComponent;
    private Timer _damageIntervalTimer;
    private ProgressBar _healthBar;
    private Node _abilities;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _visuals = GetNode<Node2D>("Visuals");

        _collisionArea = GetNode<Area2D>("CollisionArea");
        _collisionArea.BodyEntered += OnBodyEntered;
        _collisionArea.BodyExited += OnBodyExited;

        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        _healthComponent.HealthChanged += OnHealthChanged;

        _velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
        _baseSpeed = _velocityComponent.MaxSpeed;

        _damageIntervalTimer = GetNode<Timer>("DamageIntervalTimer");
        _damageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;

        _healthBar = GetNode<ProgressBar>("HealthBar");
        UpdateHealthDisplay();

        _abilities = GetNode<Node>("Abilities");
        GameEvents.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
    }

    public override void _Process(double delta)
    {
        var direction = Input.GetVector(ActionMoveLeft, ActionMoveRight, ActionMoveUp, ActionMoveDown);
        _velocityComponent.AccelerateInDirection(direction);
        _velocityComponent.Move(this);

        if (direction.Length() > 0)
        {
            _animationPlayer.Play(WalkAnimation);
        }
        else
        {
            _animationPlayer.Play(ResetAnimation);
        }
        var moveSign = Mathf.Sign(direction.X);
        _visuals.Scale = moveSign == 0 ? Vector2.One : new Vector2(moveSign, 1);
    }

    public HealthComponent GetHealthComponent()
    {
        return _healthComponent;
    }

    public void CheckDealDamage()
    {
        if (_numberOfCollidingBodies == 0 || !_damageIntervalTimer.IsStopped()) return;

        _healthComponent.Damage(1);
        _damageIntervalTimer.Start();
    }

    private void UpdateHealthDisplay()
    {
        _healthBar.Value = _healthComponent.GetHealthPercentage();
    }

    private void OnBodyEntered(Node2D body)
    {
        _numberOfCollidingBodies += 1;
        CheckDealDamage();
    }

    private void OnBodyExited(Node2D body)
    {
        _numberOfCollidingBodies -= 1;
    }

    private void OnDamageIntervalTimerTimeout()
    {
        CheckDealDamage();
    }

    private void OnHealthChanged(bool isDamage)
    {
        if (isDamage)
        {
            GameEvents.Instance.EmitPlayerDamaged();
        }
        UpdateHealthDisplay();
    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeManager.UpgradeState> currentUpgrades)
    {
        if (upgrade is Ability newAbility)
        {
            var abilityInstance = newAbility.AbilityControllerScene.Instantiate();
            _abilities.AddChild(abilityInstance);
        }
        else if (upgrade.Id == UpgradeStringNames.PlayerSpeedUpgrade)
        {
            var newMaxSpeed = _baseSpeed + _baseSpeed * currentUpgrades[upgrade.Id].Quantity * 0.25f;
            _velocityComponent.UpdateMaxSpeed(newMaxSpeed);
        }
    }

}
