// GameplayState.cs
using UnityEngine;
using Zenject;

public class GameplayState : IGameState
{
    private readonly GameStateMachine _stateMachine;

    [Inject]
    public GameplayState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering Gameplay State");

        // Initialize Gameplay elements here
    }

    public void Exit()
    {
        Debug.Log("Exiting Gameplay State");
    }

    public void ReturnToMainMenu()
    {
        _stateMachine.ChangeState(GameState.MainMenu);
    }
}