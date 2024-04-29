using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;

namespace TCMS.GUI.Models
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordID { get; set; }
        public RecordType RecordType { get; set; }
        public int VehicleId {  get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string Description {  get; set; }
        public decimal Cost { get; set; }
        public ObservableCollection<Part>? Parts { get; set; }

    }
}
