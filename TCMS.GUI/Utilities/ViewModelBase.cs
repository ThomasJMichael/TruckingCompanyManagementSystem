using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TCMS.GUI.Utilities
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Asynchronously executes the provided task and manages the IsBusy state.
        // Also provides basic error handling.
        protected async Task ExecuteAsync(Func<Task> operation, Action<Exception> onError = null)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await operation();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                onError?.Invoke(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

