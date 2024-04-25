using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.DirectoryServices.ActiveDirectory;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using Xceed.Wpf.Toolkit;
using System.Windows;
using System.Windows.Input;
using TCMS.Common.DTOs.Inventory;
using System.ComponentModel;
using Bogus.DataSets;
using System.IO.Pipelines;

namespace TCMS.GUI.ViewModels
{
    internal class VehicleFormViewModel : ViewModelBase, IDialogRequestClose, INotifyDataErrorInfo
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        private GUI.Models.Vehicle _currentVehicle;

        public event EventHandler VehicleUpdated;

        protected virtual void OnVehicleUpdated()
        {
            VehicleUpdated?.Invoke(this, EventArgs.Empty);
        }
        public Action CloseAction { get; set; }

        public GUI.Models.Vehicle CurrentVehicle
        {
            get => _currentVehicle;
            set
            {
                if (_currentVehicle != value)
                {
                    _currentVehicle = value;
                    OnPropertyChanged(nameof(CurrentVehicle));
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
                    WindowTitle = value ? "Edit Vehicle" : "Add Vehicle";
                    OnPropertyChanged(nameof(IsEditMode));
                }
            }
        }

        private string _brand = "";
        public string Brand
        {
            get => string.IsNullOrEmpty(_brand) ? "" : _brand;
            set
            {
                _brand = value;
                OnPropertyChanged();
            }
        }

        private string _model = "";
        public string Model
        {
            get => string.IsNullOrEmpty(_model) ? "" : _model;
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        private string _year = "";
        public string Year
        {
            get => string.IsNullOrEmpty(_year) ? "" : _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        private string _type = "";

        public string Type
        {
            get => string.IsNullOrEmpty(_type) ? "" : _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string _windowTitle = "Add Vehicle";

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

        private bool _submissionAttempted = false;
        public ICommand ConfirmCommand { get; }

        public VehicleFormViewModel(IApiClient apiClient, IMapper mapper, GUI.Models.Vehicle vehicle = null)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            IsEditMode = false;

            if (vehicle != null)
            {
                CurrentVehicle = vehicle;
                IsEditMode = true;
            }
            else
            {
                CurrentVehicle = new GUI.Models.Vehicle();
                IsEditMode = false;
            }

            if (IsEditMode)
            {
                Brand = CurrentVehicle.Brand;
                Model = CurrentVehicle.Model;
                Year = CurrentVehicle.Year.ToString();
                Type = CurrentVehicle.Type;
            }

            ConfirmCommand = new RelayCommand(Confirm);
        }

        private async void Confirm(object obj)
        {
            _submissionAttempted = true;

            if (HasErrors)
            {
                TriggerValidationErrors();
                return;
            }

            if (IsEditMode)
            {
                await UpdateVehicleAsync();
            }
            else
            {
                await AddVehicleAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private void TriggerValidationErrors()
        {
            ValidateProperty(nameof(Year));

            OnPropertyChanged(nameof(YearError));
        }

        private async Task AddVehicleAsync()
        {
            try
            {
                var newVehicleDto = _mapper.Map<VehicleCreateDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("vehicle/create", newVehicleDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    VehicleUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async Task UpdateVehicleAsync()
        {
            try
            {

                var updatedVehicleDto = _mapper.Map<VehicleDto>(this);
                updatedVehicleDto.VehicleId = CurrentVehicle.VehicleID;

                var result = await _apiClient.PutAsync<OperationResult>("vehicle/update", updatedVehicleDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    VehicleUpdated?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public void Cleanup()
        {
            VehicleUpdated = null;
        }

        public string YearError => _submissionAttempted ? GetFirstError("LastName") : string.Empty;

        private Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        private void ValidateProperty(string propertyName)
        {
            // Clear existing errors
            _validationErrors.Remove(propertyName);
            ICollection<string> errors = new List<string>();

            // Validate LastName
            if (propertyName == nameof(Year) && !int.TryParse(Year, out _))
            {
                errors.Add("Year must be a valid integer.");
            }

            if (errors.Any())
            {
                _validationErrors.Add(propertyName, errors);
                OnErrorsChanged(propertyName);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
        public bool HasErrors => _validationErrors.Any();

        private string GetFirstError(string propertyName)
        {
            if (_validationErrors.TryGetValue(propertyName, out ICollection<string> errors) && errors.Count > 0)
            {
                return errors.First(); // Just getting the first error for simplicity
            }
            return string.Empty;
        }
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName)) return null;
            return _validationErrors[propertyName];
        }



    }
}
