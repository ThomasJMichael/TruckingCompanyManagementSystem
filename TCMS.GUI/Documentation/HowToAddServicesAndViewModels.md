### Guide to Adding Services and ViewModels in a WPF Application

This guide is designed to help you integrate services and view models into the WPF application.

#### Step 1: Create Interfaces for Your Services

Interfaces define contracts for your services, promoting loose coupling between components. For each service, create an interface that outlines the methods that will be exposed.

Example (`IApiClient.cs`):

```csharp
public interface IApiClient
{
    Task<OperationResult<T>> PostAsync<T>(string uri, object value);
    // Define other necessary methods
}
```

#### Step 2: Implement Services

Implement the services by creating classes that fulfill the interfaces' contracts.

Example (`ApiClient.cs`):

```csharp
public class ApiClient : IApiClient
{
    public async Task<OperationResult<T>> PostAsync<T>(string uri, object value)
    {
        // Implementation of the PostAsync method
    }
}
```

#### Step 3: Create ViewModels

ViewModels act as intermediaries between views and models, handling the logic for view updates.

Example (`LoginViewModel.cs`):

```csharp
public class LoginViewModel
{
    private readonly IApiClient _apiClient;

    public LoginViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // ViewModel logic here
}
```

#### Step 4: Register Services and ViewModels in DI Container

Configure your DI container in the `App.xaml.cs` file, registering services and view models.

```csharp
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IApiClient, ApiClient>();
        // Register other services and view models
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
    }
}
```

#### Step 5: Resolving and Using Services in ViewModels

When instantiating view models, the DI container will automatically inject the required services.
