using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<ScrewPanel>().FromComponentsInHierarchy().AsTransient();
        Container.BindInterfacesAndSelfTo<Plank>().FromComponentsInHierarchy().AsTransient();
        Container.BindInterfacesAndSelfTo<Screw>().FromComponentsInHierarchy().AsTransient();
    }
}