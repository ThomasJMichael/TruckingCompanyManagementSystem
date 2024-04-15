using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class IncidentService : IIncidentService
    {
        private readonly TcmsContext _context;
        private readonly IMapper _mapper;

        public IncidentService(TcmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OperationResult<IncidentReportDto>> CreateIncidentReportAsync(IncidentReportDto incidentReportDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var incidentReport = _mapper.Map<IncidentReport>(incidentReportDto);

                _context.IncidentReports.Add(incidentReport);
                await _context.SaveChangesAsync();

                // If the incident requires a drug and alcohol test, create a new test record
                if (incidentReport.RequiresDrugAndAlcoholTest)
                {
                    var drugAndAlcoholTest = _mapper.Map<DrugAndAlcoholTest>(incidentReportDto);

                    _context.DrugAndAlcoholTests.Add(drugAndAlcoholTest);
                    await _context.SaveChangesAsync();

                    incidentReport.DrugAndAlcoholTestId = drugAndAlcoholTest.DrugAndAlcoholTestId;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                // Create a new DTO to return the new incident report
                var newIncidentReportDto = _mapper.Map<IncidentReportDto>(incidentReport);

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
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var incidentReport = await _context.IncidentReports.FindAsync(incidentReportDto.IncidentReportId);
                if (incidentReport == null) return OperationResult.Failure(new[] { "Incident report not found" });

                _mapper.Map(incidentReportDto, incidentReport);

                // Update or create drug and alcohol test if required
                if (incidentReport.RequiresDrugAndAlcoholTest)
                {
                    DrugAndAlcoholTest drugAndAlcoholTest;
                    if (incidentReport.DrugAndAlcoholTestId.HasValue)
                    {
                        // Update existing test
                        drugAndAlcoholTest = await _context.DrugAndAlcoholTests.FindAsync(incidentReport.DrugAndAlcoholTestId.Value);
                    }
                    else
                    {
                        // Create a new test
                        drugAndAlcoholTest = new DrugAndAlcoholTest
                        {
                            EmployeeId = incidentReport.EmployeeId,
                            TestDate = DateTime.Now, // Assuming the test date is now
                            TestType = TestType.PostAccident, 
                            IncidentReportId = incidentReport.IncidentReportId
                        };
                        _context.DrugAndAlcoholTests.Add(drugAndAlcoholTest);
                    }

                    // Link the test to the incident report
                    incidentReport.DrugAndAlcoholTestId = drugAndAlcoholTest.DrugAndAlcoholTestId;
                }
                else if (incidentReport.DrugAndAlcoholTestId.HasValue)
                {
                    var existingTest = await _context.DrugAndAlcoholTests.FindAsync(incidentReport.DrugAndAlcoholTestId.Value);
                    _context.DrugAndAlcoholTests.Remove(existingTest);
                    incidentReport.DrugAndAlcoholTestId = null; // Unlink the test from the incident report
                }

                await _context.SaveChangesAsync();
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
            var incidentReport = await _context.IncidentReports.FindAsync(incidentReportId);
            if (incidentReport == null) return OperationResult.Failure(new[] { "Incident report not found" });

            try
            {
                _context.IncidentReports.Remove(incidentReport);
                await _context.SaveChangesAsync();
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
                var incidentReports = await _context.IncidentReports
                    //.Include(incident => incident.Driver)
                    .ToListAsync();

                var incidentReportsDto = _mapper.Map<IEnumerable<IncidentReportDto>>(incidentReports);
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
                var incidentReport = await _context.IncidentReports.FindAsync(incidentReportId);
                if (incidentReport == null)
                    return OperationResult<IncidentReportDto>.Failure(["Incident report not found."]);

                var incidentReportDto = _mapper.Map<IncidentReportDto>(incidentReport);
                return OperationResult<IncidentReportDto>.Success(incidentReportDto);
            }
            catch (Exception ex)
            {
                return OperationResult<IncidentReportDto>.Failure([ex.Message]);
            }
        }

        public async Task<OperationResult> EvaluateAndInitiateDrugTestForIncidentAsync(int incidentReportId, DateTime? testDate = null)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var incidentReport = await _context.IncidentReports.FindAsync(incidentReportId);
                if (incidentReport == null)
                {
                    return OperationResult.Failure(new List<string> { "Incident report not found." });
                }

                // Evaluate the need for a drug and alcohol test
                if (incidentReport.RequiresDrugAndAlcoholTest)
                {
                    var drugAndAlcoholTest = new DrugAndAlcoholTest
                    {
                        EmployeeId = incidentReport.EmployeeId,
                        TestDate = testDate ?? DateTime.UtcNow,
                        TestType = TestType.PostAccident,

                    };

                    _context.DrugAndAlcoholTests.Add(drugAndAlcoholTest);
                    await _context.SaveChangesAsync();

                    // Update the incident report with the test ID
                    incidentReport.DrugAndAlcoholTestId = drugAndAlcoholTest.DrugAndAlcoholTestId;
                    await _context.SaveChangesAsync();

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
