﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class CatalogType : BaseEntity<int>
    {
        public string Type { get; set; }
    }
}
