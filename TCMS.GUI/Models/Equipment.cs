﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.GUI.Models
{
    public class Equipment
    {
        public int VehicleId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        ObservableCollection<Part> Parts { get; set; }
        ObservableCollection<MaintenanceRecord> MaintenanceRecords { get; set; }
    }
}
