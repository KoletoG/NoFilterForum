﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Section
{
    public class SectionItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public int PostsCount { get; set; }
    }
}
