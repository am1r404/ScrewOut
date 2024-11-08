using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class LevelLoader
{
    private const string PlayerPrefsCurrentLevelKey = "CurrentLevelIndex";

    public List<GameObject> levelPrefabs;
    private readonly DiContainer _container;
    private readonly SceneLoader _sceneLoader;

    private GameObject currentLevelInstance;
    private int currentLevelIndex = 0;
    public UnityAction OnLevelLoaded;

    public LevelLoader(DiContainer container, SceneLoader sceneLoader)
    {
        _container = container;
        _sceneLoader = sceneLoader;
    }

    [Inject]
    public async UniTask Initialize()
    {
        LoadLevelPrefabsFromResources();

        currentLevelIndex = PlayerPrefs.GetInt(PlayerPrefsCurrentLevelKey, 0);
        Debug.Log($"Initializing LevelLoader with Level Index: {currentLevelIndex}");
    }

    public GameObject GetCurrentLevelInstance()
    {
        return currentLevelInstance;
    }

    private void LoadLevelPrefabsFromResources()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Levels");
        // Sort prefabs by natural number order
        Array.Sort(prefabs, (a, b) => {
            int aNum = int.Parse(a.name.Replace("Level", ""));
            int bNum = int.Parse(b.name.Replace("Level", ""));
            return aNum.CompareTo(bNum);
        });
        levelPrefabs = new List<GameObject>(prefabs);

        if (levelPrefabs.Count == 0)
        {
            Debug.LogError("No level prefabs found in Resources/Levels.");
        }
        else
        {
            Debug.Log($"Loaded {levelPrefabs.Count} level prefabs from Resources/Levels.");
        }
    }

    public async UniTask LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelPrefabs.Count)
        {
            Debug.LogError("Level index out of range.");
            return;
        }

        if (currentLevelInstance != null)
        {
            GameObject.Destroy(currentLevelInstance);
            currentLevelInstance = null;
            Debug.Log($"[LevelLoader.LoadLevel] Destroyed previous level instance.");
        }

        await _sceneLoader.LoadSceneAsync("Game");

        currentLevelInstance = _container.InstantiatePrefab(levelPrefabs[levelIndex]);
        currentLevelIndex = levelIndex;

        Debug.Log($"[LevelLoader.LoadLevel] Loaded Level {currentLevelIndex}: {levelPrefabs[levelIndex].name}");

        OnLevelLoaded?.Invoke();
    }

    public void ClearLevel()
    {
        if (currentLevelInstance != null)
        {
            GameObject.Destroy(currentLevelInstance);
            currentLevelInstance = null;
            Debug.Log($"[LevelLoader.ClearLevel] Cleared current level instance.");
        }
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(currentLevelIndex).Forget();
        Debug.Log($"[LevelLoader.LoadCurrentLevel] Loading Current Level {currentLevelIndex}");
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex < levelPrefabs.Count)
        {
            LoadLevel(nextLevelIndex).Forget();
            Debug.Log($"[LevelLoader.LoadNextLevel] Loading Next Level {nextLevelIndex}");
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }
}