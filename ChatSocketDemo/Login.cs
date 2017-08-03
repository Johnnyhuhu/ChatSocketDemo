using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatSocketDemo
{
    public partial class Login : Form
    {
        #region 用户定义
        public Socket ClientSocket { get; set; }
        public User p_user { get; set; } 
        #endregion


        #region 用户函数
        public Login()
        {
            InitializeComponent();
        } 
        #endregion

        #region 用户函数
        #region 暂时不用
        /// <summary>
        /// 客户端接收数据
        /// </summary>
        /// <param name="socket"></param>
        public void fnReceiveData(object socket)
        {
            var __clientSocket = socket as Socket;
            byte[] __data = new byte[1024 * 1024];
            while (true)
            {
                int __iLen = 0;
                try
                {
                    __iLen = __clientSocket.Receive(__data, 0, __data.Length, SocketFlags.None);
                }
                catch (Exception)
                {

                    this.StopConnect();
                    return;
                }
            }
        }

        private void StopConnect()
        {
            if (ClientSocket.Connected)
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close(100);
            }
        } 
        #endregion
        #endregion

        #region 窗体事件
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                User __user = new User();

                __user.p_serverIP = this.txtIP.Text;
                __user.p_userName = this.txtLoginName.Text;
                __user.p_serverPort = this.txtPort.Text;
                this.p_user = __user;
                this.DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        } 
        #endregion
    }
}
