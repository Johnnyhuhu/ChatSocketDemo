using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSocketDemoModel
{
    public class ServerMsgModel
    {
        public string SendIP { get; set; }
        public string SendPort { get; set; }
        public string SendUserName { get; set; }
        public string SendMsg { get; set; }
        public string SendType { get; set; }//消息类型 1 登录 2 talk 3 退出
        public string ReceiveIP { get; set; }
        public string ReceivePort { get; set; }
        public List<OnlineUser> OnlineUser { get; set; }
    }
}
