﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.Logo = "~/Content/Images/Logo.png";
        }
    }
}