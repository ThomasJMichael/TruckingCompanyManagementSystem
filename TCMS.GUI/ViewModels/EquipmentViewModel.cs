using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.GUI.Views;
using Equipment = TCMS.GUI.Models.Equipment;

namespace TCMS.GUI.ViewModels
{
    public class EquipmentViewModel : Utilities.ViewModelBase
    {
        // Example property representing a list of products
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        private Lazy<Task> _LazyIncidentLoader;

        private ObservableCollection<Equipment> _equipment;
        private ObservableCollection<Equipment> _filteredEquipment;

        public ObservableCollection<Equipment> FilteredEquipment
        {
            get => _filteredEquipment;
            private set
            {
                _filteredEquipment = value;
                OnPropertyChanged();
            }
        }

        private Equipment _selectedEquipment;

        private string _searchText = "Search Equipment...";

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged();
            }
        }
        private MaintenanceRecord _selectedRecord;

        public MaintenanceRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Try to ensure the products are loaded before filtering.
                _ = EnsureIncidentsLoadedAsync().ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        // Products are loaded, perform the filter.
                        FilterEquipments();
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



        private void FilterEquipments()
        {
            Console.WriteLine("FilterProducts called");

            if (_equipment == null)
            {
                Console.WriteLine("Products collection is null, loading products.");
                EnsureIncidentsLoadedAsync();
                return;
            }

            Console.WriteLine($"Current SearchText: '{SearchText}'");
            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Equipment...")
            {
                Console.WriteLine("SearchText is empty or placeholder text, setting filteredProducts to all products.");
                FilteredEquipment = new ObservableCollection<Equipment>(_equipment);
            }
            else
            {
                Console.WriteLine("Searching...");
                // Determine if search text is purely numeric
                bool isNumericSearch = int.TryParse(SearchText, out int searchId);
                Console.WriteLine($"isNumericSearch: {isNumericSearch}, searchId: {searchId}");

                var filtered = _equipment.Where(equipment =>
                    (isNumericSearch && equipment.VehicleId.ToString().Contains(SearchText)) ||
                    (isNumericSearch && equipment.Brand.ToString().Contains(SearchText)) ||
                    (!isNumericSearch && equipment.Model.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                  .ToList();

                Console.WriteLine($"Found {filtered.Count} products after filtering.");
                FilteredEquipment = new ObservableCollection<Equipment>(filtered);
            }

            Console.WriteLine($"FilteredProducts count after filtering: {FilteredEquipment.Count}");
        }


        private void PerformSearch()
        {
            FilterEquipments();
        }

        // Commands
        public ICommand AddEquipmentCommand { get; private set; }
        public ICommand EditEquipmentCommand { get; private set; }
        public ICommand DeleteEquipmentCommand { get; private set; }
        public ICommand AddRecordCommand { get; private set; }
        public ICommand EditRecordCommand { get; private set; }
        public ICommand DeleteRecordCommand { get; private set; }
        public ICommand RefreshEquipmentsCommand { get; private set; }
        public ICommand SearchCommand { get; }
        public ICommand SearchBoxGotFocusCommand { get; }
        public ICommand SearchBoxLostFocusCommand { get; }

        public EquipmentViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;
            // Initialize commands
            AddEquipmentCommand = new RelayCommand(AddEquipment);
            EditEquipmentCommand = new RelayCommand(EditEquipment, CanExecuteEditOrDelete);
            AddRecordCommand = new RelayCommand(AddRecord);
            EditRecordCommand = new RelayCommand(UpdateRecord);
            DeleteEquipmentCommand = new RelayCommand(DeleteEquipment, CanExecuteEditOrDelete);
            RefreshEquipmentsCommand = new RelayCommand(RefreshEquipments);
            SearchCommand = new RelayCommand((obj) => FilterEquipments());
            SearchBoxGotFocusCommand = new RelayCommand(SearchBoxGotFocus);
            SearchBoxLostFocusCommand = new RelayCommand(SearchBoxLostFocus);

            _equipment = new ObservableCollection<Equipment>();
            _filteredEquipment = new ObservableCollection<Equipment>();

            InitializeLazyLoader();

        }

        private void InitializeLazyLoader()
        {
            _LazyIncidentLoader = new Lazy<Task>(() => LoadEquipmentAsync(), isThreadSafe: true);
        }

        private async Task LoadEquipmentAsync()
        {
            try
            {
                var result = await _apiClient.GetAsync<OperationResult<IEnumerable<VehicleDto>>>("vehicle/all");
                if (result.IsSuccessful && result.Data != null)
                {
                    var mappedEquipment = _mapper.Map<IEnumerable<Equipment>>(result.Data);

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _equipment.Clear();
                        foreach (var equipment in mappedEquipment)
                        {
                            _equipment.Add(equipment);
                        }
                        FilterEquipments();
                    });
                }
                else
                {
                    Debug.WriteLine("Failed to load equipment or no data returned.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while loading equipment: {ex.Message}");
            }
        }


        public async Task EnsureIncidentsLoadedAsync()
        {
            if (!_LazyIncidentLoader.IsValueCreated)
            {
                await _LazyIncidentLoader.Value;
            }
        }

        private void AdjustSearchTextOnFocus()
        {
            if (_isSearchBoxFocused && SearchText == "Search Equipment...")
            {
                SearchText = string.Empty;
            }
            else if (!_isSearchBoxFocused && string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Equipment...";
            }
        }

        private void SearchBoxGotFocus(object obj)
        {
            if (SearchText == "Search Equipment...")
            {
                SearchText = string.Empty;
            }
        }

        private void SearchBoxLostFocus(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = "Search Equipment...";
            }
        }


        private void AddEquipment(object obj)
        {
            var newEquipmentForm = new EquipmentFormViewModel(_apiClient, _mapper);
            newEquipmentForm.EquipmentUpdated += OnEquipmentUpdated;
            _dialogService.ShowDialog(newEquipmentForm);
            newEquipmentForm.EquipmentUpdated -= OnEquipmentUpdated;
        }

        private void OnEquipmentUpdated(object sender, EventArgs e)
        {
            Refresh();
        }


        private void Refresh()
        {
            _ = LoadEquipmentAsync();
        }

        private void EditEquipment(object obj)
        {
            if (SelectedEquipment != null)
            {
                var editEquipmentForm = new EquipmentFormViewModel(_apiClient, _mapper, SelectedEquipment);
                editEquipmentForm.EquipmentUpdated += OnEquipmentUpdated;
                _dialogService.ShowDialog(editEquipmentForm);
                editEquipmentForm.Cleanup();
                editEquipmentForm.EquipmentUpdated -= OnEquipmentUpdated;
            }
        }

        private bool CanExecuteEditOrDelete(object obj)
        {
            // Determine if the edit/delete command can execute, e.g., based on if a product is selected
            return true; // Example logic
        }

        private void DeleteEquipment(object obj)
        {
            // Implementation for deleting a selected product
        }

        private void RefreshEquipments(object obj)
        {
            Refresh();
        }

        private void AddRecord(object obj)
        {
            var newRecordForm = new MaintenanceFormViewModel(_apiClient, _dialogService, _mapper);
            newRecordForm.RecordsUpdated += OnRecordsUpdated;
            _dialogService.ShowDialog(newRecordForm);
            newRecordForm.RecordsUpdated -= OnRecordsUpdated;
        }

        private void UpdateRecord(object obj)
        {
            if (SelectedEquipment != null)
            {
                var editRecordForm = new MaintenanceFormViewModel(_apiClient, _dialogService, _mapper, SelectedRecord);
                editRecordForm.RecordsUpdated += OnRecordsUpdated;
                _dialogService.ShowDialog(editRecordForm);
                editRecordForm.RecordsUpdated -= OnRecordsUpdated;
            }
        }

        private void OnRecordsUpdated(object sender, EventArgs e)
        {
            RefreshEquipments(null);
        }
    }
}

