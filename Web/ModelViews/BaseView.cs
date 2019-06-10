using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ModelViews
{
    public class BaseView
    {
        public int Id { set; get; }
        public string CreatedBy { set; get; }
        public string CreatedDate { set; get; }
    }
}