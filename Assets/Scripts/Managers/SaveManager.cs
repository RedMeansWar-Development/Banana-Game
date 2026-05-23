using System.Collections.Generic;
using BananaGame.BananaTime;
using UnityEngine;

namespace BananaGame.Managers;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SaveKey = "BananaTimeTravelSave";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    private class SaveData
    {
        public List<WorldStateSnapshot> snapshots = new();
        public string currentEraName;
    }

    public void Save(IEnumerable<WorldStateSnapshot> snapshots, string currentEraName)
    {
        var data = new SaveData
        {
            snapshots = new List<WorldStateSnapshot>(snapshots),
            currentEraName = currentEraName
        };
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
        Debug.Log("SaveManager: game saved.");
    }

    public (List<WorldStateSnapshot> snapshots, string eraName) Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return (new List<WorldStateSnapshot>(), string.Empty);

        var data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SaveKey));
        return (data.snapshots, data.currentEraName);
    }

    public void DeleteSave() => PlayerPrefs.DeleteKey(SaveKey);
}
