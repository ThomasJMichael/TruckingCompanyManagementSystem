using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class ProductFormViewModel : ViewModelBase, IDialogRequestClose
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

        private string _name = "Enter product name...";
        public string Name
        {
            get => string.IsNullOrEmpty(_name) ? "Name" : _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamePlaceholderVisible));
            }
        }

        private string _description = "Enter product description...";
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

        private string _price = "Enter product price...";
        public string Price
        {
            get => string.IsNullOrEmpty(_price) ? "Price" : _price;
            set
            {
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PricePlaceholderVisible));
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

        public Visibility NamePlaceholderVisible => string.IsNullOrEmpty(_name) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility DescriptionPlaceholderVisible => string.IsNullOrEmpty(_description) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PricePlaceholderVisible => string.IsNullOrEmpty(_price) ? Visibility.Visible : Visibility.Collapsed;

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

            ConfirmCommand = new RelayCommand(Confirm);
        }

        private async void Confirm(object obj)
        {
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

        private async Task AddProductAsync()
        {
            try
            {
                var newProductDto = _mapper.Map<ProductDto>(this);

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

                var updatedProductDto = _mapper.Map<ProductDto>(this);
                updatedProductDto.ProductId = CurrentProduct.ProductId;

                var result = await _apiClient.PutAsync<OperationResult>("manifest/product/update", updatedProductDto);
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
    }
}
