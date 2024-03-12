﻿using Guilded.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class Backpack
    {
        public Guid Id { get; set; }
        public Guid? MemberId { get; set; }
        public string? ServerId { get; set; }
        public ICollection<Item>? Items { get; set; }

    }
}
