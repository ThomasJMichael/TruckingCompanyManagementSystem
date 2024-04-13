using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using AutoMapper;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class IncidentLogFormViewModel : ViewModelBase, IDialogRequestClose
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        private IncidentReport _currentIncident;

        public event EventHandler IncidentUpdated;

        protected virtual void OnIncidentUpdated()
        {
            IncidentUpdated?.Invoke(this, EventArgs.Empty);
        }
        public Action CloseAction { get; set; }

        private bool _isFatal;
        private bool _hasInjuries;
        private bool _hasTowedVehicle;
        private bool _citationIssued;
        private bool _requiresDrugAndAlchoholTest;

        public bool IsFatal
        {
            get => _isFatal;
            set
            {
                if (_isFatal != value)
                {
                    _isFatal = value;
                    OnPropertyChanged(nameof(IsFatal));
                }
            }
        }
        public bool HasInjuries
        {
            get => _hasInjuries;
            set
            {
                if (_hasInjuries != value)
                {
                    _hasInjuries = value;
                    OnPropertyChanged(nameof(HasInjuries));
                }
            }
        }
        public bool HasTowedVehicle
        {
            get => _hasTowedVehicle;
            set
            {
                if (_hasTowedVehicle != value)
                {
                    _hasTowedVehicle = value;
                    OnPropertyChanged(nameof(HasTowedVehicle));
                }
            }
        }
        public bool CitationIssued
        {
            get => _citationIssued;
            set
            {
                if (_citationIssued != value)
                {
                    _citationIssued = value;
                    OnPropertyChanged(nameof(CitationIssued));
                }
            }
        }
        public bool RequiresDrugAndAlchoholTest
        {
            get => _requiresDrugAndAlchoholTest;
            set
            {
                if (_requiresDrugAndAlchoholTest != value)
                {
                    _requiresDrugAndAlchoholTest = value;
                    OnPropertyChanged(nameof(RequiresDrugAndAlchoholTest));
                }
            }
        }
        public IncidentReport CurrentIncident
        {
            get => _currentIncident;
            set
            {
                if (_currentIncident != value)
                {
                    _currentIncident = value;
                    OnPropertyChanged(nameof(CurrentIncident));
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

        private string _incidentReportId = "Enter Vehicle ID...";
        public string IncidentReportId
        {
            get => string.IsNullOrEmpty(_incidentReportId) ? "Name" : _incidentReportId;
            set
            {
                _incidentReportId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }
        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        private string _vehicleId = "Enter Vehicle ID...";
        public string VehicleId
        {
            get => string.IsNullOrEmpty(_vehicleId) ? "Name" : _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }

        private string _employeeId = "Enter Employee ID...";
        public string EmployeeId
        {
            get => string.IsNullOrEmpty(_employeeId) ? "Name" : _employeeId;
            set
            {
                _employeeId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }

        private string _location = "Enter Location...";
        public string Location
        {
            get => string.IsNullOrEmpty(_location) ? "Name" : _location;
            set
            {
                _location = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }


        private string _description = "Enter Incident description...";
        public string Description
        {
            get => string.IsNullOrEmpty(_description) ? "Description" : _description;
            set
            {
                _description = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }
        private string _incidentType1 = "Accident";
        public string IncidentType1
        {
            get => string.IsNullOrEmpty(_incidentType1) ? "IncidentType" : _incidentType1;
            set
            {
                _incidentType1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }
        private string _incidentType2 = "SafetyViolation";
        public string IncidentType2
        {
            get => string.IsNullOrEmpty(_incidentType2) ? "IncidentType" : _incidentType2;
            set
            {
                _incidentType2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
            }
        }
        private string _incidentType3 = "Other";
        public string IncidentType3
        {
            get => string.IsNullOrEmpty(_incidentType3) ? "IncidentType" : _incidentType3;
            set
            {
                _incidentType3 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DescriptionPlaceholderVisible));
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

        public Visibility NamePlaceholderVisible => string.IsNullOrEmpty(_incidentReportId) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DescriptionPlaceholderVisible => string.IsNullOrEmpty(_description) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PricePlaceholderVisible => string.IsNullOrEmpty(_location) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand ConfirmCommand { get; }

        public IncidentLogFormViewModel(IApiClient apiClient, IMapper mapper, IncidentReport incident = null)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            IsEditMode = false;

            if (incident != null)
            {
                CurrentIncident = incident;
                IsEditMode = true;
            }
            else
            {
                CurrentIncident = new IncidentReport();
                IsEditMode = false;
            }

            ConfirmCommand = new RelayCommand(Confirm);
        }

        private async void Confirm(object obj)
        {
            if (IsEditMode)
            {
                await UpdateIncidentAsync();
            }
            else
            {
                await AddIncidentAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private async Task AddIncidentAsync()
        {
            try
            {
                var newIncidentReportDto = _mapper.Map<IncidentReportDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("incident/all", newIncidentReportDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    IncidentUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task UpdateIncidentAsync()
        {
            try
            {

                var updatedIncidentDto = _mapper.Map<IncidentReportDto>(this);
                updatedIncidentDto.IncidentReportId = CurrentIncident.IncidentReportId;

                var result = await _apiClient.PutAsync<OperationResult>("incident/all", updatedIncidentDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    IncidentUpdated?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Cleanup()
        {
            IncidentUpdated = null;
        }
    }
}
