using Microsoft.EntityFrameworkCore;
using TCMS.Data.Data;

namespace TCMS.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // ConfigureServices
        public void ConfigureServices(IServiceCollection services)
        {
            // add db context for sqlite
            services.AddDbContext<TcmsContext>(options =>
                               options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
