using Dogs.Infrastructure;
using Dogs.Model;
using Dogs.Presentation;
using Zenject;

namespace Dogs
{
    public class DogsInstaller : Installer<BreedButton, DogsInstaller>
    {
        private readonly BreedButton _breedButtonPrefab;

        public DogsInstaller(BreedButton breedButtonPrefab)
        {
            _breedButtonPrefab = breedButtonPrefab;
        }

        public override void InstallBindings()
        {
            Container.BindMemoryPool<BreedButton, BreedButton.Pool>()
                .WithInitialSize(10)
                .ExpandByOneAtATime()
                .FromComponentInNewPrefab(_breedButtonPrefab);
                
            Container.Bind<DogsModel>().AsSingle();
            Container.Bind<DogsTabPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BreedsRequester>().AsSingle().NonLazy();
        }
    }
}