using Godot;

namespace Game.Components;

public partial class HealthComponent : Node
{

    [Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void HealthChangedEventHandler(bool isDamage = true);

    [Export] private float _maxHealth = 10.0f;
    private float _currentHealth;

    public override void _Ready()
    {
        _currentHealth = _maxHealth;
    }

    public float GetHealthPercentage()
    {
        return _maxHealth <= 0 ? 0 : Mathf.Min(_currentHealth / _maxHealth, 1.0f);
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
        EmitSignal(SignalName.HealthChanged, Mathf.Sign(amount) > 0);
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
