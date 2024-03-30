using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Services.ReportSchemas
{
    public class ReportSchema
    {
        public string Filename { get; set; }
        public List<string> Fields { get; set; }
    }
}
