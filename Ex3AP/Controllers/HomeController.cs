using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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

        //Display a the simulator location by his longitude and latitude 
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

        //Display the flight route 
        [HttpGet]
        public ActionResult DisplayPath(string ip, int port, int time)
        {
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;
            Info.Instance.Time = time;
            Info.Instance.connect();
            Info.Instance.listen();
            Session["time"] = time;

            ViewBag.lon = Info.Instance.Lon;
            ViewBag.lat = Info.Instance.Lat;


            return View();
        }

        //save the flight route to a file 
        [HttpGet]
        public ActionResult savePath(string ip, int port, int time, int totalTime, string fileName)
        {
            Info.Instance.Ip = ip;
            Info.Instance.Port = port;
            Info.Instance.Time = time;
            Info.Instance.fileName = fileName;
            Info.Instance.connect();
            Info.Instance.listen();
            Session["time"] = time;
            Session["totalTime"] = totalTime;

            return View();
        }

        //load the flight rout from a file
        [HttpGet]
        public ActionResult loadPath(string fileName, int time)
        {
            
            Session["time"] = time;
            Info.Instance.fileName = fileName;
     
            return View();
        }

        [HttpPost]
        public string GetVal()
        {
            Info.Instance.listen();

            return ToXml();
        }


        [HttpPost]
        public string readFromFile()
        {
            Info.Instance.loadFile();
           
            return ToXml();
        }

        // write into an XML all the values of the lon and the lat
        private string ToXml()
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            
            Info.Instance.ToXml(writer);
            
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public string SavePathInFile()
        {
            Info.Instance.writeToFile();
            return ToXml();
        }

        public ActionResult chooseDisplay(string str, int num)
        {
            System.Net.IPAddress ip = null;
            bool isValid = System.Net.IPAddress.TryParse(str, out ip);
            if (isValid)
            {
                MainDisplay(str, num);
                return View("MainDisplay");
            }
            loadPath(str, num);
            return View("loadPath");
        }

    }
}