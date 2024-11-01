using UnityEngine;
using UnityEngine.Events;

public interface ILevelProgressionService
{
    int CurrentLevel { get; }
    UnityEvent OnLevelAdvanced { get; }
    void LoadCurrentLevel();
    void AdvanceToNextLevel();
    void ResetProgress();
}

public class LevelProgressionService : ILevelProgressionService
{
    private const string PlayerPrefsCurrentLevelKey = "CurrentLevelIndex";
    private readonly LevelLoader _levelLoader;

    public UnityEvent OnLevelAdvanced { get; } = new UnityEvent();

    public int CurrentLevel { get; private set; }

    public LevelProgressionService(LevelLoader levelLoader)
    {
        _levelLoader = levelLoader;
        CurrentLevel = PlayerPrefs.GetInt(PlayerPrefsCurrentLevelKey, 0);
    }

    public void LoadCurrentLevel()
    {
        _levelLoader.LoadLevel(CurrentLevel);
    }

    public void AdvanceToNextLevel()
    {
        int nextLevel = CurrentLevel + 1;
        if (nextLevel < _levelLoader.levelPrefabs.Count)
        {
            CurrentLevel = nextLevel;
            SaveProgress();
            _levelLoader.LoadLevel(CurrentLevel);
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
        SaveProgress();
        _levelLoader.LoadLevel(CurrentLevel);
        Debug.Log("Progress has been reset to Level 0.");
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(PlayerPrefsCurrentLevelKey, CurrentLevel);
        PlayerPrefs.Save();
    }
} 