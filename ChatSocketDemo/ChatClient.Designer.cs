namespace ChatSocketDemo
{
    partial class ChatClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSendMsg = new System.Windows.Forms.RichTextBox();
            this.txtChatMsg = new System.Windows.Forms.RichTextBox();
            this.lbl在线列表 = new System.Windows.Forms.Label();
            this.lbx在线列表 = new System.Windows.Forms.ListBox();
            this.TcpInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(495, 273);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 32);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(495, 224);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 35);
            this.btnSend.TabIndex = 10;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(167, 224);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(322, 81);
            this.txtSendMsg.TabIndex = 9;
            this.txtSendMsg.Text = "";
            // 
            // txtChatMsg
            // 
            this.txtChatMsg.Location = new System.Drawing.Point(167, 25);
            this.txtChatMsg.Name = "txtChatMsg";
            this.txtChatMsg.Size = new System.Drawing.Size(403, 193);
            this.txtChatMsg.TabIndex = 8;
            this.txtChatMsg.Text = "";
            // 
            // lbl在线列表
            // 
            this.lbl在线列表.AutoSize = true;
            this.lbl在线列表.Location = new System.Drawing.Point(9, 10);
            this.lbl在线列表.Name = "lbl在线列表";
            this.lbl在线列表.Size = new System.Drawing.Size(65, 12);
            this.lbl在线列表.TabIndex = 7;
            this.lbl在线列表.Text = "在线列表：";
            // 
            // lbx在线列表
            // 
            this.lbx在线列表.FormattingEnabled = true;
            this.lbx在线列表.ItemHeight = 12;
            this.lbx在线列表.Location = new System.Drawing.Point(9, 25);
            this.lbx在线列表.Name = "lbx在线列表";
            this.lbx在线列表.Size = new System.Drawing.Size(125, 280);
            this.lbx在线列表.TabIndex = 6;
            // 
            // TcpInfo
            // 
            this.TcpInfo.Location = new System.Drawing.Point(600, 25);
            this.TcpInfo.Name = "TcpInfo";
            this.TcpInfo.Size = new System.Drawing.Size(234, 193);
            this.TcpInfo.TabIndex = 12;
            this.TcpInfo.Text = "";
            // 
            // ChatClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 319);
            this.Controls.Add(this.TcpInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtSendMsg);
            this.Controls.Add(this.txtChatMsg);
            this.Controls.Add(this.lbl在线列表);
            this.Controls.Add(this.lbx在线列表);
            this.Name = "ChatClient";
            this.Text = "客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatClient_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtSendMsg;
        private System.Windows.Forms.RichTextBox txtChatMsg;
        private System.Windows.Forms.Label lbl在线列表;
        private System.Windows.Forms.ListBox lbx在线列表;
        private System.Windows.Forms.RichTextBox TcpInfo;
    }
}