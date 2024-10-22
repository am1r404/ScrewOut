// BootstrapInstaller.cs
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Bind SceneLoader as a singleton
        Container.Bind<SceneLoader>().AsSingle();

        // Bind GameStateMachine as a singleton
        Container.Bind<GameStateMachine>().AsSingle();

        // Bind States
        Container.Bind<BootstrapState>().AsTransient();
        Container.Bind<MainMenuState>().AsTransient();
        Container.Bind<GameplayState>().AsTransient();
    }
}