using System;
using System.Collections.Generic;
using QuizPlease.Core.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace QuizPlease.Energy
{
    public sealed class EnergyBarUIView : UIView<IEnergyBarUIViewModel>
    {
        [SerializeField] private TextMeshProUGUI _energyText = default;

        [SerializeField] private Image _progressImage = default;

        [SerializeField] private Button _spendButton = default;
        
        private bool _initialized;
        private CompositeDisposable _disposables;

        [Inject]
        public void Construct(IEnergyBarUIViewModel viewModel)
        {
            SetViewModel(viewModel);
        }

        public override void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _disposables = new CompositeDisposable();
            _initialized = true;
            _spendButton.OnClickAsObservable().Subscribe(_ => OnSpendClicked()).AddTo(_disposables);
            ViewModel.Current.Subscribe(UpdateEnergyText).AddTo(_disposables);
            ViewModel.SecondsToNext.Subscribe(UpdateProgress).AddTo(_disposables);
        }

        public override void Release()
        {
            if (!_initialized)
            {
                return;
            }

            _initialized = false;
            _disposables?.Dispose();
        }

        private void OnSpendClicked()
        {
            ViewModel.SpendEnergy();
        }

        private void UpdateEnergyText(int current)
        {
            _energyText.text = $"{current} / {ViewModel.MaxEnergy}";
        }

        private void UpdateProgress(float progress)
        {
            _progressImage.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
