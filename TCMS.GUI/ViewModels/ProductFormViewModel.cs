using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class ProductFormViewModel : ViewModelBase, IDialogRequestClose, INotifyDataErrorInfo
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;

        private Product _currentProduct;

        public event EventHandler ProductUpdated;
        protected virtual void OnProductUpdated()
        {
            ProductUpdated?.Invoke(this, EventArgs.Empty);
        }
        public Action CloseAction { get; set; }


        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                if (_currentProduct != value)
                {
                    _currentProduct = value;
                    OnPropertyChanged(nameof(CurrentProduct));
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

        private string _name = "";
        public string Name
        {
            get => string.IsNullOrEmpty(_name) ? "" : _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _description = "";
        public string Description
        {
            get => string.IsNullOrEmpty(_description) ? "" : _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _price = "";
        public string Price
        {
            get => string.IsNullOrEmpty(_price) ? "" : _price;
            set
            {
                _price = value;
                OnPropertyChanged();
                ValidateProperty(nameof(Price));
            }
        }

        private string _quantityOnHand = "";

        public string QuantityOnHand
        {
            get => string.IsNullOrEmpty(_quantityOnHand) ? "" : _quantityOnHand;
            set
            {
                _quantityOnHand = value;
                OnPropertyChanged(nameof(QuantityOnHand));
                ValidateProperty(nameof(QuantityOnHand));
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


        private bool _submissionAttempted = false;
        public ICommand ConfirmCommand { get; }

        public ProductFormViewModel(IApiClient apiClient, IMapper mapper, Product product = null)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            IsEditMode = false;

            if (product != null)
            {
                CurrentProduct = product;
                IsEditMode = true;
            }
            else
            {
                CurrentProduct = new Product(); 
                IsEditMode = false;
            }

            if (IsEditMode)
            {
                Name = CurrentProduct.Name;
                Description = CurrentProduct.Description;
                Price = CurrentProduct.Price.ToString("F2");
                QuantityOnHand = CurrentProduct.QuantityOnHand.ToString();
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
                await UpdateProductAsync();
            }
            else
            {
                await AddProductAsync();
            }

            // Close the window or navigate back as appropriate
            CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
        }

        private void TriggerValidationErrors()
        {
            ValidateProperty(nameof(Price));
            ValidateProperty(nameof(QuantityOnHand));

            OnPropertyChanged(nameof(PriceError));
            OnPropertyChanged(nameof(QuantityOnHandError));
        }

    private async Task AddProductAsync()
        {
            try
            {
                var newProductDto = _mapper.Map<AddProductDto>(this);

                var result = await _apiClient.PostAsync<OperationResult>("manifest/product/add", newProductDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    ProductUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task UpdateProductAsync()
        {
            try
            {

                var updatedProductDto = _mapper.Map<InventoryProductDetailDto>(this);
                updatedProductDto.ProductId = CurrentProduct.ProductId;

                var result = await _apiClient.PutAsync<OperationResult>("inventory/update", updatedProductDto);
                if (!result.IsSuccessful)
                {
                    Debug.WriteLine(result.Messages);
                }
                else if (result.IsSuccessful)
                {
                    ProductUpdated?.Invoke(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Cleanup()
        {
            ProductUpdated = null;
        }


        public string PriceError => _submissionAttempted ? GetFirstError("Price") : string.Empty;
        public string QuantityOnHandError => _submissionAttempted ? GetFirstError("QuantityOnHand") : string.Empty;

        private Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        private void ValidateProperty(string propertyName)
        {
            // Clear existing errors
            _validationErrors.Remove(propertyName);
            ICollection<string> errors = new List<string>();

            // Validate Price
            if (propertyName == nameof(Price) && !decimal.TryParse(Price, out _))
            {
                errors.Add("Price must be a valid decimal number.");
            }

            // Validate QuantityOnHand
            if (propertyName == nameof(QuantityOnHand) && !int.TryParse(QuantityOnHand, out _))
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
