using Godot;

namespace Game.Autoloads;

public partial class ScreenTransition : CanvasLayer
{
    [Signal] public delegate void TransitionedHalfwayEventHandler();

    private static readonly StringName DefaultAnimation = new("default");

    public static ScreenTransition Instance { get; private set; }

    private AnimationPlayer _animationPlayer;

    private bool _skipEmit = false;

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            Instance = this;
        }
    }

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public async void Transition()
    {
        _animationPlayer.Play(DefaultAnimation);
        await ToSignal(this, SignalName.TransitionedHalfway);
        _skipEmit = true;
        _animationPlayer.PlayBackwards(DefaultAnimation);
    }

    public void EmitTransitionedHalfway()
    {
        if (_skipEmit)
        {
            _skipEmit = false;
            return;
        }
        EmitSignal(SignalName.TransitionedHalfway);
    }

}
