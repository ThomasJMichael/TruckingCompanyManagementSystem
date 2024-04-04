using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.GUI.Views;
using Xceed.Wpf.Toolkit;

namespace TCMS.GUI.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        // Example property representing a list of products
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        private Lazy<Task> _LazyProductsLoader;

        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteredProducts;

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            private set
            {
                _filteredProducts = value;
                OnPropertyChanged();
            }
        }

        private Product _selectedProduct;

        private string _searchText = "Search Products...";

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Try to ensure the products are loaded before filtering.
                _ = EnsureProductsLoadedAsync().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        // Products are loaded, perform the filter.
                        FilterProducts();
                    }
                    // If the task is not completed, it's either still running or failed.
                    // If it's still running, the search will be triggered once it completes.
                    // If it failed, may want to log the error or inform the user as needed.
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private bool _isSearchBoxFocused;

        public bool IsSearchBoxFocused
        {
            get => _isSearchBoxFocused;
            set
            {
                _isSearchBoxFocused = value;
                AdjustSearchTextOnFocus();
                OnPropertyChanged();
            }
        }

        private void FilterProducts()
        {
            Console.WriteLine("FilterProducts called");

            if (_products == null)
            {
                Console.WriteLine("Products collection is null, loading products.");
                EnsureProductsLoadedAsync();
                return;
            }

            Console.WriteLine($"Current SearchText: '{SearchText}'");
            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Products...")
            {
                Console.WriteLine("SearchText is empty or placeholder text, setting filteredProducts to all products.");
                FilteredProducts = new ObservableCollection<Product>(_products);
            }
            else
            {
                Console.WriteLine("Searching...");
                // Determine if search text is purely numeric
                bool isNumericSearch = int.TryParse(SearchText, out int searchId);
                Console.WriteLine($"isNumericSearch: {isNumericSearch}, searchId: {searchId}");

                var filtered = _products.Where(product =>
                        (isNumericSearch && product.ProductId.ToString().Contains(SearchText)) ||
                        (!isNumericSearch && product.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                Console.WriteLine($"Found {filtered.Count} products after filtering.");
                FilteredProducts = new ObservableCollection<Product>(filtered);
            }

            Console.WriteLine($"FilteredProducts count after filtering: {FilteredProducts.Count}");
        }


        private void PerformSearch()
        {
            FilterProducts();
        }

        // Commands
        public ICommand AddProductCommand { get; private set; }
        public ICommand EditProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }
        public ICommand RefreshProductsCommand { get; private set; }
        public ICommand SearchCommand { get;}
        public ICommand SearchBoxGotFocusCommand { get; }
        public ICommand SearchBoxLostFocusCommand { get; }

        public ProductsViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;
            // Initialize commands
            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand(EditProduct, CanExecuteEditOrDelete);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanExecuteEditOrDelete);
            RefreshProductsCommand = new RelayCommand(RefreshProducts);
            SearchCommand = new RelayCommand((obj) => FilterProducts());
            SearchBoxGotFocusCommand = new RelayCommand(SearchBoxGotFocus);
            SearchBoxLostFocusCommand = new RelayCommand(SearchBoxLostFocus);

            _products = new ObservableCollection<Product>();
            _filteredProducts = new ObservableCollection<Product>();

            InitializeLazyLoader();

        }

        private void InitializeLazyLoader()
        {
            _LazyProductsLoader = new Lazy<Task>(() => LoadProductsAsync(), isThreadSafe: true);
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var result = await _apiClient.GetAsync<OperationResult<IEnumerable<ProductDto>>>("manifest/product/all");
                if (result.IsSuccessful && result.Data != null)
                {
                    var products = _mapper.Map<IEnumerable<Product>>(result.Data);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _products.Clear();
                        foreach (var product in products)
                        {
                            _products.Add(product);
                        }
                        FilterProducts();
                    });
                }
                else
                {
                    // Log failure or handle accordingly
                    Debug.WriteLine("Failed to load products or no products returned.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Debug.WriteLine($"An error occurred while loading products: {ex.Message}");
            }
        }


        public async Task EnsureProductsLoadedAsync()
        {
            if (!_LazyProductsLoader.IsValueCreated)
            {
                await _LazyProductsLoader.Value;
            }
        }

        private void AdjustSearchTextOnFocus()
        {
            if (_isSearchBoxFocused && SearchText == "Search Products...")
            {
                SearchText = string.Empty;
            }
            else if (!_isSearchBoxFocused && string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Products...";
            }
        }

        private void SearchBoxGotFocus(object obj)
        {
            if (SearchText == "Search Products...")
            {
                SearchText = string.Empty;
            }
        }

        private void SearchBoxLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Products...";
            }
        }


        private void AddProduct(object obj)
        {
            var newProductForm = new ProductFormViewModel(_apiClient, _mapper);
            newProductForm.ProductUpdated += OnProductUpdated;
            _dialogService.ShowDialog(newProductForm);
            newProductForm.ProductUpdated -= OnProductUpdated;
        }

        private void OnProductUpdated(object sender, EventArgs e)
        {
            RefreshProducts(null);
        }


        private void Refresh()
        {
            _ = LoadProductsAsync();
        }

        private void EditProduct(object obj)
        {
            if (SelectedProduct != null)
            {
                var editProductFormViewModel = new ProductFormViewModel(_apiClient, _mapper, SelectedProduct);
                editProductFormViewModel.ProductUpdated += OnProductUpdated;
                _dialogService.ShowDialog(editProductFormViewModel);
                editProductFormViewModel.Cleanup();
                editProductFormViewModel.ProductUpdated -= OnProductUpdated;
            }
        }

        private bool CanExecuteEditOrDelete(object obj)
        {
            // Determine if the edit/delete command can execute, e.g., based on if a product is selected
            return true; // Example logic
        }

        private void DeleteProduct(object obj)
        {
            // Implementation for deleting a selected product
        }

        private void RefreshProducts(object obj)
        {
            Refresh();
        }
    }
}

