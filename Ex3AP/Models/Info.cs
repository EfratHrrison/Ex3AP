using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace Ex3AP.Models
{
    public class Info
    {

        TcpClient _client;
        double lon;
        double lat;
        private static Info m_Instance = null;
        private NetworkStream _ns;
        private StreamReader Reader;
        private StreamWriter Writer;

        //Thread threadInfo;

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

        public bool shouldContinue
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
           // shouldContinue = true;
        }

        //connect to the simulator as a client 
        public void connect()
       {
       
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Ip), Port);
            _client = new TcpClient();
            _client.Connect(ep);
         
        }

        //disconnect the simulator 
        public void disconnect()
        {
            //_client.Close();
            //shouldContinue = false;
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
            Lon = double.Parse(StrLon);
            Lat = double.Parse(StrLat);
            //Console.WriteLine("Lon{0} ,Lat{0}", Lon,Lat);
            //close the socket 
            //_ns.Close();
            //_client.Close();
        }

    }
}