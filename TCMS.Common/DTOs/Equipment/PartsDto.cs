using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.Equipment
{
    public class PartsDto
    {
        public List<PartDetailDto>? AddedParts { get; set; }
        public List<PartDetailDto>? UpdatedParts { get; set; }
        public List<PartDetailDto>? RemovedParts { get; set; }
    }

}
