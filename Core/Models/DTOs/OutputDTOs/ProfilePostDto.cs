using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs
{
    public class ProfilePostDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
    }
}
