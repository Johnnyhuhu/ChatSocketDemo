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
    public partial class ChatClient : Form
    {
        private Socket _clientSocket;
        private User _user;

        public ChatClient()
        {
            InitializeComponent();
        }

        public ChatClient(User v_user)
        {
            InitializeComponent();
            this._user = v_user;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {

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




    }
}
