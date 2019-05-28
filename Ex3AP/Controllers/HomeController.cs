using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3AP.Models;

namespace Ex3AP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()

        { 
            return View();
        }

        [HttpGet]
        public ActionResult MainDisplay(string ip, int port)
        {
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;

            Info.Instance.connect();
            Info.Instance.listen();

            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;
            return View();
        }

        [HttpGet]
        public ActionResult Display()
        {
            Info.Instance.Ip = "127.0.0.1";
            Info.Instance.Port = 5400;

            Info.Instance.connect();

            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;


            return View();
        }

    }
}