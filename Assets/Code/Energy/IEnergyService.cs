using QuizPlease.Core.Services;
using UniRx;

namespace QuizPlease.Energy
{
    public interface IEnergyService
    {
        IReadOnlyReactiveProperty<int> Current { get; }

        IReadOnlyReactiveProperty<float> SecondsToNext { get; }

        bool TrySpend(int amount);
    }
}
