using CommunityToolkit.Mvvm.Navigation.Demo.ViewModels;
using CommunityToolkit.Mvvm.Navigation.Demo.Views;
using CommunityToolkit.Mvvm.Navigation.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System; 
using System.Windows;

namespace CommunityToolkit.Mvvm.Navigation.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Service { get; private set; }
        public new static App Current => (App)Application.Current;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Service = ConfigureServices();
            NavigationService(Service?.GetService<INavigationService>());
            new MainWindow().Show();
        }
        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<INavigationService, NavigationService>();
            services.TryAddTransient<MainWindowViewModel>();
            services.TryAddTransient<HomeViewModel>();
            services.TryAddTransient<IndexViewModel>();
            return services.BuildServiceProvider();
        }

        private void NavigationService(INavigationService service)
        {
            if (service == null) return;
            service.RegisterForNavigation<Views.Home>(nameof(Views.Home));
            service.RegisterForNavigation<Views.Index>(nameof(Views.Index));
        }
    }
}
