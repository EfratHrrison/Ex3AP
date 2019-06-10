using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace Ex3AP.Models
{
    
    public class Info
    {
        public static int counter = 0;

        TcpClient _client;
        double lon;
        double lat;
        private static Info m_Instance = null;
        private NetworkStream _ns;
        private StreamReader Reader;
        public Boolean isConnect;
        public System.IO.StreamWriter file;
       
        public string Ip
        {
            set;
            get; 
        }
        public int Port
        {
            set;
            get;
        }
        public int Time
        {
            set;
            get;
        }

        public double Lon
        {
            set
            {
                lon = value;
            }
            get
            {
                return lon;
            }
        }

        public double Lat
        {
            set
            {
                lat = value;
            }
            get
            {
                return lat;
            }
        }

        public string EndOfFile
        {
            set;
            get;
        }

        public string fileName
        {
            set;
            get;
        }

        public double throttle
        {
            set;
            get;
        }

        public double rudder
        {
            set;
            get;
        }

        public static Info Instance
        {
            get
            {
                if (null == m_Instance)
                {
                    m_Instance = new Info();
                }
                return m_Instance;
            }
        }

        public Info()
        {
            isConnect = false;
        }

        //connect to the simulator as a client 
        public void connect()
       {
            if (!isConnect)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Ip), Port);
                _client = new TcpClient();
                _client.Connect(ep);
                isConnect = true;
            }
            else
            {
                return;
            }
       
        }

        public void listen()
        {
           
            byte[] bytes;
            _ns = _client.GetStream();
            Reader = new StreamReader(_ns);
           
            bytes = Encoding.ASCII.GetBytes("get /position/longitude-deg\r\n");
            _ns.Write(bytes, 0, bytes.Length);
            string StrLon= Reader.ReadLine().Split('=')[1].Split(' ')[1].Split('\'')[1];
            bytes = Encoding.ASCII.GetBytes("get /position/latitude-deg\r\n");
            _ns.Write(bytes, 0, bytes.Length);
            string StrLat = Reader.ReadLine().Split('=')[1].Split(' ')[1].Split('\'')[1];
            Random r = new Random();
            Lon = double.Parse(StrLon);
            Lat = double.Parse(StrLat);

        }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Val");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("EndOfFile", this.EndOfFile);
            writer.WriteEndElement();
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public void writeToFile()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            //if (!FileExists)
            //{
            //    IsFileExists(path);
            //}
            listen();
            using (file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(Lon.ToString());
                file.WriteLine(Lat.ToString());
                file.WriteLine(throttle.ToString());
                file.WriteLine(rudder.ToString());
            }
        }

        //public void IsFileExists(string path)
        //{
        //    if (File.Exists(path))
        //    {
        //        File.Delete(path);
        //        FileExists = true;

        //    }
        //    return;
        //}

        public void loadFile()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            string[] data = System.IO.File.ReadAllLines(path);        // reading all the lines of the file
            int numLines = data.Length;
            if(counter >= numLines)
            {
                EndOfFile =  "noMoreLines";
                counter = 0;
            }
            else
            {
                if (data[counter] == "")
                {
                    counter++;
                }
                Lon = double.Parse(data[counter]);
                counter++;
                Lat = double.Parse(data[counter]);
                counter += 3;
               
            }
           
        }


    }
}