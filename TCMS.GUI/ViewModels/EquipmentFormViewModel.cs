using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    class EquipmentFormViewModel : ViewModelBase, IDialogRequestClose, INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        private Equipment _currentEquipment;

        public event EventHandler EquipmentUpdated;

        protected virtual void OnEquipmentUpdated()
        {
            EquipmentUpdated?.Invoke(this, EventArgs.Empty);
        }
        public Action CloseAction { get; set; }

        private bool _isFollowUpComplete;

        public bool IsFollowUpComplete
        {
            get => _isFollowUpComplete;
            set
            {
                if (_isFollowUpComplete != value)
                {
                    _isFollowUpComplete = value;
                    OnPropertyChanged(nameof(IsFollowUpComplete));
                }
            }
        }

        public Equipment CurrentEquipment
        {
            get => _currentEquipment;
            set
            {
                if (_currentEquipment != value)
                {
                    _currentEquipment = value;
                    OnPropertyChanged(nameof(CurrentEquipment));
                }
            }
        }

        private bool _isEditMode;

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    WindowTitle = value ? "Edit Product" : "Add Product";
                    OnPropertyChanged(nameof(IsEditMode));
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
                OnPropertyChanged(nameof(VehicleId));
            }
        }


        private string _brand = "Enter brand...";
        public string Brand
        {
            get => string.IsNullOrEmpty(_brand) ? "MiddleName" : _brand;
            set
            {
                _brand = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Brand));
            }
        }

        private string _model = "Enter model...";
        public string Model
        {
            get => string.IsNullOrEmpty(_model) ? "MiddleName" : _model;
            set
            {
                _model = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Model));
            }
        }

        private int _year;
        public int Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Year));
            }
        }

        private string _type = "Enter type...";
        public string Type
        {
            get => string.IsNullOrEmpty(_type) ? "MiddleName" : _type;
            set
            {
                _type = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Type));
            }
        }


        private string _windowTitle = "Add Product";

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        //public Visibility NamePlaceholderVisible => string.IsNullOrEmpty(_drugAndAlcoholTestId) ? Visibility.Visible : Visibility.Collapsed;
        //public Visibility DescriptionPlaceholderVisible => string.IsNullOrEmpty(_testDetails) ? Visibility.Visible : Visibility.Collapsed;
        //public Visibility PricePlaceholderVisible => DateTime.IsNullOrEmpty(_followUpTestDate) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ConfirmCommand { get; }

        public EquipmentFormViewModel(IApiClient apiClient, IMapper mapper, Equipment equipment = null)
        {

            _apiClient = apiClient;
            _mapper = mapper;
            IsEditMode = false;

            if (equipment != null)
            {
                CurrentEquipment = equipment;
                IsEditMode = true;
            }
            else
            {
                CurrentEquipment = new Equipment();
                IsEditMode = false;
            }

            if (IsEditMode)
            {
                Brand = CurrentEquipment.Brand;
                Model = CurrentEquipment.Model;
                Year = CurrentEquipment.Year;
                Type = CurrentEquipment.Type;

            }
            ConfirmCommand = new RelayCommand(Confirm);

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void Confirm(object obj)
        {
            if (IsEditMode)
            {
                await UpdateEquipmentAsync();
            }
            else
            {
                await AddEquipmentAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private async Task AddEquipmentAsync()
        {
            try
            {
                var newEquipmentDto = _mapper.Map<VehicleCreateDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("vehicle/create", newEquipmentDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    EquipmentUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task UpdateEquipmentAsync()
        {
            try
            {
                var updatedEquipmentDto = _mapper.Map<Equipment>(this);
                updatedEquipmentDto.VehicleId = CurrentEquipment.VehicleId;

                var result = await _apiClient.PutAsync<OperationResult>("vehicle/all", updatedEquipmentDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    EquipmentUpdated?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Cleanup()
        {
            EquipmentUpdated = null;
        }
    }

}

