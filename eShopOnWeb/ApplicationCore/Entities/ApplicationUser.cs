using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ApplicationUser : ClaimsIdentity
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
