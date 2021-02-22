using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
namespace 贪吃蛇
{
    public partial class LoginForm : Form
    {
        string Parameter = null;
        Socket Client = null, Server = null;
        public LoginForm()
        {
            InitializeComponent();
            string Host = Dns.GetHostName();
            IPAddress[] List = Dns.GetHostAddresses(Host);
            foreach (IPAddress Local in List)
            {
                AddressFamily Family = Local.AddressFamily;
                if (Family == AddressFamily.InterNetwork)
                {
                    if (Local.ToString().LastIndexOf('.') < 11)
                    {
                        Parameter = Local.ToString();
                        LocalAddress.Text += Parameter;
                        break;
                    }
                }
            }
            ListenSocket();
            RemoteList();
        }
        private void ListenSocket()
        {
            Server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP = IPAddress.Parse(Parameter);
            Server.Bind(new IPEndPoint(IP, 1234));
            Server.Listen(5);
            Thread Listener = new Thread(ClientConnect);
            Listener.Start();
        }
        private void ClientConnect()
        {
            Client = Server.Accept();
        }
        private void Clock_Tick(object sender, EventArgs e)
        {
            if (Client != null)
            {
                Clock.Enabled = false;
                new MainForm(Client, false).Show();
                Hide();
            }
        }
        private void RemoteList()
        {
            RemoteAddress.Items.Clear();
            string Gap = Parameter.Remove(Parameter.LastIndexOf('.') + 1);
            for (int i = 2; i < 256; i++)
            {
                Ping Ping = new Ping();
                Ping.PingCompleted += new PingCompletedEventHandler(OK);
                Ping.SendAsync(Gap + i.ToString(), 1500, null);
            }
        }
        private void OK(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
                if (e.Reply.Address.ToString() != Parameter)
                {
                    string Reply = e.Reply.Address.ToString();
                    RemoteAddress.Items.Add(Reply);
                }
            }
            (sender as IDisposable).Dispose();
        }
        private void Offline_Click(object sender, EventArgs e)
        {
            new MainForm(null, true).Show();
            Hide();
        }
        private void RemoteAddress_DoubleClick(object sender, EventArgs e)
        {
            if (RemoteAddress.SelectedItem != null)
            {
                string IP = RemoteAddress.SelectedItem.ToString();
                IPAddress Address = IPAddress.Parse(IP);
                Socket Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Socket.Connect(new IPEndPoint(Address, 1234));
                    new MainForm(Socket, true).Show();
                    Hide();
                }
                catch
                {
                    MessageBox.Show("连接失败！", "提示");
                }
            }
        }
        private void RefreshList_Click(object sender, EventArgs e)
        {
            RemoteList();
        }
        private void LoginForm_FormClosed(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}