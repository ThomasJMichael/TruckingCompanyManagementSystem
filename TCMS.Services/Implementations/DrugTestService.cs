using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class DrugTestService(TcmsContext context, IMapper mapper) : IDrugTestService
    {
        public async Task<OperationResult> CreateTestAsync(DrugTestCreateDto drugTestDto)
        {
            try
            {
                var drugTest = mapper.Map<DrugAndAlcoholTest>(drugTestDto);
                context.DrugAndAlcoholTests.Add(drugTest);
                await context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<DrugTestDto>>> GetAllTestsAsync()
        {
            try
            {
                var tests = await context.DrugAndAlcoholTests
                    .Include(t => t.Driver)
                    .ToListAsync();

                var testDtos = mapper.Map<IEnumerable<DrugTestDto>>(tests);

                return OperationResult<IEnumerable<DrugTestDto>>.Success(testDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<DrugTestDto>>.Failure(new List<string> { e.Message });
            }
        }


    public async Task<OperationResult<DrugTestDto>> GetTestByIdAsync(int drugTestId)
        {
            try
            {
                var test = await context.DrugAndAlcoholTests.FindAsync(drugTestId);
                if (test == null) return OperationResult<DrugTestDto>.Failure(["Test not found."]);

                var testDto = mapper.Map<DrugTestDto>(test);
                return OperationResult<DrugTestDto>.Success(testDto);
            }
            catch (Exception e)
            {
                return OperationResult<DrugTestDto>.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> UpdateTestAsync(DrugTestUpdateDto drugTestDto)
        {
            var test = await context.DrugAndAlcoholTests.FindAsync(drugTestDto.DrugAndAlcoholTestId);
            if (test == null) return OperationResult.Failure(["Test not found."]);

            mapper.Map(drugTestDto, test);

            try
            {
                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult> DeleteTestAsync(int drugTestId)
        {
            var test = await context.DrugAndAlcoholTests.FindAsync(drugTestId);
            if (test == null) return OperationResult.Failure(["Test not found."]);

            try
            {
                context.DrugAndAlcoholTests.Remove(test);
                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }

        public async Task<OperationResult<IEnumerable<DrugTestDto>>> GetTestsByEmployeeId (int employeeId)
        {
            try
            {
                var tests = await context.DrugAndAlcoholTests
                    .Where(test => test.EmployeeId.Equals(employeeId))
                    .ToListAsync();

                var testDtos = mapper.Map<IEnumerable<DrugTestDto>>(tests);

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
                var test = await context.DrugAndAlcoholTests.FindAsync(drugTestId);
                if (test == null) return OperationResult.Failure(["Test not found."]);

                if (followUpDate < DateTime.Now) return OperationResult.Failure(["Follow-up date must be in the future."]);

                test.FollowUpTestDate = followUpDate;
                test.IsFollowUpComplete = false;

                await context.SaveChangesAsync();
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
                var test = await context.DrugAndAlcoholTests.FindAsync(drugTestId);
                if (test == null) return OperationResult.Failure(["Test not found."]);

                if (test.FollowUpTestDate == null) return OperationResult.Failure(["No follow-up test scheduled."]);

                test.TestResult = result;
                test.IsFollowUpComplete = true;

                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure([e.Message]);
            }
        }
    }
}
