﻿using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Warning
{
    public class CreateWarningViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MinLength(10,ErrorMessage ="Text characters should be at least 10")]
        public string Content { get; set; }
    }
}
