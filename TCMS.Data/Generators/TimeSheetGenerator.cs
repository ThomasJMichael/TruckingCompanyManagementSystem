using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class TimeSheetGenerator
    {
        public static List<TimeSheet> GenerateTimeSheetsForEmployees(List<Employee> employees, int sheetsPerEmployee)
        {
            var timeSheetFaker = new Faker<TimeSheet>()
                .RuleFor(ts => ts.EmployeeId, f => f.PickRandom(employees).EmployeeId) // Associate with an employee
                .RuleFor(ts => ts.Date, f => f.Date.Recent(30)) // Generate a date within the last 30 days
                .RuleFor(ts => ts.ClockIn, (f, ts) => ts.Date.AddHours(f.Random.Int(6, 9))) // Clock in between 6am and 9am
                .RuleFor(ts => ts.ClockOut, (f, ts) => ts.ClockIn.AddHours(f.Random.Int(8, 12))) // Work between 8 and 12 hours
                .FinishWith((f, ts) =>
                {
                    // Ensure ClockOut is always later than ClockIn by at least one hour
                    if (ts.ClockOut <= ts.ClockIn)
                    {
                        ts.ClockOut = ts.ClockIn.AddHours(1);
                    }
                });

            var timeSheets = new List<TimeSheet>();
            foreach (var employee in employees)
            {
                timeSheets.AddRange(timeSheetFaker.Generate(sheetsPerEmployee)); // Generate N sheets per employee
            }

            return timeSheets;
        }
    }

}
