using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
{
    public GameObject gameManagerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<SceneLoader>().AsSingle().NonLazy();
        Container.Bind<GameStateMachine>().AsSingle().NonLazy();
        Container.Bind<LevelLoader>().AsSingle().NonLazy();
        Container.Bind<ILevelProgressionService>()
            .To<LevelProgressionService>()
            .AsSingle();
        Container.Bind<BootstrapState>().AsTransient();
        Container.Bind<MainMenuState>().AsTransient();
        Container.Bind<GameplayState>().AsTransient();
    }
}