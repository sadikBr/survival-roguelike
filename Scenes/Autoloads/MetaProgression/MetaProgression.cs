using Game.Resources.MetaUpgrades;

using Godot;

namespace Game.Autoloads;

public partial class MetaProgression : Node
{

    private const string SaveFilePath = "user://meta_progression.tres";
    public static MetaProgression Instance { get; private set; }

    public SaveData SaveData { get; private set; }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            Instance = this;
        }
    }

    public override void _Ready()
    {
        SaveData = LoadSaveData();
        GameEvents.Instance.ExperienceVialCollected += OnExperieceVialCollected;
    }

    public void AddMetaUpgrade(MetaUpgrade metaUpgrade)
    {
        if (!SaveData.CurrentMetaUpgrades.TryGetValue(metaUpgrade.Id, out var data))
        {
            SaveData.CurrentMetaUpgrades.Add(metaUpgrade.Id, new MetaUpgradeState
            {
                MetaUpgrade = metaUpgrade,
                Quantity = 1
            });
        }
        else
        {
            data.Quantity += 1;
            SaveData.CurrentMetaUpgrades[metaUpgrade.Id] = data;
        }
        SaveData.MetaUpgradeCurrency -= metaUpgrade.ExperienceCost;
        Save();
    }

    public void Save()
    {
        ResourceSaver.Save(SaveData, SaveFilePath);
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
        SaveData.MetaUpgradeCurrency += amount;
    }
}
