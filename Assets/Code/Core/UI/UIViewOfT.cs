namespace QuizPlease.Core.UI
{
    public abstract class UIView<TVm> : UIView where TVm : IUIViewModel
    {
        protected TVm ViewModel { get; private set; }

        protected void SetViewModel(TVm viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
