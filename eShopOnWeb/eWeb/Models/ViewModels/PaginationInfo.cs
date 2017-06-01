using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eWeb.Models.ViewModels
{
    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int ActualPage { get; set; }
        public int TotalPages { get; set; }
        public string Previouse { get; set; }
        public string Next { get; set; }
    }
}
