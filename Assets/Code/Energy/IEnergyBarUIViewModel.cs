using QuizPlease.Core.UI;
using UniRx;

namespace QuizPlease.Energy
{
    public interface IEnergyBarUIViewModel : IUIViewModel
    {
        IReadOnlyReactiveProperty<int> Current { get; }

        IReadOnlyReactiveProperty<float> SecondsToNext { get; }

        int MaxEnergy { get; }

        void SpendEnergy();
    }
}
