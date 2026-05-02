using Game.Resources.MetaUpgrades;

using Godot;

namespace Game.Autoloads;

public partial class MetaProgression : Node
{

    private const string SaveFilePath = "user://meta_progression.tres";
    public static MetaProgression Instance { get; private set; }

    private SaveData _saveData;

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            Instance = this;
        }
    }

    public override void _Ready()
    {
        _saveData = LoadSaveData();
        GameEvents.Instance.ExperienceVialCollected += OnExperieceVialCollected;
    }

    public void AddMetaUpgrade(MetaUpgrade metaUpgrade)
    {
        if (!_saveData.CurrentMetaUpgrades.TryGetValue(metaUpgrade.Id, out var data))
        {
            _saveData.CurrentMetaUpgrades.Add(metaUpgrade.Id, new MetaUpgradeState
            {
                MetaUpgrade = metaUpgrade,
                Quantity = 1
            });
        }
        else
        {
            data.Quantity += 1;
            _saveData.CurrentMetaUpgrades[metaUpgrade.Id] = data;
        }
    }

    public void Save()
    {
        ResourceSaver.Save(_saveData, SaveFilePath);
    }

    private SaveData LoadSaveData()
    {
        if (!FileAccess.FileExists(SaveFilePath))
        {
            return new SaveData();
        }

        var saveData = ResourceLoader.Load<SaveData>(SaveFilePath);
        return saveData;
    }

    // private void PrintAsJson(SaveData input)
    // {
    //     var options = new JsonSerializerOptions { WriteIndented = true, IgnoreReadOnlyProperties = true };
    //     var json = JsonSerializer.Serialize(input, options);
    //     GD.Print(json);
    // }

    private void OnExperieceVialCollected(int amount)
    {
        _saveData.MetaUpgradeCurrency += amount;
    }
}
