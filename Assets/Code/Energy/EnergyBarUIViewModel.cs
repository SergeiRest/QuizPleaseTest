using UniRx;

namespace QuizPlease.Energy
{
    public class EnergyBarUIViewModel : IEnergyBarUIViewModel
    {
        private const int SpendAmount = 10;

        private readonly IEnergyService _energyService;
        private readonly EnergySettings _settings;
        public IReadOnlyReactiveProperty<int> Current => _energyService.Current;

        public IReadOnlyReactiveProperty<float> SecondsToNext => _energyService.SecondsToNext;

        public int MaxEnergy => _settings.MaxEnergy;

        public EnergyBarUIViewModel(IEnergyService energyService, EnergySettings settings)
        {
            _energyService = energyService;
            _settings = settings;
        }
        

        public void SpendEnergy()
        {
            _energyService.TrySpend(SpendAmount);
        }
    }
}
