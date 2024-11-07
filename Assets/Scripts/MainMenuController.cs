using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public GameObject roomObject;
        public List<GameObject> objectsToShow;
    }

    [SerializeField] private Button _startGameButton;
    private LevelProgressionService _levelProgressionService;

    private GameStateMachine _stateMachine;
    [SerializeField] private TextMeshProUGUI textLevel;

    [SerializeField] private List<Room> rooms;
    [SerializeField] private List<int> minimumLevelForRoom = new List<int> { 1, 10, 20, 28, 36 };

    [Inject]
    public void Construct(GameStateMachine stateMachine, LevelProgressionService levelProgressionService)
    {
        _stateMachine = stateMachine;
        _levelProgressionService = levelProgressionService;
    }

    void Awake()
    {
        if (!PlayerPrefs.HasKey("CurrentLevelIndex"))
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", 0);
            PlayerPrefs.Save();
        }

        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);

        int activeRoomIndex = -1;

        for (int i = 0; i < minimumLevelForRoom.Count && i < rooms.Count; i++)
        {
            if (currentLevelIndex >= minimumLevelForRoom[i])
            {
                activeRoomIndex = i;
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == activeRoomIndex)
            {
                rooms[i].roomObject.SetActive(true);

                HandleObjectsActivation(rooms[i].objectsToShow, currentLevelIndex);
            }
            else
            {
                rooms[i].roomObject.SetActive(false);
            }
        }

        if (_startGameButton != null)
            _startGameButton.onClick.AddListener(OnStartGameClicked);
    }

    private void HandleObjectsActivation(List<GameObject> objects, int currentLevelIndex)
    {
        // Ensure the threshold is not negative
        int activationThreshold = Mathf.Max(0, currentLevelIndex);

        for (int i = 0; i < objects.Count; i++)
        {
            if (i < activationThreshold)
            {
                if (_levelProgressionService.justPassedLevel && i == activationThreshold - 1)
                {
                    AnimateObject(objects[i]);
                }
                else
                {
                    objects[i].SetActive(true);
                }
            }
            else
            {
                objects[i].SetActive(false);
            }
        }
    }

    private void AnimateObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
    }

    private void Start()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        textLevel.text = "Level " + (currentLevelIndex + 1).ToString();
    }

    void OnDestroy()
    {
        if (_startGameButton != null)
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
    }

    private void OnStartGameClicked()
    {
        _levelProgressionService.LoadCurrentLevel();
        _stateMachine.ChangeState(GameState.Gameplay);
    }
}