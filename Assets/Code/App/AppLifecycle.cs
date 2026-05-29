using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using QuizPlease.Core.Services;
using QuizPlease.Core.UI;
using UnityEngine;
using VContainer.Unity;

namespace QuizPlease.App
{
    public sealed class AppLifecycle : IStartable, IDisposable
    {
        private readonly IReadOnlyList<IService> _services;
        private readonly IReadOnlyList<IUIView> _views;
        private readonly CancellationTokenSource _lifetimeCancellation = new CancellationTokenSource();

        private bool _started;
        private bool _disposed;

        public AppLifecycle(IReadOnlyList<IService> services, IReadOnlyList<IUIView> views)
        {
            _services = services;
            _views = views;
        }

        public void Start()
        {
            if (_started)
            {
                return;
            }

            _started = true;
            InitializeAsync(_lifetimeCancellation.Token).Forget(Debug.LogException);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _lifetimeCancellation.Cancel();
            ReleaseAsync(CancellationToken.None).Forget(Debug.LogException);
        }

        private async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            for (var i = 0; i < _services.Count; i++)
            {
                await _services[i].InitializeAsync(cancellationToken);
            }

            for (var i = 0; i < _views.Count; i++)
            {
                _views[i].Initialize();
            }
        }

        private async UniTask ReleaseAsync(CancellationToken cancellationToken)
        {
            for (var i = _views.Count - 1; i >= 0; i--)
            {
                _views[i].Release();
            }

            for (var i = _services.Count - 1; i >= 0; i--)
            {
                await _services[i].ReleaseAsync(cancellationToken);
            }

            _lifetimeCancellation.Dispose();
        }
    }
}
