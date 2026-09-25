using ReactiveUI;

namespace Test.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content;
        public ViewModelBase Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public MainWindowViewModel()
        {
            Content = new ProductListPageViewModel(this);
        }

        public void Navigate(ViewModelBase vm) => Content = vm;
    }
}
