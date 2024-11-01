using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MainMenuState>().AsSingle();
        Container.Bind<MainMenuController>().FromComponentInHierarchy().AsSingle();
    }
}