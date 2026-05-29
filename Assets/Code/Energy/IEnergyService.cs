using QuizPlease.Core.Services;
using UniRx;

namespace QuizPlease.Energy
{
    public interface IEnergyService : IService
    {
        IReadOnlyReactiveProperty<int> Current { get; }

        IReadOnlyReactiveProperty<float> SecondsToNext { get; }

        bool TrySpend(int amount);
    }
}
