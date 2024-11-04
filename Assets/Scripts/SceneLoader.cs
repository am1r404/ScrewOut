// SceneLoader.cs
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public async UniTask LoadSceneAsync(string sceneName, Action onLoaded = null)
    {
        await SceneManager.LoadSceneAsync(sceneName);
        onLoaded?.Invoke();
    }
}