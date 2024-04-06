using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class IncidentAndTestGenerator
    {
        public static List<IncidentReport> GenerateIncidents(List<Driver> drivers, int incidentCount, Faker faker)
        {
            var incidentReports = new List<IncidentReport>();

            for (int i = 0; i < incidentCount; i++)
            {
                var driver = faker.PickRandom(drivers);
                var incidentReport = new IncidentReport
                {
                    IncidentDate = faker.Date.Past(1),
                    Location = faker.Address.City(),
                    Type = faker.PickRandom<IncidentType>(),
                    Description = faker.Lorem.Sentence(),
                    EmployeeId = driver.EmployeeId,
                    Driver = driver,
                };

                incidentReport.IsFatal = faker.Random.Bool(0.1f);
                incidentReport.HasInjuries = faker.Random.Bool(0.3f);
                incidentReport.HasTowedVehicle = faker.Random.Bool(0.6f);
                incidentReport.CitationIssued = faker.Random.Bool(0.5f);

                incidentReports.Add(incidentReport);
            }

            return incidentReports;
        }

        public static List<DrugAndAlcoholTest> GenerateTestsForIncidents(List<IncidentReport> incidentReports)
        {
            var drugAndAlcoholTests = new List<DrugAndAlcoholTest>();

            foreach (var report in incidentReports)
            {
                if (report.Type == IncidentType.Accident && (report.IsFatal || (report.HasInjuries && report.CitationIssued) || (report.HasTowedVehicle && report.CitationIssued)))
                {
                    var test = new DrugAndAlcoholTest
                    {
                        EmployeeId = report.EmployeeId,
                        TestDate = report.IncidentDate.AddDays(1), // Test a day after the incident
                        TestType = TestType.PostAccident,
                        TestResult = TestResult.Negative,
                        TestDetails = "Test performed post-accident.",
                        IncidentReportId = report.IncidentReportId,
                        IncidentReport = report
                    };
                    drugAndAlcoholTests.Add(test);
                    report.DrugAndAlcoholTest = test;
                    report.DrugAndAlcoholTestId = test.DrugAndAlcoholTestId;
                }
            }

            return drugAndAlcoholTests;
        }

        public static List<DrugAndAlcoholTest> GenerateUnrelatedTests(List<Driver> drivers, int testCount, Faker faker)
        {
            var drugAndAlcoholTests = new List<DrugAndAlcoholTest>();

            for (int i = 0; i < testCount; i++)
            {
                var driver = faker.PickRandom(drivers);
                var test = new DrugAndAlcoholTest
                {
                    EmployeeId = driver.EmployeeId,
                    Driver = driver,
                    TestDate = faker.Date.Past(1),
                    TestType = faker.PickRandom<TestType>(),
                    TestResult = faker.PickRandom<TestResult>(),
                    TestDetails = faker.Lorem.Sentence(),
                };

                test.IncidentReport = null;
                test.IncidentReportId = null;
                drugAndAlcoholTests.Add(test);
            }

            return drugAndAlcoholTests;
        }
    }

}
