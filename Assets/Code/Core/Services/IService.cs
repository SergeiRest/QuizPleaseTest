using System.Threading;
using Cysharp.Threading.Tasks;

namespace QuizPlease.Core.Services
{
    public interface IService
    {
        UniTask InitializeAsync(CancellationToken cancellationToken);

        UniTask ReleaseAsync(CancellationToken cancellationToken);
    }
}
