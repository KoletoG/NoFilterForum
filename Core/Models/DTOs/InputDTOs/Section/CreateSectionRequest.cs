﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Section
{
    public class CreateSectionRequest
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
    }
}
