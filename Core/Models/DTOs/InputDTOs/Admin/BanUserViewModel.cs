using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Admin
{
    public class BanUserViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
