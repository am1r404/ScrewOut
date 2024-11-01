using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    private ILevelProgressionService _levelProgressionService;

    private GameStateMachine _stateMachine;
    [SerializeField] private TextMeshProUGUI textLevel;

    [Inject]
    public void Construct(GameStateMachine stateMachine, ILevelProgressionService levelProgressionService)
    {
        _stateMachine = stateMachine;
        _levelProgressionService = levelProgressionService;
    }

    void Awake()
    {
        if (_startGameButton != null)
            _startGameButton.onClick.AddListener(OnStartGameClicked);
    }

    private void Start()
    {
        textLevel.text = "Level " + (PlayerPrefs.GetInt("CurrentLevelIndex", 0) + 1);
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