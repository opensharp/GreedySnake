namespace 贪吃蛇
{
    partial class LoginForm
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
            this.components = new System.ComponentModel.Container();
            this.Offline = new System.Windows.Forms.Button();
            this.RefreshList = new System.Windows.Forms.Button();
            this.LocalAddress = new System.Windows.Forms.Label();
            this.RemoteAddress = new System.Windows.Forms.ListBox();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Offline
            // 
            this.Offline.Location = new System.Drawing.Point(13, 13);
            this.Offline.Margin = new System.Windows.Forms.Padding(4);
            this.Offline.Name = "Offline";
            this.Offline.Size = new System.Drawing.Size(108, 51);
            this.Offline.TabIndex = 0;
            this.Offline.Text = "单机游戏";
            this.Offline.UseVisualStyleBackColor = true;
            this.Offline.Click += new System.EventHandler(this.Offline_Click);
            // 
            // RefreshList
            // 
            this.RefreshList.Location = new System.Drawing.Point(129, 13);
            this.RefreshList.Margin = new System.Windows.Forms.Padding(4);
            this.RefreshList.Name = "RefreshList";
            this.RefreshList.Size = new System.Drawing.Size(108, 51);
            this.RefreshList.TabIndex = 1;
            this.RefreshList.Text = "刷新列表";
            this.RefreshList.UseVisualStyleBackColor = true;
            this.RefreshList.Click += new System.EventHandler(this.RefreshList_Click);
            // 
            // LocalAddress
            // 
            this.LocalAddress.Location = new System.Drawing.Point(13, 246);
            this.LocalAddress.Margin = new System.Windows.Forms.Padding(4);
            this.LocalAddress.Name = "LocalAddress";
            this.LocalAddress.Size = new System.Drawing.Size(224, 27);
            this.LocalAddress.TabIndex = 2;
            this.LocalAddress.Text = "本机：";
            this.LocalAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RemoteAddress
            // 
            this.RemoteAddress.FormattingEnabled = true;
            this.RemoteAddress.ItemHeight = 27;
            this.RemoteAddress.Location = new System.Drawing.Point(13, 72);
            this.RemoteAddress.Margin = new System.Windows.Forms.Padding(4);
            this.RemoteAddress.Name = "RemoteAddress";
            this.RemoteAddress.Size = new System.Drawing.Size(224, 166);
            this.RemoteAddress.Sorted = true;
            this.RemoteAddress.TabIndex = 3;
            this.RemoteAddress.DoubleClick += new System.EventHandler(this.RemoteAddress_DoubleClick);
            // 
            // Clock
            // 
            this.Clock.Enabled = true;
            this.Clock.Interval = 300;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 286);
            this.Controls.Add(this.RemoteAddress);
            this.Controls.Add(this.LocalAddress);
            this.Controls.Add(this.RefreshList);
            this.Controls.Add(this.Offline);
            this.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模式选择";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Offline;
        private System.Windows.Forms.Button RefreshList;
        private System.Windows.Forms.Label LocalAddress;
        private System.Windows.Forms.ListBox RemoteAddress;
        private System.Windows.Forms.Timer Clock;
    }
}