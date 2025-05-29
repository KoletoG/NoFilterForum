using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class GetIndexPostRequest
    {
        public string TitleOfSection { get; set; }
        public int Page {  get; set; }
    }
}
