﻿using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        public ActionResult Index()
        {

            //busco usuario logiado y lo comparo con los que tengo en la bd:
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            return View(user);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            return View(user);
        }
    }
}