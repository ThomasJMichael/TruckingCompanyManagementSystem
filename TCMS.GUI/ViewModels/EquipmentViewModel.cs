using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using Xceed.Wpf.Toolkit;
using System.Windows.Input;
using TCMS.Common.DTOs.Inventory;

namespace TCMS.GUI.ViewModels
{
    public class EquipmentViewModel : ViewModelBase
    {
        // Represent list of vehicles
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        private Lazy<Task> _LazyVehiclesLoader;

        private ObservableCollection<Vehicle> _vehicles;
        private ObservableCollection<Vehicle> _filteredVehicles;

        // Similar to Inventory segment
        public ObservableCollection<Vehicle> FilteredVehicles
        {
            get => _filteredVehicles;
            private set
            {
                _filteredVehicles = value;
                OnPropertyChanged();
            }
        }

        private Vehicle _selectedVehicle;

        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set { _selectedVehicle = value; OnPropertyChanged(); }
        }

        private string _searchText = "Search Vehicles...";
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Try to ensure the vehicles are loaded before filtering.
                _ = EnsureVehiclesLoadedAsync().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        // Vehicles are loaded, perform the filter.
                        FilterVehicles();
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

        private void FilterVehicles()
        {   
            Console.WriteLine("FilterVehicles called");

            if (_vehicles == null)
            {
                Console.WriteLine("Vehicles collection is null, loading vehicles.");
                EnsureVehiclesLoadedAsync();
                return;
            }

            Console.WriteLine($"Current SearchText: '{SearchText}'");
            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Vehicles...")
            {
                Console.WriteLine("SearchText is empty or placeholder text, setting filteredVehicles to all vehicles.");
                FilteredVehicles = new ObservableCollection<Vehicle>(_vehicles);
            }
            else
            {
                Console.WriteLine("Searching...");
                // Determine if search text is purely numeric
                bool isNumericSearch = int.TryParse(SearchText, out int searchId);
                Console.WriteLine($"isNumericSearch: {isNumericSearch}, searchId: {searchId}");

                var filtered = _vehicles.Where(vehicle =>
                        (isNumericSearch && vehicle.VehicleID.ToString().Contains(SearchText)) ||
                        (!isNumericSearch && vehicle.Brand.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                Console.WriteLine($"Found {filtered.Count} vehicles after filtering.");
                FilteredVehicles = new ObservableCollection<Vehicle>(filtered);
            }

            Console.WriteLine($"FilteredProducts count after filtering: {FilteredVehicles.Count}");
        }
        private void PerformSearch()
        {
            FilterVehicles();
        }

        // Commands
        public ICommand AddVehicleCommand { get; private set; }
        public ICommand EditVehicleCommand { get; private set; }
        public ICommand DeleteVehicleCommand { get; private set; }
        public ICommand SearchCommand { get; }
        public ICommand SearchBoxGotFocusCommand { get; }
        public ICommand SearchBoxLostFocusCommand { get; }

        //public EquipmentViewModel()
        //{
            //maybe delete this? not sure if code is working; I feel like it isn't
        //}


        public EquipmentViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;
            // Initialize commands
            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle, CanExecuteEditOrDelete);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle, CanExecuteEditOrDelete);
            SearchCommand = new RelayCommand((obj) => FilterVehicles());
            SearchBoxGotFocusCommand = new RelayCommand(SearchBoxGotFocus);
            SearchBoxLostFocusCommand = new RelayCommand(SearchBoxLostFocus);

            _vehicles = new ObservableCollection<Vehicle>();
            _filteredVehicles = new ObservableCollection<Vehicle>();

            InitializeLazyLoader();

        }
        private void InitializeLazyLoader()
        {
            _LazyVehiclesLoader = new Lazy<Task>(() => LoadVehiclesAsync(), isThreadSafe: true);
        }

        private async Task LoadVehiclesAsync()
        {
            try
            {

                // Replace PartDetailDto with Vehicle Details (create vehicle details?)
                var result = await _apiClient.GetAsync<OperationResult<IEnumerable<VehicleDto>>>("vehicle/all");
                if (result.IsSuccessful && result.Data != null)
                {
                    var vehicles = _mapper.Map<IEnumerable<Vehicle>>(result.Data);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _vehicles.Clear();
                        foreach (var vehicle in vehicles)
                        {
                            _vehicles.Add(vehicle);
                        }
                        FilterVehicles();
                    });
                }
                else
                {
                    // Log failure or handle accordingly
                    Debug.WriteLine("Failed to load vehicles or no vehicles returned.");
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Debug.WriteLine($"An error occurred while loading vehicles: {ex.Message}");
            }
        }
        public async Task EnsureVehiclesLoadedAsync()
        {
            if (!_LazyVehiclesLoader.IsValueCreated)
            {
                await _LazyVehiclesLoader.Value;
            }
        }
        private void AdjustSearchTextOnFocus()
        {
            if (_isSearchBoxFocused && SearchText == "Search Vehicles...")
            {
                SearchText = string.Empty;
            }
            else if (!_isSearchBoxFocused && string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Vehicles...";
            }
        }
        private void SearchBoxGotFocus(object obj)
        {
            if (SearchText == "Search Vehicles...")
            {
                SearchText = string.Empty;
            }
        }
        private void SearchBoxLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Vehicles...";
            }
        }
        private void AddVehicle(object obj)
        {
            var newVehicleForm = new VehicleFormViewModel(_apiClient, _mapper);
            newVehicleForm.VehicleUpdated += OnVehicleUpdated;
            _dialogService.ShowDialog(newVehicleForm);
            newVehicleForm.VehicleUpdated -= OnVehicleUpdated;
        }
        private void OnVehicleUpdated(object sender, EventArgs e)
        {
            RefreshVehicles(null);
        }

        private void EditVehicle(object obj)
        {
            if (SelectedVehicle != null)
            {
                var editVehicleFormViewModel = new VehicleFormViewModel(_apiClient, _mapper, SelectedVehicle);
                editVehicleFormViewModel.VehicleUpdated += OnVehicleUpdated;
                _dialogService.ShowDialog(editVehicleFormViewModel);
                editVehicleFormViewModel.Cleanup();
                editVehicleFormViewModel.VehicleUpdated -= OnVehicleUpdated;
            }
        }
        private bool CanExecuteEditOrDelete(object obj)
        {
            // Determine if the edit/delete command can execute, e.g., based on if a product is selected
            return true; // Example logic
        }
        private void DeleteVehicle(object obj)
        {
            // Implementation for deleting a selected vehicle
        }
        private void RefreshVehicles(object obj)
        {
            Refresh();
        }
        private void Refresh()
        {
            _ = LoadVehiclesAsync();
        }

    }
}