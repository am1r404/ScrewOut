using UnityEngine;
using Zenject;

public class GameplayState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private SceneLoader _sceneLoader;
    private LevelLoader _levelLoader;

    [Inject]
    public GameplayState(GameStateMachine stateMachine, SceneLoader sceneLoader, LevelLoader levelLoader)
    {
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        _levelLoader = levelLoader;
    }

    public void Enter()
    {
        Debug.Log("Entering Gameplay State");
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