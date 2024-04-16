using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using AutoMapper;
using TCMS.Data.Mappings;
using TCMS.GUI.Services.Implementations;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.GUI.ViewModels;
using TCMS.GUI.Views;

namespace TCMS.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Provides application-wide services and configuration, especially for dependency injection.
    /// </summary>
    public partial class App : Application
    {
        // ServiceProvider to hold and resolve service instances.
        public static ServiceProvider ServiceProvider { get; private set; }
        private static IMapper _mapper;

        // Constructor for the App class.
        public App()
        {
            // Create a new ServiceCollection to register application services.
            ServiceCollection services = new ServiceCollection();
            // Call the method to configure and register services.
            ConfigureServices(services);
            // Build the ServiceProvider from the service collection.
            ServiceProvider = services.BuildServiceProvider();
        }

        // Configures services and registers them with the dependency injection container.
        private void ConfigureServices(ServiceCollection services)
        {
            // Register the IApiClient interface with its implementation ApiClient as a singleton.
            // Singleton services are created once per application lifetime.
            services.AddSingleton<IApiClient, ApiClient>();

            // Register and configure the HttpClient used by IApiClient.
            // This setup is particularly useful for setting global HTTP client configurations like the base address.
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7041/api/"); // API base URL; adjust as needed.
            });

            // Initialize the AutoMapper for DI
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingConfigurations());
                cfg.AddProfile<GUIAutoMapperProfile>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IEmployeeUserService, EmployeeUserService>();

            // Register the LoginViewModel as a singleton to ensure a single instance is used throughout the application.
            services.AddSingleton<LoginViewModel>();

            services.AddSingleton<ProductsViewModel>(provider => new ProductsViewModel(
                provider.GetRequiredService<IApiClient>(),
                provider.GetRequiredService<IMapper>(),
                provider.GetRequiredService<IDialogService>()));

            // Register the LoginWindow and set its DataContext to the LoginViewModel.
            // This demonstrates how to perform more complex service registrations that require factory methods.
            services.AddSingleton<LoginWindow>(provider =>
            {
                // Resolve the LoginViewModel instance from the service provider.
                var viewModel = provider.GetRequiredService<LoginViewModel>();
                // Create the LoginWindow instance and set its DataContext to the resolved ViewModel.
                var window = new LoginWindow
                {
                    DataContext = viewModel,
                };
                return window;
            });

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<EmployeeViewModel>();
            services.AddSingleton<TimeClockViewModel>();
            services.AddSingleton<ShipmentsViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<IncidentLogViewModel>();


        }

        // Override the OnStartup method to perform actions when the application starts.
        protected override void OnStartup(StartupEventArgs e)
        {
            // Ensure to call the base class's OnStartup method.
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Resolve the LoginWindow from the service provider.
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();

            // Show the LoginWindow as the application starts.
            loginWindow.Show();
        }
    }
}
