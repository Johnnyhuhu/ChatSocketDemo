using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ChatSocketDemoModel;

namespace ChatSocketDemoServer
{
    public partial class ChatServer : Form
    {
        #region 用户定义
        private List<Socket> _ClientProxSocketList = new List<Socket>();
        //连接的用户
        private List<User> userList = new List<User>();
        private List<OnlineUser> onlineUser = new List<OnlineUser>();

        private IPAddress localAddress;
        private int port = 51888;

        private delegate void SetListBoxCallBack(string str);
        private SetListBoxCallBack setListBoxCallback;
        private delegate void SetComboBoxCallBack(User user);
        private SetComboBoxCallBack setComboBoxCallback;

        private TcpListener myListener;
        ArrayList MessList = new ArrayList();
        int MessCount = 0; 
        #endregion

        #region 系统函数
        public ChatServer()
        {
            InitializeComponent();
            this.setListBoxCallback = new SetListBoxCallBack(SetListBox);
            this.setComboBoxCallback = new SetComboBoxCallBack(AddComboBoxItem);
            IPAddress[] addrIP = Dns.GetHostAddresses(this.txtIP.Text); //ip地址
            localAddress = addrIP[0];
        } 
        #endregion

        #region 用户函数
        public void fnConnection()
        {
            #region 另一种监听方式 已注释
            ////1.创建Socket对象
            //Socket __serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////2.绑定端口IP
            //IPAddress __ip = IPAddress.Parse(this.txtIP.Text);
            //__serverSocket.Bind(new IPEndPoint(__ip,int.Parse(this.txtPort.Text)));
            ////3.开始侦听
            ////连接等待的队列，最大为10个。 
            ////如果同时来的100个连接请求，只能处理1个，队列中放着10个等待连接的客户端，其他的返回错误消息。
            //__serverSocket.Listen(100);
            ////4.开始接收客户端的连接
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.fnAcceptClientConnect), __serverSocket); 
            #endregion

