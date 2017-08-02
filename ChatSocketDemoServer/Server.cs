using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatSocketDemoServer
{
    public class Server
    {
        private static ListBox _listbox;
        public delegate void ListBoxCallBack(string str);
        public Server(ListBox v_listbox)
        {
            _listbox = v_listbox;
        }


        public static void SetListBox(string str)
        {
            if (_listbox.InvokeRequired)
            {
                ListBoxCallBack __listCallBack = new ListBoxCallBack(SetListBox);
                _listbox.Invoke(__listCallBack, str);
            }
            else
            {
                _listbox.Items.Add(str);
                _listbox.SelectedIndex = _listbox.Items.Count - 1;
                _listbox.ClearSelected();
            }

        }
    }
}
