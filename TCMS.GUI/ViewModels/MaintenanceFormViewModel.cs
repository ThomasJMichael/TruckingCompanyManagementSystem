using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class MaintenanceFormViewModel : Utilities.ViewModelBase, IDialogRequestClose, INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IDialogService _dialogService;
        private readonly IMapper _mapper;

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                    _description = value;
                    OnPropertyChanged();
            }
        }
        public Array RecordTypes => Enum.GetValues(typeof(RecordType));
        private RecordType _selectedRecordType;

        public RecordType SelectedRecordType
        {
            get => _selectedRecordType;
            set
            {
                if (_selectedRecordType != value)
                {
                    _selectedRecordType = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _maintenanceDate;

        public DateTime MaintenanceDate
        {
            get => _maintenanceDate;
            set
            {
                if (_maintenanceDate != value)
                {
                    _maintenanceDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _totalCost;

        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost != value)
                {
                    _totalCost = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _vehicleId;

        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditMode;

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }
        public event EventHandler RecordsUpdated;
        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
        public ICommand AddPartsCommand { get; private set; }
        public ICommand ConfirmCommand { get; private set; }
        public MaintenanceFormViewModel(IApiClient apiClient, IDialogService dialogService, IMapper mapper, MaintenanceRecord? maintenanceRecord = null)
        {
            _apiClient = apiClient;
            _dialogService = dialogService;
            _mapper = mapper;
            _isEditMode = false;

            if (maintenanceRecord != null)
            {
                Id = maintenanceRecord.MaintenanceRecordID;
                Description = maintenanceRecord.Description;
                SelectedRecordType = maintenanceRecord.RecordType;
                MaintenanceDate = maintenanceRecord.MaintenanceDate;
                TotalCost = maintenanceRecord.Cost;
                VehicleId = maintenanceRecord.VehicleId;
                IsEditMode = true;
            }
            else
            {
                CreateInitialRecord();
            }

            AddPartsCommand = new RelayCommand(AddParts);
            ConfirmCommand = new RelayCommand(Confirm);

        }

        private void AddParts(object obj)
        {
            throw new NotImplementedException();
        }

        private void Confirm(object obj)
        {
            if (!IsEditMode)
            {
                CreateNewRecord();
            }
            else
            {
                UpdateRecord();
            }
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private async void CreateInitialRecord()
        {
            var initialRecordDto = new MaintenanceRecordDto
            {
                Description = "Initial",
                RecordType = RecordType.Maintenance,
                MaintenanceDate = DateTime.Now,
                Cost = 0,
                VehicleId = 1
            };
            var result = await _apiClient.PostAsync<OperationResult<MaintenanceRecordDto>>("maintenance/create", initialRecordDto);
            if (result.IsSuccessful)
            {
                Id = result.Data.MaintenanceRecordId;
            }
            else
            {
                Debug.WriteLine(result.Errors);
            }
        }

        private async void CreateNewRecord()
        {
            var newRecord = new MaintenanceRecordDto
            {
                Description = Description,
                RecordType = SelectedRecordType,
                MaintenanceDate = MaintenanceDate,
                Cost = TotalCost,
                VehicleId = VehicleId
            };

            var result = await _apiClient.PostAsync<OperationResult>("maintenance/create", newRecord);
            if (result.IsSuccessful)
            {
                RecordsUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.WriteLine(result.Messages);
            }
        }

        private async void UpdateRecord()
        {
            var updatedRecord = new MaintenanceRecordDto
            {
                MaintenanceRecordId = Id,
                Description = Description,
                RecordType = SelectedRecordType,
                MaintenanceDate = MaintenanceDate,
                Cost = TotalCost,
                VehicleId = VehicleId
            };

            var result = await _apiClient.PutAsync<OperationResult>("maintenance/update", updatedRecord);
            if (result.IsSuccessful)
            {
                RecordsUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.WriteLine(result.Messages);
            }
        }
    }
}
