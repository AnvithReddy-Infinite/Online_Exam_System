using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES_WepApi.Models.DTOs
{
    public class StudentSearchResultDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int TechId { get; set; }
        public int LevelId { get; set; }
        public int Score { get; set; }
    }
}