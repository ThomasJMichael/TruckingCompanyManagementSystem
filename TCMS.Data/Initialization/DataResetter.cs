using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TCMS.Data.Data;


namespace TCMS.Data.Initialization
{
    public class DatabaseResetter
    {
        public static async Task ResetDatabaseAsync(TcmsContext context)
        {
            // Disable foreign key checks to avoid constraint violations during delete
            await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF;");

            // Clear the tables
            context.Inventories.RemoveRange(context.Inventories);
            context.Products.RemoveRange(context.Products);
            context.TimeSheets.RemoveRange(context.TimeSheets.ToList());
            context.Employees.RemoveRange(context.Employees.ToList());
            context.Users.RemoveRange(context.Users.ToList());
            await context.SaveChangesAsync();

            // Reset the auto-increment ID counters
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Inventories';");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Products';");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='TimeSheets';");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Employees';");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='Users';");

            // Re-enable foreign key checks
            await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;");

            await context.SaveChangesAsync();
        }
    }
}

