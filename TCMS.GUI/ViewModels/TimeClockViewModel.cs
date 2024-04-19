using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public TimeClockViewModel()
            {
                TimeRecords = new ObservableCollection<TimeRecord>
            {
                // Example data
                new TimeRecord { ClockInTime = DateTime.Now.AddHours(-1), ClockOutTime = DateTime.Now, TotalHours = 1 }
            };
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