            myListener = new TcpListener(localAddress, int.Parse(this.txtPort.Text));
            myListener.Start();
            this.SetListBox(string.Format("开始在{0}:{1}监听客户连接", this.localAddress, this.port));
            //创建一个线程监听客户端连接请求
            ThreadStart ts = new ThreadStart(this.ListenClientConnect);
            Thread myThread = new Thread(ts);
            myThread.Start();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        /// <summary>
        /// 接收客户端连接
        /// </summary>
        private void ListenClientConnect()
        {
            while (true)
            {
                TcpClient newClient = null;
                try
                {
                    //等待用户进入
                    newClient = myListener.AcceptTcpClient();
                }
                catch (Exception)
                {
                    //当单击“停止监听”或者退出此窗体时AcceptTcpClient()会产生异常
                    //因此可以利用此异常退出循环
                    break;
                }

                //每接受一个客户端连接,就创建一个对应的线程循环接收该客户端发来的信息
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ReceiveData);
                Thread threadReceive = new Thread(pts);
                User user = new User(newClient);
                threadReceive.Start(user);
                userList.Add(user);
                AddComboBoxItem(user);
                SetListBox(string.Format("[{0}]进入", newClient.Client.RemoteEndPoint));
                SetListBox(string.Format("当前连接用户数：{0}", userList.Count));

            }
        }

        /// <summary>
        /// 接收、处理客户端信息，每客户1个线程，参数用于区分是哪个客户
        /// </summary>
        /// <param name="obj"></param>
        public void ReceiveData(object obj)
        {
            User user = obj as User;
            TcpClient client = user.client;
            //是否正常退出接收线程
            bool normalExit = false;
            //用于控制是否退出循环
            bool exitWhile = false;
            while (exitWhile == false)
            {
                string receiveString = null;
                try
                {
                    //从网络流中读出字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读出字符串
                    receiveString = user.br.ReadString();
                }
                catch (Exception)
                {
                    //底层套接字不存在时会出现异常
                    SetListBox("接收数据失败");
                }

                if (receiveString == null)
                {

                    if (normalExit == false)
                    {
                        //如果停止了监听，Connected为false
                        if (client.Connected == true)
                        {
                            SetListBox(string.Format("与[{0}]失去联系，已终止接收该用户信息", client.Client.RemoteEndPoint));
                        }
                    }
                    break;
                }
                SetListBox(string.Format("来自[{0}]：{1}", user.client.Client.RemoteEndPoint, receiveString));
                ClientMsgModel msg = new ClientMsgModel();
                msg = (ClientMsgModel)JsonConvert.DeserializeObject(receiveString, typeof(ClientMsgModel));
                                
                switch (msg.Type)
                {
                    case "1":
                        this.SendLoginMsg(msg, user);
                        break;

                    case "2":
                        //OnlineUser olUser = this.onlineUser.Where(delegate(OnlineUser ol)
                        //{
                        //    return ol.IP == msg.ReceiveIP && ol.Port == msg.ReceivePort;
                        //}).FirstOrDefault();
                        User userReceive = this.userList.Where(delegate(User u)
                        {
                            return u.client.Client.RemoteEndPoint.ToString() == msg.ReceiveIP + ":" + msg.ReceivePort;
                        }).FirstOrDefault();

                        ServerMsgModel __serMsgModel = new ServerMsgModel();
                        __serMsgModel.SendIP = msg.IP;
                        __serMsgModel.SendPort = msg.Port;
                        __serMsgModel.SendUserName = msg.UserName;
                        __serMsgModel.SendType = msg.Type;
                        string __sendSerMsg = JsonConvert.SerializeObject(__serMsgModel);

                        SendToClient(userReceive, receiveString);
                        break;
                    default: break;
                }
            }
            userList.Remove(user);
            client.Close();
            SetListBox(string.Format("当前连接用户数：{0}", userList.Count));

        }

        private void SendTalkMsg()
        {

        }

        public void SendLoginMsg(ClientMsgModel msg, User user)
        {
            //IPAddress ip = IPAddress.Parse(user.client.Client.RemoteEndPoint.ToString());

            OnlineUser __onlineUser = new OnlineUser();
            __onlineUser.IP = msg.IP;
            __onlineUser.Port = msg.Port;
            __onlineUser.UserName = msg.UserName;
            if (!this.onlineUser.Contains(__onlineUser))
            {
                this.onlineUser.Add(__onlineUser);
            }

            SetListBox(string.Format("登录IP:{0}，端口：{1}", msg.IP, msg.Port));

            ServerMsgModel __serMsgModel = new ServerMsgModel();
            __serMsgModel.SendIP = msg.IP;
            __serMsgModel.SendPort = msg.Port;
            __serMsgModel.SendUserName = msg.UserName;
            __serMsgModel.SendType = msg.Type;
            __serMsgModel.OnlineUser = this.onlineUser;
            string __sendSerMsg = JsonConvert.SerializeObject(__serMsgModel);

            for (int i = 0; i < this.userList.Count; i++)
            {
                SendToClient(userList[i], __sendSerMsg);
            }
        }

        public void SendToClient(User user, string str)
        {
            try
            {
                user.bw.Write(str);
                user.bw.Flush();
                SetListBox(string.Format("向[{0}]发送：{1}", user.client.Client.RemoteEndPoint, str));

            }
            catch
            {
                SetListBox(string.Format("向[{0}]发送信息失败", user.client.Client.RemoteEndPoint));
            }
        }

        private void SetListBox(string str)
        {
            if (this.listBoxStatus.InvokeRequired)
            {
                this.Invoke(this.setListBoxCallback, str);
            }
            else
            {
                this.listBoxStatus.AppendText(str + Environment.NewLine);
            }
        }

        private void AddComboBoxItem(User user)
        {
            if (this.comboBoxReceiver.InvokeRequired)
            {
                this.Invoke(setComboBoxCallback, user);
            }
            else
            {
                this.comboBoxReceiver.Items.Add(user.client.Client.RemoteEndPoint);
            }
        }

        #region 暂时不用
        /// <summary>
        /// 接收客户端连接  暂不用
        /// </summary>
        public void fnAcceptClientConnect(object v_socket)
        {
            var __serverSocket = v_socket as Socket;
            Server.SetListBox("服务器端开始接收客户端链接");
            while (true)
            {
                Socket __proxSocket = __serverSocket.Accept();
                Server.SetListBox(string.Format("客户端：{0}连接上了", __proxSocket.RemoteEndPoint.ToString()));

                this._ClientProxSocketList.Add(__proxSocket);

                //不停的接收当前链接的客户端发送来的消息
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.fnReceiveData), __proxSocket);
            }
        }

        /// <summary>
        /// 接收客户端的消息
        /// </summary>
        /// <param name="v_socket"></param>
        public void fnReceiveData(object v_socket)
        {
            var __proxSocket = v_socket as Socket;
            byte[] data = new byte[1024 * 1024];
            while (true)
            {
                int __iLen = 0;
                try
                {
                    __iLen = __proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch (Exception)
                {
                    //异常退出
                    Server.SetListBox(string.Format("客户端:{0}非正常退出",
                        __proxSocket.RemoteEndPoint.ToString()));
                    this._ClientProxSocketList.Remove(__proxSocket);

                    this.fnStopConnect(__proxSocket);
                    return;
                }

                if (__iLen <= 0)
                {
                    //客户端正常退出
                    Server.SetListBox(string.Format("客户端:{0}正常退出",
                        __proxSocket.RemoteEndPoint.ToString()));

                    this._ClientProxSocketList.Remove(__proxSocket);
                    this.fnStopConnect(__proxSocket);//停止连接
                    return;//让方法结束，终结当前接收客户端数据的异步线程。
                }

            }
        }

        private void fnStopConnect(Socket proxSocket)
        {
            try
            {
                if (proxSocket.Connected)
                {
                    proxSocket.Shutdown(SocketShutdown.Both);
                    proxSocket.Close(100);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        } 
        #endregion
        #endregion

        #region 窗体事件
        private void buttonSend_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                this.fnConnection();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                SetListBox(string.Format("目前连接用户数：{0}", userList.Count));
                SetListBox("开始停止服务，并依次使用户退出!");
                for (int i = 0; i < userList.Count; i++)
                {
                    comboBoxReceiver.Items.Remove(userList[i].client.Client.RemoteEndPoint);
                    userList[i].br.Close();
                    userList[i].bw.Close();
                    userList[i].client.Close();
                }
                //通过停止监听让myListener.AcceptTcpClient()产生异常退出监听线程
                myListener.Stop();
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        #endregion
    }
}
