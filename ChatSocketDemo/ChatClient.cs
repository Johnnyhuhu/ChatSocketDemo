using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ChatSocketDemoModel;
using System.Net;

namespace ChatSocketDemo
{
    public partial class ChatClient : Form
    {
        #region 用户定义
        private Socket _clientSocket;
        private User _user;

        bool linkFlag = false; //对话状态
        bool uw = false; //写入对话内容
        string getMess; //从服务端接受的数据
        private bool isExit = false; //连接状态 是否退出
        private TcpClient client; //客户端
        private NetworkStream networkStream; //传输流
        private BinaryReader br; //读传输流
        private BinaryWriter bw; //写传输流

        private delegate void AddListBoxItemCallBack(string str);
        private AddListBoxItemCallBack _addList;
        private delegate void AppendChatMsgTextCallBack(string str);
        private AppendChatMsgTextCallBack _appendChatMsgText;

        //在线人数
        List<OnlineUser> _onlineUser = new List<OnlineUser>();

        #endregion

        #region 系统函数
        public ChatClient()
        {
            InitializeComponent();
        }        
        public ChatClient(User v_user)
        {
            InitializeComponent();
            this._user = v_user;
            //建立网络通信
            try
            {
                this.client = new TcpClient(this._user.p_serverIP, int.Parse(this._user.p_serverPort));//定义服务器端ip地址和端口，与服务器端定义要一致

                this.linkFlag = true;
            }
            catch
            {

                return;
            }
            this._addList = new AddListBoxItemCallBack(this.AddListBoxItem);
            this._appendChatMsgText = new AppendChatMsgTextCallBack(this.AppendChatMsgText);
            if (this.linkFlag)
            {
                //获取网络流
                networkStream = client.GetStream();
                //将网络作为二进制读写对象，使用utf8编码
                br = new BinaryReader(networkStream);
                bw = new BinaryWriter(networkStream);

                //发送套接字、当前栏目、编辑的内容、用户名、发送时间。
                ClientMsgModel msg = new ClientMsgModel()
                {
                    IP = this._user.p_serverIP,
                    Port = this._user.p_serverPort,
                    Msg = this.txtSendMsg.Text,
                    NowDate = DateTime.Now.ToString(),
                    Type = "1",
                    UserName = this._user.p_userName
                };
                //string sendMsg = ConvertJson.ToJson(msg);
                string sendMsg = JsonConvert.SerializeObject(msg);

                SendString(sendMsg);

                ThreadStart ts = new ThreadStart(ReceiveData);
                Thread thRece = new Thread(ts);
                thRece.Start();
            }
            this.Text = this.Text + " 当前登录名：" + this._user.p_userName;
        } 
        #endregion

        #region 用户函数
        private void ReceiveData()
        {
            while (this.isExit==false)
            {
                string receiveString = null;
                try
                {
                    //从网络流中读出字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读出字符串
                    receiveString = br.ReadString();
                }
                catch
                {
                    //底层套接字不存在时会出现异常
                    //提示数据接收失败
                    TcpInfo.AppendText("接收数据失败");
                }
                if (receiveString == null)
                {
                    if (isExit == false)
                    {
                        MessageBox.Show("与服务器失去联系！");
                    }
                    break;
                }
                uw = true;
                getMess = receiveString;
                //提示接收到的数据
                //TcpInfo.AppendText(Environment.NewLine + "接收服务器数据：" + Environment.NewLine + receiveString);
                ServerMsgModel __serverMsg = (ServerMsgModel)JsonConvert.DeserializeObject(receiveString, typeof(ServerMsgModel));

                switch (__serverMsg.SendType)
                {
                    case "1":
                        this._onlineUser = __serverMsg.OnlineUser;
                        for (int i = 0; i < __serverMsg.OnlineUser.Count; i++)
                        {
                            this.AddListBoxItem(__serverMsg.OnlineUser[i].UserName);
                        }
                        break;
                    case "2":
                        this.AppendChatMsgText(__serverMsg.SendMsg);
                        break;
                    default: break;
                }  

            }
        }
        private void SendString(string str)
        {
            try
            {
                //将字符串写入网络，此方法会自动附加字符串长度前缀
                bw.Write(str);
                bw.Flush();

                //提示发送成功
                TcpInfo.AppendText("发送：" + str);
            }
            catch
            {

                //提示发送失败
                TcpInfo.AppendText("发送失败!");
            }
        } 

        public void AddListBoxItem(string str)
        {
            if (this.lbx在线列表.InvokeRequired)
            {
                this.Invoke(_addList, str);
            }
            else
            {
                if (!this.lbx在线列表.Items.Contains(str))
                {
                    this.lbx在线列表.Items.Add(str);
                }
            }
        }

        public void AppendChatMsgText(string str)
        {
            if (this.txtChatMsg.InvokeRequired)
            {
                this.txtChatMsg.Invoke(this._appendChatMsgText, str);
            }
            else
            {
                this.txtChatMsg.AppendText(str + Environment.NewLine);
            }
        }

        #endregion

        #region 窗体事件
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //Func<OnlineUser, bool> b =
                OnlineUser ol = this._onlineUser.Where(delegate(OnlineUser olUser)
                { 
                    return olUser.UserName == this.lbx在线列表.SelectedItem.ToString(); 
                }).FirstOrDefault();
                //发送套接字、当前栏目、编辑的内容、用户名、发送时间。
                ClientMsgModel msg = new ClientMsgModel()
                {
                    IP = this._user.p_serverIP,
                    Port = this._user.p_serverPort,
                    Msg = this.txtSendMsg.Text,
                    NowDate = DateTime.Now.ToString(),
                    Type = "2",
                    UserName = this._user.p_userName,
                    ReceiveIP = ol.IP,
                    ReceivePort = ol.Port,
                };
                //string sendMsg = ConvertJson.ToJson(msg);
                string sendMsg = JsonConvert.SerializeObject(msg);
                SendString(sendMsg);
                this.txtSendMsg.Clear();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        #endregion




    }
}
