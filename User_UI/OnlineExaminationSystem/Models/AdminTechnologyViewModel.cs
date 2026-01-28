using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem.Models
{
    public class AdminTechnologyViewModel
    {
        [Required(ErrorMessage = "Technology name is required")]
        public string TechnologyName { get; set; }
    }
}
