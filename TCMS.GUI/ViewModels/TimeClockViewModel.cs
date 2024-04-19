using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class TimeClockViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TimeRecord> _timeRecords;

        public ObservableCollection<TimeRecord> TimeRecords
        {
            get => _timeRecords;
            set
            {
                _timeRecords = value;
                OnPropertyChanged(nameof(TimeRecords));
            }
        }

        private DateTime? _lastClockInTime;

        public ICommand ClockInCommand { get; private set; }
        public ICommand ClockOutCommand { get; private set; }

        public TimeClockViewModel()
        {
            TimeRecords = new ObservableCollection<TimeRecord>();
            ClockInCommand = new RelayCommand(ExecuteClockIn);
            ClockOutCommand = new RelayCommand(ExecuteClockOut, CanExecuteClockOut);
        }

        private void ExecuteClockIn(object parameter)
        {
            _lastClockInTime = DateTime.Now; // Save the clock-in time
        }

        private void ExecuteClockOut(object parameter)
        {
            if (_lastClockInTime.HasValue)
            {
                var now = DateTime.Now;
                var totalHours = (now - _lastClockInTime.Value).TotalHours;
                TimeRecords.Add(new TimeRecord { ClockInTime = _lastClockInTime.Value, ClockOutTime = now, TotalHours = totalHours });
                _lastClockInTime = null; // Reset clock-in time after clocking out
            }
        }

        private bool CanExecuteClockOut(object parameter)
        {
            return _lastClockInTime.HasValue; // Enable clock out only if there's a valid clock-in time
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TimeRecord
    {
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public double TotalHours { get; set; }
    }
}
