using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AutoMapper;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.DTOs.Report;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;
using TCMS.GUI.Views;
using Equipment = TCMS.GUI.Models.Equipment;

namespace TCMS.GUI.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly IApiClient _apiClient;
        private readonly IDialogService _dialogService;

        private ObservableCollection<PayrollReport> _payrollReports;

        public ObservableCollection<PayrollReport> PayrollReports
        {
            get => _payrollReports;
            set
            {
                _payrollReports = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MaintenanceRecord> _maintenanceReports;

        public ObservableCollection<MaintenanceRecord> MaintenanceRecords
        {
            get => _maintenanceReports;
            set
            {
                _maintenanceReports = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Shipment> _incomingShipments;

        public ObservableCollection<Shipment> IncomingShipments
        {
            get => _incomingShipments;
            set
            {
                _incomingShipments = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Shipment> _outgoingShipments;

        public ObservableCollection<Shipment> OutgoingShipments
        {
            get => _outgoingShipments;
            set
            {
                _outgoingShipments = value;
                OnPropertyChanged();
            }
        }

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

        private int? _selectedVehicleId;
        public int? SelectedVehicleId
        {
            get => _selectedVehicleId;
            set
            {
                _selectedVehicleId = value;
                OnPropertyChanged(nameof(SelectedVehicleId));
            }
        }
        public ObservableCollection<MaintenanceRecord> SelectedVehicleMaintenanceReports { get; } = new ObservableCollection<MaintenanceRecord>();
        public ICommand GenerateVehicleReportCommand { get; }
        public ReportViewModel(IDialogService dialogService, IMapper mapper, IApiClient apiClient)
        {
            _dialogService = dialogService;
            _apiClient = apiClient;
            _mapper = mapper;

            LoadPayrollReports();
            LoadMaintenanceRecords();
            LoadIncomingShipments();
            LoadOutgoingShipments();
            _ = LoadEquipmentAsync();

            GenerateVehicleReportCommand = new RelayCommand(GenerateVehicleReport);
        }

        private void GenerateVehicleReport(object obj)
        {
            if (SelectedVehicleId.HasValue)
            {
                var selectedVehicle = _filteredEquipment.FirstOrDefault(v => v.VehicleId == SelectedVehicleId.Value);
                if (selectedVehicle != null)
                {
                    SelectedVehicleMaintenanceReports.Clear();
                    foreach (var record in selectedVehicle.MaintenanceRecords)
                    {
                        if (record.RecordType is not (RecordType.Maintenance and RecordType.Inspection))
                        {
                            record.RecordType = RecordType.Maintenance;
                        }
                        SelectedVehicleMaintenanceReports.Add(record);
                    }
                }
            }
        }

        private async void LoadPayrollReports()
        {
            var reportReqDto = new ReportRequestDto
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now
            };
            try
            {
                var result =
                    await _apiClient.PostAsync<OperationResult<IEnumerable<PayrollReportDto>>>("reports/payroll",
                        reportReqDto);
                if (result.IsSuccessful)
                {
                    PayrollReports =
                        new ObservableCollection<PayrollReport>(_mapper.Map<IEnumerable<PayrollReport>>(result.Data));
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

        }
        private async void LoadMaintenanceRecords()
        {
            var reportReqDto = new ReportRequestDto
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now
            };
            try
            {
                var result =
                    await _apiClient.PostAsync<OperationResult<IEnumerable<MaintenanceRecordDto>>>("reports/maintenance", reportReqDto);
                if (result.IsSuccessful)
                {
                    MaintenanceRecords =
                        new ObservableCollection<MaintenanceRecord>(_mapper.Map<IEnumerable<MaintenanceRecord>>(result.Data));
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

        }
        private async void LoadIncomingShipments()
        {
            try
            {
                var result =
                    await _apiClient.GetAsync<OperationResult<IEnumerable<IncomingShipmentReportDto>>>("reports/incoming-shipments");
                if (result.IsSuccessful)
                {
                    IncomingShipments =
                        new ObservableCollection<Shipment>(_mapper.Map<IEnumerable<Shipment>>(result.Data));
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

        }
        private async void LoadOutgoingShipments()
        {
            try
            {
                var result =
                    await _apiClient.GetAsync<OperationResult<IEnumerable<OutgoingShipmentReportDto>>>("reports/outgoing-shipments");
                if (result.IsSuccessful)
                {
                    OutgoingShipments =
                        new ObservableCollection<Shipment>(_mapper.Map<IEnumerable<Shipment>>(result.Data));
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

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
                        FilteredEquipment = new ObservableCollection<Equipment>(mappedEquipment);
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
    }

}
