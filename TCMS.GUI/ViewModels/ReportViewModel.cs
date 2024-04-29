using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AutoMapper;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.DTOs.Report;
using TCMS.Common.Operations;
using TCMS.GUI.Models;
using TCMS.GUI.Services.Interfaces;
using TCMS.GUI.Utilities;

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

        public ReportViewModel(IDialogService dialogService, IMapper mapper, IApiClient apiClient)
        {
            _dialogService = dialogService;
            _apiClient = apiClient;
            _mapper = mapper;

            LoadPayrollReports();
            LoadMaintenanceRecords();
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
    }

}
