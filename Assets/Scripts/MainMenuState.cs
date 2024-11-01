// MainMenuState.cs
using UnityEngine;
using Zenject;

public class MainMenuState : IGameState
{
    private readonly GameStateMachine _stateMachine;

    [Inject]
    public MainMenuState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Entering MainMenu State");

        // Initialize Main Menu UI elements if necessary
    }

    public void Exit()
    {
        Debug.Log("Exiting MainMenu State");
    }
}