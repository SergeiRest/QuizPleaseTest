using UnityEngine;

namespace QuizPlease.Core.UI
{
    public abstract class UIView : MonoBehaviour, IUIView
    {
        public abstract void Initialize();

        public abstract void Release();

        protected virtual void OnDestroy()
        {
            Release();
        }
    }
}
