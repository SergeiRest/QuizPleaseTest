using QuizPlease.Core.Services;
using QuizPlease.Core.UI;
using QuizPlease.Energy;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QuizPlease.App
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private EnergySettings _energySettings = default;

        [SerializeField]
        private EnergyBarUIView _energyBarView = default;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_energySettings);
            builder.Register<EnergyService>(Lifetime.Singleton).As<IEnergyService, IService>();
            builder.Register<EnergyBarUIViewModel>(Lifetime.Singleton).As<IEnergyBarUIViewModel>();
            builder.RegisterComponent(_energyBarView).As<IUIView>();
            builder.RegisterEntryPoint<AppLifecycle>(Lifetime.Singleton);
        }
    }
}
