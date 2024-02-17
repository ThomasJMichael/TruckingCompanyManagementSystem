using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMS.Common.DTOs.User
{
    public class ChangeUsernameDto
    {
        [Required]
        public string CurrentUsername { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username must contain only letters and numbers.")]
        public string NewUsername { get; set; }
    }
}
