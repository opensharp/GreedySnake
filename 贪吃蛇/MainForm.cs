using System;
using System.Collections;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
namespace 贪吃蛇
{
    public partial class MainForm : Form
    {
        int X1 = 7, X2 = 30;
        int Y1 = 20, Y2 = 7;
        Graphics Draw = null;
        Socket Socket = null;
        Queue Body1 = new Queue();
        Queue Body2 = new Queue();
        char Key1 = 'd', Key2 = 'k';
        char Dir1 = 'd', Dir2 = 'k';
        int Tail1 = 803, Tail2 = 150;
        Bitmap Panel = new Bitmap(800, 600);
        Bitmap BackDot = new Bitmap(20, 20);
        Bitmap HeadDot = new Bitmap(20, 20);
        Bitmap BodyDot = new Bitmap(20, 20);
        Bitmap TailDot = new Bitmap(20, 20);
        Bitmap FoodDot = new Bitmap(20, 20);
        Bitmap DarkDot = new Bitmap(20, 20);
        Block[] Buffer = new Block[1200];
        bool Master = true, Slave = true;
        delegate int Lambda(int x, int y);
        Lambda Sum = (x, y) => x + y * 40;
        private enum Block : byte
        {
            Back, Head, Body, Tail, Food, Dark, End
        }
        public MainForm(Socket Para1, bool Para2)
        {
            InitializeComponent();
            Socket = Para1;
            Master = Para1 != null && Para2;
            Slave = Para1 != null && !Para2;
            if (Master) Text += "  Master";
            else if (Slave) Text += "  Slave";
            Graphics.FromImage(BackDot).Clear(Color.White);
            Graphics.FromImage(HeadDot).Clear(Color.Red);
            Graphics.FromImage(BodyDot).Clear(Color.Blue);
            Graphics.FromImage(TailDot).Clear(Color.Yellow);
            Graphics.FromImage(FoodDot).Clear(Color.Green);
            Graphics.FromImage(DarkDot).Clear(Color.Black);
            Buffer[Sum(3, 20)] = Block.Tail;
            Buffer[Sum(4, 20)] = Block.Body;
            Buffer[Sum(5, 20)] = Block.Body;
            Buffer[Sum(6, 20)] = Block.Body;
            Buffer[Sum(7, 20)] = Block.Head;
            Buffer[Sum(30, 3)] = Block.Tail;
            Buffer[Sum(30, 4)] = Block.Body;
            Buffer[Sum(30, 5)] = Block.Body;
            Buffer[Sum(30, 6)] = Block.Body;
            Buffer[Sum(30, 7)] = Block.Head;
            Body1.Enqueue(Sum(4, 20));
            Body1.Enqueue(Sum(5, 20));
            Body1.Enqueue(Sum(6, 20));
            Body1.Enqueue(Sum(7, 20));
            Body2.Enqueue(Sum(30, 4));
            Body2.Enqueue(Sum(30, 5));
            Body2.Enqueue(Sum(30, 6));
            Body2.Enqueue(Sum(30, 7));
            for (int i = 0; i < 40; i++)
            {
                Buffer[Sum(i, 0)] = Block.Dark;
                Buffer[Sum(i, 29)] = Block.Dark;
            }
            for (int i = 1; i < 29; i++)
            {
                Buffer[Sum(0, i)] = Block.Dark;
                Buffer[Sum(39, i)] = Block.Dark;
            }
            if (Socket == null | Master)
            {
                GenerateFood(1);
                GenerateFood(2);
            }
            RefreshPalette();
            if (Socket != null)
            {
                Socket.NoDelay = true;
                Thread Thread = new Thread(ReceiveData);
                Thread.Start();
            }
        }
        private void Clock_Tick(object sender, EventArgs e)
        {
            Dir1 = Key1;
            Dir2 = Key2;
            if (Socket == null | Master)
            {
                bool Over = Sum(X1, Y1) == Sum(X2, Y2);
                ApplyChange(X1, Y1, Block.Body);
                ApplyChange(X2, Y2, Block.Body);
                switch (Dir1)
                {
                    case 'w': Y1--; break;
                    case 's': Y1++; break;
                    case 'a': X1--; break;
                    case 'd': X1++; break;
                }
                switch (Dir2)
                {
                    case 'i': Y2--; break;
                    case 'k': Y2++; break;
                    case 'j': X2--; break;
                    case 'l': X2++; break;
                }
                bool Food1 = Buffer[Sum(X1, Y1)] == Block.Food;
                bool Food2 = Buffer[Sum(X2, Y2)] == Block.Food;
                if (Buffer[Sum(X1, Y1)] == Block.Back)
                {
                    ApplyChange(Tail1, Block.Back);
                    Tail1 = (int)Body1.Dequeue();
                    ApplyChange(Tail1, Block.Tail);
                }
                else if (Food1) GenerateFood(1);
                else Over = true;
                if (Buffer[Sum(X2, Y2)] == Block.Back)
                {
                    ApplyChange(Tail2, Block.Back);
                    Tail2 = (int)Body2.Dequeue();
                    ApplyChange(Tail2, Block.Tail);
                }
                else if (Food2) GenerateFood(2);
                else Over = true;
                ApplyChange(X1, Y1, Block.Head);
                ApplyChange(X2, Y2, Block.Head);
                Body1.Enqueue(Sum(X1, Y1));
                Body2.Enqueue(Sum(X2, Y2));
                if (Over)
                {
                    Clock.Enabled = false;
                    ApplyChange(0, Block.End);
                    Palette.Image = null;
                    MessageBox.Show("游戏结束！", "提示");
                    Application.Exit();
                }
            }
            RefreshPalette();
        }
        private void GenerateFood(int Num)
        {
            int Seed = Guid.NewGuid().GetHashCode();
            int Food = new Random(Seed).Next(1200);
            if (Buffer[Food] != Block.Back) GenerateFood(Num);
            else ApplyChange(Food, Block.Food);
        }
        private void RefreshPalette()
        {
            Palette.Image = Panel;
            Draw = Graphics.FromImage(Palette.Image);
            for (int i = 0; i < 1200; i++)
            {
                int X = i % 40 * 20;
                int Y = i / 40 * 20;
                switch (Buffer[i])
                {
                    case Block.Back: Draw.DrawImage(BackDot, X, Y); break;
                    case Block.Head: Draw.DrawImage(HeadDot, X, Y); break;
                    case Block.Body: Draw.DrawImage(BodyDot, X, Y); break;
                    case Block.Tail: Draw.DrawImage(TailDot, X, Y); break;
                    case Block.Food: Draw.DrawImage(FoodDot, X, Y); break;
                    case Block.Dark: Draw.DrawImage(DarkDot, X, Y); break;
                }
            }
        }
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            byte[] Key = { (byte)e.KeyChar };
            bool KeyWS = e.KeyChar == 'w' | e.KeyChar == 's';
            bool KeyAD = e.KeyChar == 'a' | e.KeyChar == 'd';
            bool KeyIK = e.KeyChar == 'i' | e.KeyChar == 'k';
            bool KeyJL = e.KeyChar == 'j' | e.KeyChar == 'l';
            if (KeyWS | KeyAD)
            {
                switch (Dir1)
                {
                    case 'w': case 's': if (!KeyAD) return; break;
                    case 'a': case 'd': if (!KeyWS) return; break;
                }
                Key1 = e.KeyChar;
                if (Slave) Socket.Send(Key);
            }
            else if (KeyIK | KeyJL)
            {
                switch (Dir2)
                {
                    case 'i': case 'k': if (!KeyJL) return; break;
                    case 'j': case 'l': if (!KeyIK) return; break;
                }
                Key2 = e.KeyChar;
                if (Slave) Socket.Send(Key);
            }
        }
        private void ApplyChange(int X, int Y, Block Type)
        {
            byte[] Data = { (byte)X, (byte)Y, (byte)Type };
            Buffer[Sum(X, Y)] = Type;
            if (Master) Socket.Send(Data);
        }
        private void ApplyChange(int Num, Block Type)
        {
            int X = Num % 40, Y = Num / 40;
            byte[] Data = { (byte)X, (byte)Y, (byte)Type };
            Buffer[Num] = Type;
            if (Master) Socket.Send(Data);
        }
        private void ReceiveData()
        {
            try
            {
                if (Master)
                {
                    byte[] Data = new byte[1];
                    while (true)
                    {
                        Socket.Receive(Data, 1, SocketFlags.None);
                        bool KeyWS = Data[0] == 'w' | Data[0] == 's';
                        bool KeyAD = Data[0] == 'a' | Data[0] == 'd';
                        if (KeyWS | KeyAD) Key1 = (char)Data[0];
                        else Key2 = (char)Data[0];
                    }
                }
                else
                {
                    byte[] Data = new byte[3];
                    while (true)
                    {
                        Socket.Receive(Data, 3, SocketFlags.None);
                        int Food = Data[0] + Data[1] * 40;
                        if (Data[2] == 6)
                        {
                            Palette.Image = null;
                            Clock.Enabled = false;
                            MessageBox.Show("游戏结束！", "提示");
                            Application.Exit();
                        }
                        else Buffer[Food] = (Block)Data[2];
                    }
                }
            }
            catch
            {
                Application.Exit();
            }
        }
        private void MainForm_FormClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}