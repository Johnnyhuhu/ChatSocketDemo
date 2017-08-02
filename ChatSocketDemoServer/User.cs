using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatSocketDemoServer
{
    public class User
    {
        public TcpClient client;
        public BinaryReader br;
        public BinaryWriter bw;

        public User(TcpClient client)
        {
            this.client = client;
            NetworkStream networkStream = this.client.GetStream();
            br = new BinaryReader(networkStream);
            bw = new BinaryWriter(networkStream);
        }
    }
}
