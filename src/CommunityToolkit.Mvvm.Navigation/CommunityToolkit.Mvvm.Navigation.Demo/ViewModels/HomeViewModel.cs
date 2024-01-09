using CommunityToolkit.Mvvm.ComponentModel; 
using CommunityToolkit.Mvvm.Navigation.Wpf;
using System.Windows;

namespace CommunityToolkit.Mvvm.Navigation.Demo.ViewModels
{
    public class HomeViewModel : ObservableObject, INavigationAware, IJournalAware
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        public bool IsNavigationTarget(NavigationParameters parameters) => true;

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
           
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Title = parameters.GetValue<string>("Value");
        }
        public bool PersistInHistory() => true;
    }
}
