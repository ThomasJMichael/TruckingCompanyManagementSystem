using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

namespace TCMS.GUI.ViewModels
{
    public class PartsManagementFormViewModel : Utilities.ViewModelBase, IDialogRequestClose
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        
        private ObservableCollection<Part> _displayedParts = [];
        public ObservableCollection<Part> DisplayedParts
        {
            get => _displayedParts;
            set
            {
                _displayedParts = value;
                OnPropertyChanged();
            }
        }
        private List<PartDetailDto> AddedParts { get; set; } = [];
        private List<PartDetailDto> RemovedParts { get; set; } = [];
        private List<PartDetailDto> UpdatedParts { get; set; } = [];

        private int _partDetailId;
        private string _partName;
        private string _partNumber;
        private decimal _cost;
        private string _supplier;
        private bool _isFromStock;
        private int _quantity;

        public int PartDetailId
        {
            get => _partDetailId;
            set { _partDetailId = value; OnPropertyChanged(); }
        }
        public string PartName
        {
            get => _partName;
            set { _partName = value; OnPropertyChanged(); }
        }

        public string PartNumber
        {
            get => _partNumber;
            set { _partNumber = value; OnPropertyChanged(); }
        }

        public decimal Cost
        {
            get => _cost;
            set { _cost = value; OnPropertyChanged(); }
        }

        public string Supplier
        {
            get => _supplier;
            set { _supplier = value; OnPropertyChanged(); }
        }

        public bool IsFromStock
        {
            get => _isFromStock;
            set { _isFromStock = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        private Part _selectedPart;
        public Part SelectedPart
        {
            get => _selectedPart;
            set
            {
                _selectedPart = value;
                OnPropertyChanged();
                if (_selectedPart != null)
                {
                    PartDetailId = _selectedPart.PartDetailId;
                    PartName = _selectedPart.PartName;
                    PartNumber = _selectedPart.PartNumber;
                    Cost = _selectedPart.Cost;
                    Supplier = _selectedPart.Supplier;
                    Quantity = _selectedPart.QuantityOnHand;
                    IsFromStock = _selectedPart.IsFromStock;
                }
            }
        }
        private int _maintenanceRecordId;

        public int MaintenanceRecordId
        {
            get => _maintenanceRecordId;
            set
            {
                _maintenanceRecordId = value;
                OnPropertyChanged();
            }
        }


        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
        public event EventHandler PartsUpdated;
        public ICommand NewPartCommand { get; private set; }
        public ICommand AddPartCommand { get; private set; }
        public ICommand UpdatePartCommand { get; private set; }
        public ICommand DeletePartCommand { get; private set; }
        public ICommand ConfirmCommand { get; private set; }

        public PartsManagementFormViewModel(IApiClient apiClient, IMapper mapper, int maintenanceRecordId)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _maintenanceRecordId = maintenanceRecordId;
            NewPartCommand = new RelayCommand(NewPart);
            AddPartCommand = new RelayCommand(AddPart);
            UpdatePartCommand = new RelayCommand(UpdatePart);
            DeletePartCommand = new RelayCommand(DeletePart);
            ConfirmCommand = new RelayCommand(Confirm);

            LoadParts();
        }

        private async void LoadParts()
        {
            try
            {
                var result = await _apiClient.GetAsync<OperationResult<IEnumerable<PartDetailDto>>>($"maintenance/all-parts/{MaintenanceRecordId}");
                if (result.IsSuccessful)
                {
                    var mappedParts = _mapper.Map<IEnumerable<Part>>(result.Data);
                    DisplayedParts = new ObservableCollection<Part>(mappedParts);
                }
                else
                {
                    Debug.WriteLine(result.Errors);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void NewPart(object obj)
        {
            ClearFields();
        }

        private async void Confirm(object obj)
        {

            if ((AddedParts.Count > 0 || RemovedParts.Count > 0 || UpdatedParts.Count > 0))
            {
                var parts = new PartsDto()
                {
                    AddedParts = AddedParts,
                    RemovedParts = RemovedParts,
                    UpdatedParts = UpdatedParts
                };
                var result = await _apiClient.PostAsync<OperationResult>($"maintenance/update-parts/{MaintenanceRecordId}", parts);
                if (result.IsSuccessful)
                {
                    PartsUpdated?.Invoke(this, EventArgs.Empty);
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                }
                else
                {
                    Debug.WriteLine(result.Messages);
                }
            }
            else
            {
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
            }
        }

        private void DeletePart(object obj)
        {
            var deletedPart = new PartDetailDto()
            {
                PartDetailId = PartDetailId,
            };

            RemovedParts.Add(deletedPart);
            DisplayedParts.Remove(SelectedPart);
            ClearFields();
        }

        private void UpdatePart(object obj)
        {
            var updatedPart = new PartDetailDto()
            {
                PartDetailId = PartDetailId,
                PartName = PartName,
                PartNumber = PartNumber,
                Cost = Cost,
                Supplier = Supplier,
                IsFromStock = IsFromStock,
                QuantityOnHand = Quantity,
                MaintenanceRecordID = MaintenanceRecordId
                };
            var displayPart = new Part()
            {
                PartDetailId = PartDetailId,
                PartName = PartName,
                PartNumber = PartNumber,
                Cost = Cost,
                Supplier = Supplier,
                IsFromStock = IsFromStock,
                QuantityOnHand = Quantity,
                MaintenanceRecordId = MaintenanceRecordId
            };

            UpdatedParts.Add(updatedPart);
            DisplayedParts.Remove(SelectedPart);
            DisplayedParts.Add(displayPart);
            ClearFields();
        }
        

        private void AddPart(object obj)
        {
            var newPart = new PartDetailDto()
            {
                PartName = PartName,
                PartNumber = PartNumber,
                Cost = Cost,
                Supplier = Supplier,
                IsFromStock = IsFromStock,
                QuantityOnHand = Quantity,
                MaintenanceRecordID = MaintenanceRecordId
            };

            var displayPart = new Part()
            {
                PartName = PartName,
                PartNumber = PartNumber,
                Cost = Cost,
                Supplier = Supplier,
                IsFromStock = IsFromStock,
                QuantityOnHand = Quantity,
                MaintenanceRecordId = MaintenanceRecordId
            };

            AddedParts.Add(newPart);
            DisplayedParts.Add(displayPart);
            ClearFields();
        }

        private void ClearFields()
        {
            PartName = "";
            PartNumber = "";
            Cost = 0;
            Supplier = "";
            IsFromStock = false;
            Quantity = 0;
        }
    }
}

