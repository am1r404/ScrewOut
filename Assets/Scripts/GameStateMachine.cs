// GameStateMachine.cs
using System;
using Zenject;

public class GameStateMachine
{
    private IGameState _currentState;
    private readonly DiContainer _container;

    public GameStateMachine(DiContainer container)
    {
        _container = container;
    }

    public void ChangeState(GameState newState)
    {
        _currentState?.Exit();

        switch (newState)
        {
            case GameState.Bootstrap:
                _currentState = _container.Resolve<BootstrapState>();
                break;
            case GameState.MainMenu:
                _currentState = _container.Resolve<MainMenuState>();
                break;
            case GameState.Gameplay:
                _currentState = _container.Resolve<GameplayState>();
                break;
            default:
                throw new ArgumentException($"Unknown state: {newState}");
        }

        _currentState.Enter();
    }
}