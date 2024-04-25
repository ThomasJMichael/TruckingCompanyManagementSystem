using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Data.Models;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using PurchaseOrder = TCMS.GUI.Models.PurchaseOrder;
using Shipment = TCMS.GUI.Models.Shipment;

namespace TCMS.GUI.ViewModels
{
    public class ShipmentsViewModel : Utilities.ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;

        public ShipmentsViewModel(IApiClient apiClient, IMapper mapper, IDialogService dialogService)
        {
            _apiClient = apiClient;
            _mapper = mapper;
            _dialogService = dialogService;

            LoadPurchaseOrders();
            LoadShipments();
        }

        private ObservableCollection<Shipment> _shipments;
        private ObservableCollection<Shipment> _filteredShipments;

        public ObservableCollection<Shipment> FilteredShipments
        {
            get => _filteredShipments;
            private set
            {
                _filteredShipments = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PurchaseOrder> _purchaseOrders;

        public ObservableCollection<PurchaseOrder> PurchaseOrders
        {
            get => _purchaseOrders;
            private set
            {
                _purchaseOrders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PurchaseOrder> _filteredPurchaseOrders;

        public ObservableCollection<PurchaseOrder> FilteredPurchaseOrders
        {
            get => _filteredPurchaseOrders;
            private set
            {
                _filteredPurchaseOrders = value;
                OnPropertyChanged();
            }
        }

        private PurchaseOrder _selectedPurchaseOrder;

        public PurchaseOrder SelectedPurchaseOrder
        {
            get { return _selectedPurchaseOrder; }
            set
            {
                _selectedPurchaseOrder = value;
                OnPropertyChanged();
            }
        }

        private Shipment _selectedShipment;

        public Shipment SelectedShipment
        {
            get { return _selectedShipment; }
            set
            {
                _selectedShipment = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PurchaseOrderItem> _purchaseOrderItemsResult;

        public ObservableCollection<PurchaseOrderItem> PurchaseOrderItemsResult
        {
            get => _purchaseOrderItemsResult;
            private set
            {
                _purchaseOrderItemsResult = value;
                OnPropertyChanged();
            }
        }

        private PurchaseOrderItem _selectedPurchaseOrderItem;

        public PurchaseOrderItem SelectedPurchaseOrderItem
        {
            get { return _selectedPurchaseOrderItem; }
            set
            {
                _selectedPurchaseOrderItem = value;
                OnPropertyChanged();
            }
        }

        private string _searchText = "Search Purchase Orders...";

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    UpdateSearchResults();
                }
            }
        }

        private void UpdateSearchResults()
        {
            switch (SelectedTabIndex)
            {
                case 0:
                    FilterPurchaseOrders();
                    break;
                case 1:
                    FilterShipments();
                    break;
            }
        }

        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                UpdateSearchText(value);
            }
        }

        private void UpdateSearchText(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    SearchText = "Search Purchase Orders...";
                    break;
                case 1:
                    SearchText = "Search Shipments...";
                    break;
            }
        }

        private void FilterPurchaseOrders()
        {
            if (_purchaseOrders == null)
            {
                LoadPurchaseOrders();
                return;
            }

            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Purchase Orders...")
            {
                FilteredPurchaseOrders = new ObservableCollection<PurchaseOrder>(_purchaseOrders);
            }
            else
            {
                var filtered = _purchaseOrders.Where(po =>
                        (po.Company.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.Address.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.City.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.State.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.Zip.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.ShippingCost.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.TotalCost.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.ShippingPaid.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         po.DateCreated.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                FilteredPurchaseOrders = new ObservableCollection<PurchaseOrder>(filtered);
            }
        }

        private void FilterShipments()
        {
            if (_shipments == null)
            {
                LoadShipments();
                return;
            }

            if (string.IsNullOrEmpty(SearchText) || SearchText == "Search Shipments...")
            {
                FilteredShipments = new ObservableCollection<Shipment>(_shipments);
            }
            else
            {
                var filtered = _shipments.Where(s =>
                        (s.ShipmentId.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         s.ArrivalDateTime.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         s.DepartureDateTime.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         s.EstimatedArrivalDateTime.ToString()
                             .IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                         s.ShipmentDirection.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                FilteredShipments = new ObservableCollection<Shipment>(filtered);
            }

        }

        private async void LoadPurchaseOrders()
        {
            try
            {
                var purchaseOrdersResult =
                    await _apiClient.GetAsync<OperationResult<IEnumerable<PurchaseOrderDto>>>("purchase-order/all");
                if (purchaseOrdersResult.IsSuccessful)
                {
                    var mappedPurchaseOrders = _mapper.Map<IEnumerable<PurchaseOrder>>(purchaseOrdersResult.Data);
                    PurchaseOrders = new ObservableCollection<PurchaseOrder>(mappedPurchaseOrders);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private async void LoadShipments()
        {
            try
            {
                var shipments = await _apiClient.GetAsync<OperationResult<IEnumerable<ShipmentDetailDto>>>("shipment/all");
                if (shipments.IsSuccessful)
                {
                    _shipments =
                        new ObservableCollection<Shipment>(shipments.Data.Select(s => _mapper.Map<Shipment>(s)));
                    FilterShipments();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
