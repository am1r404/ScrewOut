using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;

    private GameStateMachine _stateMachine;

    [Inject]
    public void Construct(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    void Awake()
    {
        if (_startGameButton != null)
            _startGameButton.onClick.AddListener(OnStartGameClicked);
    }

    void OnDestroy()
    {
        if (_startGameButton != null)
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
    }

    private void OnStartGameClicked()
    {
        _stateMachine.ChangeState(GameState.Gameplay);
    }
}