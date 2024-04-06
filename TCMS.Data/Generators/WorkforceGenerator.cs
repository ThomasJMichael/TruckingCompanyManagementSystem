using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class WorkforceGenerator
    {
        public static List<Employee> GenerateRegularEmployees(int count)
        {
            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Address, f => f.Address.StreetAddress())
                .RuleFor(e => e.City, f => f.Address.City())
                .RuleFor(e => e.State, f => f.Address.State())
                .RuleFor(e => e.Zip, f => f.Address.ZipCode())
                .RuleFor(e => e.HomePhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.CellPhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.PayRate, f => f.Finance.Amount(15, 50))
                .RuleFor(e => e.StartDate, f => f.Date.Past(10));

            return employeeFaker.Generate(count);
        }


        public static List<Employee> GenerateDrivers(int count)
        {
            var driverFaker = new Faker<Driver>()
                .RuleFor(d => d.FirstName, f => f.Name.FirstName())
                .RuleFor(d => d.LastName, f => f.Name.LastName())
                .RuleFor(d => d.Address, f => f.Address.StreetAddress())
                .RuleFor(d => d.City, f => f.Address.City())
                .RuleFor(d => d.State, f => f.Address.State())
                .RuleFor(d => d.Zip, f => f.Address.ZipCode())
                .RuleFor(d => d.HomePhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(d => d.CellPhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(d => d.PayRate, f => f.Finance.Amount(15, 50))
                .RuleFor(d => d.StartDate, f => f.Date.Past(10))
                .RuleFor(d => d.CDLNumber, f => f.Random.Replace("####-###-####"))
                .RuleFor(d => d.CDLExperationDate, f => f.Date.Future());

            return driverFaker.Generate(count).Cast<Employee>().ToList();
        }

    }
}
