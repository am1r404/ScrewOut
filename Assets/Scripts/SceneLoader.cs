// SceneLoader.cs
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void LoadSceneAsync(string sceneName, Action onLoaded = null)
    {
        SceneManager.LoadSceneAsync(sceneName).completed += (asyncOperation) =>
        {
            onLoaded?.Invoke();
        };
    }
}