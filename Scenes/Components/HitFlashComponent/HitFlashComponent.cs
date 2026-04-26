using Godot;

namespace Game.Components;

public partial class HitFlashComponent : Node
{

    [Export] private HealthComponent _healthComponent;
    [Export] private Sprite2D _sprite;
    [Export] private ShaderMaterial _hitFlashMaterial;

    private Tween _tween;

    public override void _Ready()
    {
        _healthComponent.HealthChanged += OnHealthChanged;
        _sprite.Material = _hitFlashMaterial;
    }

    private async void OnHealthChanged(bool isDamage)
    {
        if (isDamage)
        {
            if (_tween != null && _tween.IsValid())
            {
                _tween.Kill();
            }

            ((ShaderMaterial)_sprite.Material).SetShaderParameter("percent", 1.0f);
            _tween = CreateTween();
            _tween.TweenProperty(_sprite.Material, "shader_parameter/percent", 0.0f, 0.25f)
                .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
        }
    }
}
