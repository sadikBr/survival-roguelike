using Game.GameObjects.Entities;
using Game.UI;

using Godot;

namespace Game;

public partial class Main : Node
{

    [Export] private PackedScene _endScreenScene;

    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("%Player");
        _player.GetHealthComponent().Died += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        var endScreenInstance = _endScreenScene.Instantiate<EndScreen>();
        AddChild(endScreenInstance);
        endScreenInstance.SetDefeat();
    }
}
