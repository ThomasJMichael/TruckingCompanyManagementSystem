using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Common.DTOs.Shipment
{
    public class UpdateItemStatusDto
    {
        [Required(ErrorMessage = "Purchase order ID is required.")]
        public int PurchaseOrderId { get; set; }

        [Required(ErrorMessage = "Manifest ID is required.")]
        public int ManifestId { get; set; }

        [Required(ErrorMessage = "Item ID is required.")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public ItemStatus Status { get; set; }
    }
}
