using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using TCMS.GUI.Services.Implementations;
using TCMS.GUI.Services.Interfaces;
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
        private readonly ServiceProvider _serviceProvider;

        // Constructor for the App class.
        public App()
        {
            // Create a new ServiceCollection to register application services.
            ServiceCollection services = new ServiceCollection();
            // Call the method to configure and register services.
            ConfigureServices(services);
            // Build the ServiceProvider from the service collection.
            _serviceProvider = services.BuildServiceProvider();
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
                client.BaseAddress = new Uri("https://localhost:7041/"); // API base URL; adjust as needed.
            });

            // Register the LoginViewModel as a singleton to ensure a single instance is used throughout the application.
            services.AddSingleton<LoginViewModel>();

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

            // Additional services and ViewModels can be registered here following a similar pattern.
        }

        // Override the OnStartup method to perform actions when the application starts.
        protected override void OnStartup(StartupEventArgs e)
        {
            // Ensure to call the base class's OnStartup method.
            base.OnStartup(e);

            // Resolve the LoginWindow from the service provider.
            var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
            // Although the DataContext is already set in ConfigureServices, 
            // it demonstrates how you could re-assign or ensure it's set as expected.
            loginWindow.DataContext = _serviceProvider.GetRequiredService<LoginViewModel>();
            // Show the LoginWindow as the application starts.
            loginWindow.Show();
        }
    }
}
