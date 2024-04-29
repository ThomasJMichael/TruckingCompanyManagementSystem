using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class VehicleDto
    {
        public int VehicleId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Type { get; set; }

        public ICollection<PartDetailDto> Parts { get; set; }
        public ICollection<MaintenanceRecordDto> MaintenanceRecords { get; set; }
    }

}
