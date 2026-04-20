using Godot;

namespace Game.Components;

public partial class HealthComponent : Node
{

    [Signal] public delegate void DiedEventHandler();

    [Export] private float _maxHealth = 10.0f;
    private float _currentHealth;

    public override void _Ready()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(float amount)
    {
        UpdateHealth(amount);
    }

    public void Heal(float amount)
    {
        UpdateHealth(-amount);
    }

    private void UpdateHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
        Callable.From(CheckDeath).CallDeferred();
    }

    private void CheckDeath()
    {
        if (_currentHealth <= 0)
        {
            EmitSignal(SignalName.Died);
            Owner.QueueFree();
        }
    }

}
