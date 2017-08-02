using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSocketDemoModel
{
    public class ClientMsgModel
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }//消息类型 1 登录 2 talk 3 退出
        public string NowDate { get; set; }
        public string ReceiveIP { get; set; }
        public string ReceivePort { get; set; }
    }
}
