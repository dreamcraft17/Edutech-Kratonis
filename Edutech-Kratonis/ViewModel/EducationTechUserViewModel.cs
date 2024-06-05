using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edutech_Kratonis.ViewModel
{
    public class EducationTechUserViewModel
    {

        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public int id { get; set; }
        public bool RememberMe { get; set; }
        public string activated { get; set; }
    }
}