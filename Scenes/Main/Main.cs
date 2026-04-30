using Game.GameObjects.Entities;
using Game.UI;

using Godot;

namespace Game;

public partial class Main : Node
{

    private static readonly StringName ActionPause = new("pause");

    [Export] private PackedScene _endScreenScene;
    [Export] private PackedScene _pauseScreenScene;

    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("%Player");
        _player.GetHealthComponent().Died += OnPlayerDied;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed(ActionPause))
        {
            var instance = _pauseScreenScene.Instantiate<PauseMenu>();
            AddChild(instance);
            GetTree().Root.SetInputAsHandled();
        }
    }

    private void OnPlayerDied()
    {
        var endScreenInstance = _endScreenScene.Instantiate<EndScreen>();
        AddChild(endScreenInstance);
        endScreenInstance.SetDefeat();
    }
}
