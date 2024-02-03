﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Models
{
    public class Suggestion
    {
        public Guid Id { get; set; }
        public LocalServerMember? Author { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool Approved { get; set; }


    }
}
