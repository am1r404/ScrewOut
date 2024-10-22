using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private Button _returnToMenuButton;

    private GameStateMachine _stateMachine;

    [Inject]
    public void Construct(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    void Awake()
    {
        if (_returnToMenuButton != null)
            _returnToMenuButton.onClick.AddListener(OnReturnToMenuClicked);
    }

    void OnDestroy()
    {
        if (_returnToMenuButton != null)
            _returnToMenuButton.onClick.RemoveListener(OnReturnToMenuClicked);
    }

    private void OnReturnToMenuClicked()
    {
        _stateMachine.ChangeState(GameState.MainMenu);
    }
}