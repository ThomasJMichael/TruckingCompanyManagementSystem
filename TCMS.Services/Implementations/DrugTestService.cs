using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.DrugTestDto;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class DrugTestService : IDrugTestService
    {
        private readonly TcmsContext _context;
        public async Task<OperationResult> CreateTestAsync(DrugTestCreateDto drugTestDto)
        {
            try
            {
                var drugTest = new DrugAndAlcoholTest
                {
                    DriverId = drugTestDto.DriverId,
                    TestType = drugTestDto.TestType,
                    TestResult = drugTestDto.TestResult,
                    TestDetails = drugTestDto.TestDetails,
                    TestDate = drugTestDto.TestDate,
                    IncidentReportId = drugTestDto.IncidentReportId,
                    FollowUpTestDate = drugTestDto.FollowUpTestDate
                };
                _context.DrugAndAlcoholTests.Add(drugTest);
                await _context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<IEnumerable<DrugTestDto>>> GetAllTestsAsync()
        {
            try
            {
                var tests = await _context.DrugAndAlcoholTests
                    .Include(t => t.Driver)
                    .Select(t => new DrugTestDto
                    {
                        DrugAndAlcoholTestId = t.DrugAndAlcoholTestId,
                        DriverId = t.DriverId,
                        TestType = t.TestType,
                        TestResult = t.TestResult,
                        TestDetails = t.TestDetails,
                        TestDate = t.TestDate,
                        IncidentReportId = t.IncidentReportId,
                        FollowUpTestDate = t.FollowUpTestDate,
                        IsFollowUpComplete = t.IsFollowUpComplete
                    })
                    .ToListAsync();

                return OperationResult<IEnumerable<DrugTestDto>>.Success(tests);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<DrugTestDto>>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<DrugTestDto>> GetTestByIdAsync(int drugTestId)
        {
            try
            {
                var test = await _context.DrugAndAlcoholTests.FindAsync(drugTestId);
                if (test == null) return OperationResult<DrugTestDto>.Failure(["Test not found."]);

                var testDto = new DrugTestDto
                {
                    DrugAndAlcoholTestId = test.DrugAndAlcoholTestId,
                    DriverId = test.DriverId,
                    TestType = test.TestType,
                    TestResult = test.TestResult,
                    TestDetails = test.TestDetails,
                    TestDate = test.TestDate,
                    IncidentReportId = test.IncidentReportId,
                    FollowUpTestDate = test.FollowUpTestDate,
                    IsFollowUpComplete = test.IsFollowUpComplete
                };
                return OperationResult<DrugTestDto>.Success(testDto);
            }
            catch (Exception e)
            {
                return OperationResult<DrugTestDto>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> UpdateTestAsync(DrugTestUpdateDto drugTestDto)
        {
            var test = await _context.DrugAndAlcoholTests.FindAsync(drugTestDto.DrugAndAlcoholTestId);
            if (test == null) return OperationResult.Failure(["Test not found."]);

            test.TestDate = drugTestDto.TestDate;
            test.TestType = drugTestDto.TestType;
            test.TestResult = drugTestDto.TestResult;
            test.TestDetails = drugTestDto.TestDetails;
            test.IncidentReportId = drugTestDto.IncidentReportId;
            test.FollowUpTestDate = drugTestDto.FollowUpTestDate;
            test.IsFollowUpComplete = drugTestDto.IsFollowUpComplete;

            try
            {
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> DeleteTestAsync(int drugTestId)
        {
            var test = await _context.DrugAndAlcoholTests.FindAsync(drugTestId);
            if (test == null) return OperationResult.Failure(["Test not found."]);

            try
            {
                _context.DrugAndAlcoholTests.Remove(test);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<IEnumerable<DrugTestDto>>> GetTestsByDriverIdAsync(int driverId)
        {
            try
            {
                var tests = await _context.DrugAndAlcoholTests
                    .Where(test => test.DriverId == driverId)
                    .ToListAsync();

                var testDtos = tests.Select(t => new DrugTestDto
                {
                    DrugAndAlcoholTestId = t.DrugAndAlcoholTestId,
                    DriverId = t.DriverId,
                    TestType = t.TestType,
                    TestResult = t.TestResult,
                    TestDetails = t.TestDetails,
                    TestDate = t.TestDate,
                    IncidentReportId = t.IncidentReportId,
                    FollowUpTestDate = t.FollowUpTestDate,
                    IsFollowUpComplete = t.IsFollowUpComplete
                });

                return OperationResult<IEnumerable<DrugTestDto>>.Success(testDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<DrugTestDto>>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> ScheduleFollowUpTestAsync(int drugTestId, DateTime followUpDate)
        {
            try
            {
                var test = await _context.DrugAndAlcoholTests.FindAsync(drugTestId);
                if (test == null) return OperationResult.Failure(["Test not found."]);

                if (followUpDate < DateTime.Now) return OperationResult.Failure(["Follow-up date must be in the future."]);

                test.FollowUpTestDate = followUpDate;
                test.IsFollowUpComplete = false;

                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> CompleteFollowUpTestAsync(int drugTestId, TestResult result)
        {
            try
            {
                var test = await _context.DrugAndAlcoholTests.FindAsync(drugTestId);
                if (test == null) return OperationResult.Failure(["Test not found."]);

                if (test.FollowUpTestDate == null) return OperationResult.Failure(["No follow-up test scheduled."]);

                test.TestResult = result;
                test.IsFollowUpComplete = true;

                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }
    }
}
