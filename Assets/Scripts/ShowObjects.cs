using UnityEngine;
using Zenject;
using DG.Tweening;

public class ShowObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [Inject]
    LevelProgressionService levelProgressionService;

    void Start()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        if (currentLevelIndex == 0) return;
        if (levelProgressionService.justPassedLevel)
        {
            ShowWithAnimation(currentLevelIndex -1);
        }
        else
        {
            objects[currentLevelIndex - 1].SetActive(true);
        }
        for (int i = 0; i < currentLevelIndex; i++)
        {
            objects[i].SetActive(true);
        }
    }

    private void ShowWithAnimation(int currentLevelIndex)
    {
        var currentObject = objects[currentLevelIndex];
        currentObject.SetActive(true);
        currentObject.transform.localScale = Vector3.zero;
        currentObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
    } 
}
