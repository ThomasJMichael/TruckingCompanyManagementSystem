using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.GUI.Models
{
    public class MaintenanceRecord
    {
        public int maintenanceRecordID { get; set; }
        public int vehicleID {  get; set; }
        public DateTime maintenanceDate { get; set; }
        public string description {  get; set; }
        public decimal cost { get; set; }
        public ObservableCollection<Part>? Parts { get; set; }

    }
}
