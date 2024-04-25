using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.GUI.Models
{
    public class Part
    {
        public string partName { get; set; }
        public string partNumber {  get; set; }
        public int quantityOnHand {  get; set; }
        public decimal price {  get; set; }
        public string supplier { get; set; }
        public bool isFromStock {  get; set; }
        public int vehicleID {  get; set; }
        public int maintenanceRecordID {  get; set; }
        public int repairRecordID {  get; set; }

    }
}
