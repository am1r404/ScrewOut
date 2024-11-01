using UnityEngine;

public class ShowObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    void Start()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);

        for (int i = 0; i < currentLevelIndex; i++)
        {
            objects[i].SetActive(true);
        }
    }
}
