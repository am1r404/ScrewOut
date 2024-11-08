using UnityEngine;
using UnityEngine.Events;

public class LevelProgressionService
{
    private const string PlayerPrefsCurrentLevelKey = "CurrentLevelIndex";
    private readonly LevelLoader _levelLoader;
    public UnityEvent OnLevelAdvanced { get; } = new();
    public int CurrentLevel { get; private set; }
    public bool justPassedLevel;

    public LevelProgressionService(LevelLoader levelLoader)
    {
        _levelLoader = levelLoader;
        CurrentLevel = PlayerPrefs.GetInt(PlayerPrefsCurrentLevelKey, 0);
        Debug.Log($"[LevelProgressionService] Initialized with CurrentLevel: {CurrentLevel}");
    }

    public void LoadCurrentLevel()
    {
        Debug.Log($"[LevelProgressionService] Loading Level {CurrentLevel}");
        _levelLoader.LoadLevel(CurrentLevel);
    }

    public void AdvanceToNextLevel()
    {
        justPassedLevel = true;
        int nextLevel = CurrentLevel + 1;
        Debug.Log($"[LevelProgressionService] Advancing to Level {nextLevel}");

        if (nextLevel < _levelLoader.levelPrefabs.Count)
        {
            CurrentLevel = nextLevel;
            SaveProgress();
            Debug.Log($"[LevelProgressionService] CurrentLevel updated to {CurrentLevel}");
            OnLevelAdvanced.Invoke();
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    public void ResetProgress()
    {
        CurrentLevel = 0;
        justPassedLevel = false;
        SaveProgress();
        _levelLoader.LoadLevel(CurrentLevel);
        Debug.Log("Progress has been reset to Level 0.");
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PlayerPrefsCurrentLevelKey, CurrentLevel);
        PlayerPrefs.Save();
        Debug.Log($"[LevelProgressionService] Progress saved: CurrentLevelIndex = {CurrentLevel}");
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }
} 