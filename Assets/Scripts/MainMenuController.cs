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
        Debug.Log("[Construct] Dependencies injected.");
    }

    void Awake()
    {
        // Ensure LevelProgressionService initializes CurrentLevel correctly
        if (_levelProgressionService == null)
        {
            Debug.LogError("[MainMenuController] LevelProgressionService is not assigned.");
            return;
        }

        int currentLevelIndex = _levelProgressionService.GetCurrentLevel();
        Debug.Log($"[MainMenuController.Awake] CurrentLevelIndex: {currentLevelIndex}");

        int activeRoomIndex = -1;

        for (int i = 0; i < minimumLevelForRoom.Count && i < rooms.Count; i++)
        {
            if (currentLevelIndex >= minimumLevelForRoom[i])
            {
                activeRoomIndex = i;
                Debug.Log($"[MainMenuController.Awake] Room {i} is eligible for activation.");
            }
            else
            {
                Debug.Log($"[MainMenuController.Awake] Room {i} is not eligible for activation.");
                break;
            }
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == activeRoomIndex)
            {
                rooms[i].roomObject.SetActive(true);
                Debug.Log($"[MainMenuController.Awake] Activating Room {i}.");
                HandleObjectsActivation(rooms[i].objectsToShow, currentLevelIndex);
            }
            else
            {
                rooms[i].roomObject.SetActive(false);
                Debug.Log($"[MainMenuController.Awake] Deactivating Room {i}.");
            }
        }

        if (_startGameButton != null)
        {
            _startGameButton.onClick.AddListener(OnStartGameClicked);
            Debug.Log("[MainMenuController.Awake] Start Game button listener added.");
        }
        else
        {
            Debug.LogWarning("[MainMenuController.Awake] Start Game button is not assigned.");
        }

        // Reset the justPassedLevel flag after handling activations
        _levelProgressionService.justPassedLevel = false;
        Debug.Log("[MainMenuController.Awake] justPassedLevel flag reset.");
    }

    private void HandleObjectsActivation(List<GameObject> objects, int currentLevelIndex)
    {
        // Ensure the threshold is not negative
        int activationThreshold = Mathf.Max(0, currentLevelIndex);
        Debug.Log($"[MainMenuController.HandleObjectsActivation] Activation Threshold: {activationThreshold}");

        for (int i = 0; i < objects.Count; i++)
        {
            if (i < activationThreshold)
            {
                if (_levelProgressionService.justPassedLevel && i == activationThreshold - 1)
                {
                    AnimateObject(objects[i]);
                    Debug.Log($"[MainMenuController.HandleObjectsActivation] Animating Object {i}.");
                }
                else
                {
                    objects[i].SetActive(true);
                    Debug.Log($"[MainMenuController.HandleObjectsActivation] Activating Object {i}.");
                }
            }
            else
            {
                objects[i].SetActive(false);
                Debug.Log($"[MainMenuController.HandleObjectsActivation] Deactivating Object {i}.");
            }
        }
    }

    private void AnimateObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        Debug.Log($"[MainMenuController.AnimateObject] Object {obj.name} animated.");
    }

    private void Start()
    {
        int currentLevelIndex = _levelProgressionService.GetCurrentLevel();
        textLevel.text = "Level " + (currentLevelIndex + 1).ToString(); // Display starts at 1
        Debug.Log($"[MainMenuController.Start] Displaying Level: {currentLevelIndex + 1}");
    }

    void OnDestroy()
    {
        if (_startGameButton != null)
        {
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
            Debug.Log("[MainMenuController.OnDestroy] Start Game button listener removed.");
        }
    }

    private void OnStartGameClicked()
    {
        Debug.Log("[MainMenuController.OnStartGameClicked] Start Game button clicked.");
        _levelProgressionService.LoadCurrentLevel();
        _stateMachine.ChangeState(GameState.Gameplay);
    }
}