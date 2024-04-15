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
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync(); // This reapplies all existing migrations
        }

    }
}

