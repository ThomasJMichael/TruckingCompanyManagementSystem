using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Views;

// Namespace declaration: specific to your project's structure
namespace TCMS.GUI.ViewModels
{
    // The LoginViewModel class, which implements the INotifyPropertyChanged interface
    // This interface is crucial for data binding in MVVM architectures, allowing the view to observe property changes.
    public class LoginViewModel : INotifyPropertyChanged
    {
        // Private fields
        private readonly IApiClient _apiClient; // A reference to an API client used to communicate with the backend.
        private string _loginErrorMessage; // Backing field for storing the login error message.

        // Public properties

        // LoginErrorMessage: Exposes the login error message to the view.
        // Changes to this property automatically notify the view to update via OnPropertyChanged.
        public string LoginErrorMessage
        {
            get => _loginErrorMessage;
            set => SetProperty(ref _loginErrorMessage, value);
        }

        private string _username; // Backing field for the username.
        private string _password; // Backing field for the password.

        // Username: User's username bound to the input field in the view.
        // Updates to this property notify the view due to the call to SetProperty in its setter.
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        // Password: User's password bound to the password input field in the view.
        // Similar to Username, it notifies the view of changes.
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        // LoginCommand: Represents the action to log in, bound to the login button in the view.
        // It's enabled/disabled based on the result of CanExecuteLogin.
        public ICommand LoginCommand { get; }

        // Event declaration for property changes, part of the INotifyPropertyChanged interface.
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public LoginViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient; // Initialize the API client.
            // Initialize the LoginCommand with a RelayCommand. ExecuteLogin is executed when the command is invoked.
            // CanExecuteLogin determines if the command can execute based on current property values.
            LoginCommand = new RelayCommand(async () => await ExecuteLogin(), CanExecuteLogin);
        }

        // CanExecuteLogin: Determines if the LoginCommand is executable.
        // Returns true if both username and password fields are not empty, otherwise false.
        private bool CanExecuteLogin() => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        // ExecuteLogin: Asynchronously attempts to log in with the provided credentials.
        private async Task ExecuteLogin()
        {
            var loginDto = new LoginDto { Username = Username, Password = Password }; // Create a DTO for login.
            try
            {
                // Attempt to log in via the API client. The result can be used to proceed based on success or failure.
                var result = await _apiClient.PostAsync<OperationResult>("api/auth/login", loginDto);
                if (result.IsSuccessful) // Assuming IsSuccess is a property that indicates success
                {
                    // Close the current Login window and open the main window
                    // This should run on the UI thread
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Close the current window. This assumes ExecuteLogin is called from the Login window's ViewModel.
                        // You might need a reference to the window you want to close if it's not the one directly associated.
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.DataContext == this)
                            {
                                window.Close();
                                break;
                            }
                        }

                        // Open the main window
                        var mainWindow = new MainWindow(); // Assuming MainWindow is the name of your main window class
                        mainWindow.Show();
                    });
                }
                else
                {
                    // Handle unsuccessful login (e.g., set LoginErrorMessage)
                    LoginErrorMessage = "Login failed. Please check your username and password.";
                }
            }
            catch (HttpRequestException ex) // Catch HTTP request exceptions separately to handle HTTP-related issues.
            {
                // Specific error handling based on the exception message.
                // Adjust these based on your backend API's error messages.
                if (ex.Message.Contains("User not found") || ex.Message.Contains("Invalid password"))
                {
                    LoginErrorMessage = "The username or password is incorrect. Please try again.";
                }
                else
                {
                    LoginErrorMessage = "An error occurred while trying to log in. Please try again later.";
                }
            }
            catch (Exception ex) // Catch all other exceptions.
            {
                // Handle unexpected errors, logging them for debugging purposes.
                LoginErrorMessage = "An unexpected error occurred. Please try again later.";
                Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        // OnPropertyChanged: Invoked when a property value changes to notify the view.
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // SetProperty: A generic method to set the value of a property and notify the view of the change.
        // Also, it ensures the command state is updated appropriately by invoking RaiseCanExecuteChanged on LoginCommand.
        private void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) return;
            backingField = value;
            OnPropertyChanged(propertyName);
            (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}

