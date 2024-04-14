using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.DTOs.User;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class EmployeeFormViewModel : ViewModelBase, IDialogRequestClose, INotifyDataErrorInfo
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        private Employee _currentEmployee;

        public event EventHandler EmployeeUpdated;
        protected virtual void OnEmployeeUpdated()
        {
            EmployeeUpdated?.Invoke(this, EventArgs.Empty);
        }
        public Action CloseAction { get; set; }


        public Employee CurrentEmployee
        {
            get => _currentEmployee;
            set
            {
                if (_currentEmployee != value)
                {
                    _currentEmployee = value;
                    OnPropertyChanged(nameof(CurrentEmployee));
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
                    WindowTitle = value ? "Edit Employee" : "Add Employee";
                    OnPropertyChanged(nameof(IsEditMode));
                }
            }
        }

        private string _firstName = "";
        public string FirstName
        {
            get => string.IsNullOrEmpty(_firstName) ? "" : _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _middleName = "";
        public string MiddleName
        {
            get => string.IsNullOrEmpty(_middleName) ? "" : _middleName;
            set
            {
                _middleName = value;
                OnPropertyChanged();
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get => string.IsNullOrEmpty(_LastName) ? "" : _LastName;
            set
            {
                _LastName = value;
                OnPropertyChanged();
                ValidateProperty(nameof(LastName));
            }
        }

        private string _address = "";

        public string Address
        {
            get => string.IsNullOrEmpty(_address) ? "" : _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
                ValidateProperty(nameof(Address));
            }
        }
        private string _city = "";

        public string City
        {
            get => string.IsNullOrEmpty(_city) ? "" : _city;
            set
            {
                City = value;
                OnPropertyChanged(nameof(City));
                ValidateProperty(nameof(City));
            }
        }

        private string _state = "";

        public string State
        {
            get => string.IsNullOrEmpty(_state) ? "" : _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
                ValidateProperty(nameof(State));
            }
        }

        private string _zip = "";

        public string Zip
        {
            get => string.IsNullOrEmpty(_zip) ? "" : _zip;
            set
            {
                _zip = value;
                OnPropertyChanged(nameof(Zip));
                ValidateProperty(nameof(Zip));
            }
        }

        private string _homePhoneNumber = "";

        public string HomePhoneNumber
        {
            get => string.IsNullOrEmpty(_homePhoneNumber) ? "" : _homePhoneNumber;
            set
            {
                _homePhoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _cellPhoneNumber = "";

        private string CellPhoneNumber
        {
            get => string.IsNullOrEmpty(_cellPhoneNumber) ? "" : _cellPhoneNumber;
            set
            {
                _cellPhoneNumber = value;
                OnPropertyChanged();
            }
        }

        private decimal _payRate;

        public decimal PayRate
        {
            get => _payRate;
            set
            {
                _payRate = value;
                OnPropertyChanged();
            }
        }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private string _userRole = "";

        public string UserRole
        {
            get => string.IsNullOrEmpty(_userRole) ? "" : _userRole;
            set
            {
                _userRole = value;
                OnPropertyChanged();
            }
        }


        private string _windowTitle = "Add Employee";

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

        public EmployeeFormViewModel(IApiClient apiClient, IMapper mapper, Employee employee = null)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            IsEditMode = false;

            if (employee != null)
            {
                CurrentEmployee = employee;
                IsEditMode = true;
            }
            else
            {
                CurrentEmployee = new Employee();
                IsEditMode = false;
            }

            if (IsEditMode)
            {
                FirstName = CurrentEmployee.FirstName;
                MiddleName = CurrentEmployee.MiddleName;
                LastName = CurrentEmployee.LastName;
                Address = CurrentEmployee.Address;
                City = CurrentEmployee.City;
                State = CurrentEmployee.State;
                Zip = CurrentEmployee.Zip;
                HomePhoneNumber = CurrentEmployee.HomePhoneNumber;
                CellPhoneNumber = CurrentEmployee.CellPhoneNumber;
                PayRate = CurrentEmployee.PayRate;
                StartDate = CurrentEmployee.StartDate;
                UserRole = CurrentEmployee.UserRole;
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
                await UpdateEmployeeAsync();
            }
            else
            {
                await AddEmployeeAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private void TriggerValidationErrors()
        {
            ValidateProperty(nameof(LastName));
            ValidateProperty(nameof(Address));

            OnPropertyChanged(nameof(PriceError));
            OnPropertyChanged(nameof(QuantityOnHandError));
        }

        private async Task AddEmployeeAsync()
        {
            try
            {
                var newEmployeeDto = _mapper.Map<NewAccountDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("auth/create", newEmployeeDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    EmployeeUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task UpdateEmployeeAsync()
        {
            try
            {

                var updatedEmployeeDto = _mapper.Map<EmployeeDto>(this);
                updatedEmployeeDto.EmployeeId = CurrentEmployee.EmployeeId;

                var result = await _apiClient.PutAsync<OperationResult>("inventory/update", updatedEmployeeDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    EmployeeUpdated?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Cleanup()
        {
            EmployeeUpdated = null;
        }


        public string PriceError => _submissionAttempted ? GetFirstError("LastName") : string.Empty;
        public string QuantityOnHandError => _submissionAttempted ? GetFirstError("Address") : string.Empty;

        private Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        private void ValidateProperty(string propertyName)
        {
            // Clear existing errors
            _validationErrors.Remove(propertyName);
            ICollection<string> errors = new List<string>();

            // Validate LastName
            if (propertyName == nameof(LastName) && !decimal.TryParse(LastName, out _))
            {
                errors.Add("LastName must be a valid decimal number.");
            }

            // Validate Address
            if (propertyName == nameof(Address) && !int.TryParse(Address, out _))
            {
                errors.Add("Quantity must be a valid integer.");
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
