using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class IncidentService(TcmsContext context, IMapper mapper) : IIncidentService
    {
        public async Task<OperationResult<IncidentReportDto>> CreateIncidentReportAsync(IncidentReportDto incidentReportDto)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var incidentReport = mapper.Map<IncidentReport>(incidentReportDto);

                context.IncidentReports.Add(incidentReport);
                await context.SaveChangesAsync();

                // If the incident requires a drug and alcohol test, create a new test record
                if (incidentReport.RequiresDrugAndAlcoholTest)
                {
                    var drugAndAlcoholTest = mapper.Map<DrugAndAlcoholTest>(incidentReportDto);

                    context.DrugAndAlcoholTests.Add(drugAndAlcoholTest);
                    await context.SaveChangesAsync();

                    incidentReport.DrugAndAlcoholTestId = drugAndAlcoholTest.DrugAndAlcoholTestId;
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                // Create a new DTO to return the new incident report
                var newIncidentReportDto = mapper.Map<IncidentReportDto>(incidentReport);

                return OperationResult<IncidentReportDto>.Success(newIncidentReportDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult<IncidentReportDto>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult> UpdateIncidentReportAsync(IncidentReportDto incidentReportDto)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var incidentReport = await context.IncidentReports.FindAsync(incidentReportDto.IncidentReportId);
                if (incidentReport == null) return OperationResult.Failure(new[] { "Incident report not found" });

                mapper.Map(incidentReportDto, incidentReport);

                // Update or create drug and alcohol test if required
                if (incidentReport.RequiresDrugAndAlcoholTest())
                {
                    DrugAndAlcoholTest drugAndAlcoholTest;
                    if (incidentReport.DrugAndAlcoholTestId.HasValue)
                    {
                        // Update existing test
                        drugAndAlcoholTest = await context.DrugAndAlcoholTests.FindAsync(incidentReport.DrugAndAlcoholTestId.Value);
                    }
                    else
                    {
                        // Create a new test
                        drugAndAlcoholTest = new DrugAndAlcoholTest
                        {
                            DriverId = incidentReport.DriverId,
                            TestDate = DateTime.Now, // Assuming the test date is now
                            TestType = TestType.PostAccident, 
                            IncidentReportId = incidentReport.IncidentReportId
                        };
                        context.DrugAndAlcoholTests.Add(drugAndAlcoholTest);
                    }

                    // Link the test to the incident report
                    incidentReport.DrugAndAlcoholTestId = drugAndAlcoholTest.DrugAndAlcoholTestId;
                }
                else if (incidentReport.DrugAndAlcoholTestId.HasValue)
                {
                    var existingTest = await context.DrugAndAlcoholTests.FindAsync(incidentReport.DrugAndAlcoholTestId.Value);
                    context.DrugAndAlcoholTests.Remove(existingTest);
                    incidentReport.DrugAndAlcoholTestId = null; // Unlink the test from the incident report
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new[] { ex.Message });
            }
        }


        public async Task<OperationResult> DeleteIncidentReportAsync(int incidentReportId)
        {
            var incidentReport = await context.IncidentReports.FindAsync(incidentReportId);
            if (incidentReport == null) return OperationResult.Failure(new[] { "Incident report not found" });

            try
            {
                context.IncidentReports.Remove(incidentReport);
                await context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(new[] { ex.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<IncidentReportDto>>> GetAllIncidentReportsAsync()
        {
            try
            {
                var incidentReports = await context.IncidentReports
                    .Include(incident => incident.Driver)
                    .ToListAsync();

                var incidentReportsDto = mapper.Map<IEnumerable<IncidentReportDto>>(incidentReports);
                return OperationResult<IEnumerable<IncidentReportDto>>.Success(incidentReportsDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<IncidentReportDto>>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult<IncidentReportDto>> GetIncidentReportByIdAsync(int incidentReportId)
        {
            try
            {
                var incidentReport = await context.IncidentReports.FindAsync(incidentReportId);
                if (incidentReport == null)
                    return OperationResult<IncidentReportDto>.Failure(["Incident report not found."]);

                var incidentReportDto = mapper.Map<IncidentReportDto>(incidentReport);
                return OperationResult<IncidentReportDto>.Success(incidentReportDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IncidentReportDto>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult> EvaluateAndInitiateDrugTestForIncidentAsync(int incidentReportId, DateTime? testDate = null)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var incidentReport = await context.IncidentReports.FindAsync(incidentReportId);
                if (incidentReport == null)
                {
                    return OperationResult.Failure(new List<string> { "Incident report not found." });
                }

                // Evaluate the need for a drug and alcohol test
                if (incidentReport.RequiresDrugAndAlcoholTest)
                {
                    var drugAndAlcoholTest = new DrugAndAlcoholTest
                    {
                        DriverId = incidentReport.DriverId,
                        TestDate = testDate ?? DateTime.UtcNow,
                        TestType = TestType.PostAccident,

                    };

                    context.DrugAndAlcoholTests.Add(drugAndAlcoholTest);
                    await context.SaveChangesAsync();

                    // Update the incident report with the test ID
                    incidentReport.DrugAndAlcoholTestId = drugAndAlcoholTest.DrugAndAlcoholTestId;
                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return OperationResult.Failure(new List<string> { ex.Message });
            }
        }

    }
}
