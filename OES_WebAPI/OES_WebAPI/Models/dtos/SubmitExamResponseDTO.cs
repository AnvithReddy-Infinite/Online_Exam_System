using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models.DTOs
{
    public class SubmitExamResponseDTO
    {
        public string UserName { get; set; }
        public string Technology { get; set; }
        public string Level { get; set; }
        public int Score { get; set; }
        public int TimeTakenMinutes { get; set; }
        public string Result { get; set; }
    }
}