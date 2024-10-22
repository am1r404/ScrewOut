using UnityEngine;
using Zenject;

public class BootstrapController : MonoBehaviour
{
    private GameStateMachine _stateMachine;

    [Inject]
    public void Construct(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    void Start()
    {
        _stateMachine.ChangeState(GameState.Bootstrap);
    }
}