using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class BaseModel
    {
        public int Id { set; get; }
        public string CreatedBy { set; get; }
        public string CreatedDate { set; get; }
    }
}