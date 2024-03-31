using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCMS.Common.DTOs.User;
using TCMS.GUI.Services.Implementations;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.ViewModels;

// Ensure the correct namespaces are being used, particularly for types like Window, RoutedEventArgs, etc.
namespace TCMS.GUI.Views
{
    // Summary description for the LoginWindow class.
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// This class represents the code-behind for the LoginWindow, handling UI events and interactions.
    /// </summary>
    // LoginWindow.xaml.cs indicates that this file contains the code-behind for the LoginWindow XAML file.
    public partial class LoginWindow : Window
    {
        // Constructor for the LoginWindow class.
        public LoginWindow()
        {
            // Initializes the component (UI elements defined in XAML) of the LoginWindow.
            // This method is auto-generated and should always be called first in the constructor to ensure
            // the UI is setup correctly before performing any additional initialization logic.
            InitializeComponent();

            // Subscribes to the PasswordChanged event of the passwordBox control.
            // This event is triggered every time the text in the passwordBox (a PasswordBox control) changes.
            // It allows the ViewModel to be updated with the new password as the user types it.
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        // Event handler for when the password in the passwordBox changes.
        // This method ensures that the password entered into the passwordBox is passed to the ViewModel.
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Checks if the DataContext of the window is an instance of LoginViewModel.
            // The DataContext is typically set in the XAML or in the window's constructor and binds the ViewModel to the View.
            if (DataContext is LoginViewModel viewModel)
            {
                // If the DataContext is a LoginViewModel, update its Password property with the text from the passwordBox.
                // This is necessary because the PasswordBox does not support data binding for its Password property due to security reasons.
                // Therefore, we manually synchronize the Password property of the ViewModel with the content of the passwordBox.
                viewModel.Password = passwordBox.Password;
            }
        }
    }
}

