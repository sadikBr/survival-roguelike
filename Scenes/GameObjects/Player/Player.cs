using System;

using Game.Autoloads;
using Game.Components;
using Game.Managers;
using Game.Resources.Upgrades;

using Godot;
using Godot.Collections;

namespace Game.GameObjects.Entities;

public partial class Player : CharacterBody2D
{
    private static readonly StringName ActionMoveLeft = new("move_left");
    private static readonly StringName ActionMoveRight = new("move_right");
    private static readonly StringName ActionMoveUp = new("move_up");
    private static readonly StringName ActionMoveDown = new("move_down");

    private const float MaxSpeed = 125.0f;
    private const float Acceleration = 25.0f;

    private int _numberOfCollidingBodies;

    private Area2D _collisionArea;
    private HealthComponent _healthComponent;
    private Timer _damageIntervalTimer;
    private ProgressBar _healthBar;
    private Node _abilities;

    public override void _Ready()
    {
        _collisionArea = GetNode<Area2D>("CollisionArea");
        _collisionArea.BodyEntered += OnBodyEntered;
        _collisionArea.BodyExited += OnBodyExited;

        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        _healthComponent.HealthChanged += OnHealthChanged;

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
        var targetVelocity = direction * MaxSpeed;

        Velocity = Velocity.Lerp(targetVelocity, 1 - Mathf.Exp((float)delta * -Acceleration));
        MoveAndSlide();
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
        UpdateHealthDisplay();
    }

    private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, UpgradeManager.UpgradeState> currentUpgrades)
    {
        if (upgrade is not Ability newAbility) return;

        var abilityInstance = newAbility.AbilityControllerScene.Instantiate();
        _abilities.AddChild(abilityInstance);
    }

}
