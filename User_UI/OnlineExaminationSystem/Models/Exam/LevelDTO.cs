using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES_WebAPI.Models.dtos
{
    public class LevelDTO
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int PassMarks { get; set; }
    }
}