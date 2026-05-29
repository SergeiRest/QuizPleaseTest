using System.Threading;
using Cysharp.Threading.Tasks;

namespace QuizPlease.Core.Services
{
    public abstract class Service : IService
    {
        public abstract UniTask InitializeAsync(CancellationToken cancellationToken);

        public abstract UniTask ReleaseAsync(CancellationToken cancellationToken);
    }
}
