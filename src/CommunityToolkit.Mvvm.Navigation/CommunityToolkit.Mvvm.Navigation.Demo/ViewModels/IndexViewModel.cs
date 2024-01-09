using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Navigation.Wpf;
using System.Windows;

namespace CommunityToolkit.Mvvm.Navigation.Demo.ViewModels
{
    public class IndexViewModel : ObservableObject, INavigationAware, IJournalAware
    {
        INavigationService Navigation;
        public IndexViewModel(INavigationService navigation)
        {
            Navigation = navigation;
        }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        public bool IsNavigationTarget(NavigationParameters parameters)
        {
            var result = MessageBox.Show("你是否离开当前页面？", "警告", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) return true;
            else return false;
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Title = parameters.GetValue<string>("Value");
        }
        public bool PersistInHistory() => false;
        private RelayCommand _GoBackCommand;
        public RelayCommand GoBackCommand =>
            _GoBackCommand ?? (_GoBackCommand = new RelayCommand(ExecuteGoBackCommand));

        void ExecuteGoBackCommand()
        {
            Navigation.GoBack("Main");
        }
    }
}
