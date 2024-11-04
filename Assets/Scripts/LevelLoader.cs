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
        }

        await _sceneLoader.LoadSceneAsync("Game");

        currentLevelInstance = _container.InstantiatePrefab(levelPrefabs[levelIndex]);
        currentLevelIndex = levelIndex;

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