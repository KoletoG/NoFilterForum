﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.DTOs.OutputDTOs.Message
{
    public record CreateMessageDTO(PostResult Result, string? Message, string? MessageId);
}
