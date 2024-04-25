using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.GUI.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public ObservableCollection<Part>? Parts { get; set; }
        public ObservableCollection<MaintenanceRecord>? MaintenanceRecords { get; set;}
        public ObservableCollection<RepairRecord>? RepairRecords { get; set; }

        //year, model, type, parts list, maintenance record

        //maintenance record = routine inspections, routine maintenance (oil change, filter change, tire rotations etc) and repair records
        //repair records = description of repair, list of parts, company that performed repair

        //vehicle details: ID, Brand, Model, Year, Type
        //parts list: Name, ID, Quantity, Price, Supplier
        //maintenance records: ID, Type, Description, Date, Cost

    }
}
