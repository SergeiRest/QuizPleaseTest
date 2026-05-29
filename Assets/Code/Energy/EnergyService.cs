using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using QuizPlease.Core.Services;
using UniRx;
using UnityEngine;

namespace QuizPlease.Energy
{
    public class EnergyService : Service, IEnergyService
    {
        private readonly EnergySettings _settings;
        private readonly ReactiveProperty<int> _current;
        private readonly ReactiveProperty<float> _secondsToNext;

        private CancellationTokenSource _regenCancellation;
        private UniTask _regenTask;
        private UniTaskCompletionSource _belowMaxSignal;
        private float _elapsedSeconds;
        private bool _initialized;
        
        public IReadOnlyReactiveProperty<int> Current => _current;

        public IReadOnlyReactiveProperty<float> SecondsToNext => _secondsToNext;

        public EnergyService(EnergySettings settings)
        {
            _settings = settings;
            _current = new ReactiveProperty<int>(_settings.MaxEnergy);
            _secondsToNext = new ReactiveProperty<float>(0);
        }

        public override UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            if (_initialized)
            {
                return UniTask.CompletedTask;
            }

            _initialized = true;
            _regenCancellation = new CancellationTokenSource();
            _regenTask = RunRegenerationAsync(_regenCancellation.Token);
            return UniTask.CompletedTask;
        }

        public override async UniTask ReleaseAsync(CancellationToken cancellationToken)
        {
            if (!_initialized)
            {
                return;
            }

            _initialized = false;

            _regenCancellation.Cancel();
            _belowMaxSignal?.TrySetCanceled(_regenCancellation.Token);

            await _regenTask
                .AttachExternalCancellation(cancellationToken)
                .SuppressCancellationThrow();

            _regenCancellation.Dispose();
            _regenCancellation = null;
            _belowMaxSignal = null;
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0 || _current.Value < amount)
            {
                return false;
            }

            var wasFull = _current.Value >= _settings.MaxEnergy;
            _current.Value -= amount;

            if (wasFull)
            {
                _elapsedSeconds = 0f;
                _secondsToNext.Value = 0f;
            }

            _belowMaxSignal?.TrySetResult();
            return true;
        }

        private async UniTask RunRegenerationAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_current.Value >= _settings.MaxEnergy)
                {
                    _current.Value = _settings.MaxEnergy;
                    _secondsToNext.Value = 1f;
                    await WaitUntilBelowMaxAsync(cancellationToken);
                    continue;
                }

                while (_elapsedSeconds < _settings.RegenSeconds && !cancellationToken.IsCancellationRequested)
                {
                    _elapsedSeconds += Time.deltaTime;
                    _secondsToNext.Value = _elapsedSeconds / _settings.RegenSeconds;
                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                }
                
                _current.Value++;
                _current.Value= Mathf.Clamp(_current.Value, 0, _settings.MaxEnergy);
                _elapsedSeconds = 0;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        private async UniTask WaitUntilBelowMaxAsync(CancellationToken cancellationToken)
        {
            if (_current.Value < _settings.MaxEnergy)
            {
                return;
            }

            _belowMaxSignal = new UniTaskCompletionSource();

            try
            {
                await _belowMaxSignal.Task.AttachExternalCancellation(cancellationToken);
            }
            finally
            {
                _belowMaxSignal = null;
            }
        }
    }
}
