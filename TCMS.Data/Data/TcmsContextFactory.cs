using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TCMS.Data.Data
{
    
    // Add DbContext
    public class TcmsContextFactory : IDesignTimeDbContextFactory<TcmsContext>
    {
        // Build configuration
        public IConfiguration Configuration { get; } = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json")
                       .Build();
        public TcmsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TcmsContext>();
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            return new TcmsContext(optionsBuilder.Options);
        }
    }
}
