using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatSocketDemo
{
    public class User
    {
        public string p_userName { get; set; }
        public string p_serverIP { get; set; }
        public string p_serverPort { get; set; }
        public Socket p_Client { get; set; }
    }
}
