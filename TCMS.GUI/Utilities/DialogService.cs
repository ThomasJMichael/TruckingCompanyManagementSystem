using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TCMS.GUI.Utilities
{
    public class DialogService : IDialogService
    {
        public void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            // Assuming you have a corresponding view for each view model
            var viewTypeName = typeof(TViewModel).Name.Replace("ViewModel", "");
            var viewTypeFullName = $"TCMS.GUI.Views.{viewTypeName}";
            var viewType = Assembly.GetExecutingAssembly().GetType(viewTypeFullName);

            if (viewType == null) throw new InvalidOperationException($"View type not found for {viewTypeName}");

            var view = Activator.CreateInstance(viewType) as Window;
            if (view == null) throw new InvalidOperationException($"View could not be created for {viewTypeName}");

            // Set up the close action on the ViewModel.
            viewModel.CloseRequested += (_, __) => view.Close();

            view.DataContext = viewModel;
            view.ShowDialog();
        }
    }

}
