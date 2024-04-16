using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.DataAnnotations;

namespace TCMS.Common.DTOs.User
{
    public class UserRoleDto
    {
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        public string? RoleName { get; set; }
    }
}
