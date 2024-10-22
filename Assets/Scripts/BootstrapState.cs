// BootstrapState.cs
using UnityEngine;
using Zenject;

public class BootstrapState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    [Inject]
    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
        Debug.Log("Entering Bootstrap State");

        // Initialize necessary services here
        // For example: Analytics, Ads, etc.

        // After initialization, load the Main Menu
        _sceneLoader.LoadSceneAsync("MainMenu", () =>
        {
            _stateMachine.ChangeState(GameState.MainMenu);
        });
    }

    public void Exit()
    {
        Debug.Log("Exiting Bootstrap State");
    }
}