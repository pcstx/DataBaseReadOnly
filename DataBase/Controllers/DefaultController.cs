﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataBase.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
         [OutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            return View();
        }
    }
}