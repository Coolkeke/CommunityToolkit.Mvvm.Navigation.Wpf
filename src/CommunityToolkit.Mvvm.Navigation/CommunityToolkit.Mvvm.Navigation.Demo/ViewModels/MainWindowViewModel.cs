using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Navigation.Wpf;
namespace CommunityToolkit.Mvvm.Navigation.Demo.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        INavigationService Navigation;
        public MainWindowViewModel(INavigationService navigation)
        {
            Navigation = navigation;
        }
        private RelayCommand<string> _GoPageCommand;
        public RelayCommand<string> GoPageCommand =>
            _GoPageCommand ?? (_GoPageCommand = new RelayCommand<string>(ExecuteGoPageCommand));

        void ExecuteGoPageCommand(string view)
        {
            var Parameters = new NavigationParameters
            {
                { "Value", view }
            };
            Navigation.Navigate("Main", view, Parameters);
        }
    }
}
