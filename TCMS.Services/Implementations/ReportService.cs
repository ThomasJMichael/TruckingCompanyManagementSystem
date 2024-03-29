using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DTOs.Report;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class ReportService : IReportService
    {
        public async Task<OperationResult<CsvResultDto>> GeneratePayrollReport(ReportRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<CsvResultDto>> GenerateMaintenanceReport(ReportRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<CsvResultDto>> GenerateVehicleMaintenanceReport(ReportRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<CsvResultDto>> GenerateIncomingShipmentsReport(ReportRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<CsvResultDto>> GenerateOutgoingShipmentsReport(ReportRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
