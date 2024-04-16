using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.enums;
using TCMS.Data.Models;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces
{
    public interface IDrugTestService
    {
        Task<OperationResult> CreateTestAsync(DrugTestCreateDto drugTestDto);
        Task<OperationResult<IEnumerable<DrugTestDto>>> GetAllTestsAsync();
        Task<OperationResult<DrugTestDto>> GetTestByIdAsync(int drugTestId);
        Task<OperationResult> UpdateTestAsync(DrugTestUpdateDto drugTestDto);
        Task<OperationResult> DeleteTestAsync(int drugTestId);

        // Specialized operations
        Task<OperationResult<IEnumerable<DrugTestDto>>> GetTestsByEmployeeId (int employeeId);
        Task<OperationResult> ScheduleFollowUpTestAsync(int drugTestId, DateTime followUpDate);
        Task<OperationResult> CompleteFollowUpTestAsync(int drugTestId, TestResult result);
    }
}
