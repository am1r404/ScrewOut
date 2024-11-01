using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using UnityEngine.Events;

public class LevelLoader
{
    private const string PlayerPrefsCurrentLevelKey = "CurrentLevelIndex";

    public List<GameObject> levelPrefabs;
    [Inject]
    private ZenjectSceneLoader _sceneLoader;

    [Inject]
    private DiContainer _container;

    private GameObject currentLevelInstance;
    private int currentLevelIndex = 0;
    public UnityAction OnLevelLoaded;

    [Inject]
    public void Initialize()
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

    public void LoadLevel(int levelIndex)
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
        }

        currentLevelInstance = _container.InstantiatePrefab(levelPrefabs[levelIndex]);

        currentLevelIndex = levelIndex;

        PlayerPrefs.SetInt(PlayerPrefsCurrentLevelKey, currentLevelIndex);
        PlayerPrefs.Save();

        OnLevelLoaded?.Invoke();
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex < levelPrefabs.Count)
        {
            LoadLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }
}