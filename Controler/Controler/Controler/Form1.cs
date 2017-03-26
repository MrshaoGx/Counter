using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Controler
{
    public partial class Form1 : Form
    {
        Socket sock = null;
        bool isConnected = false;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Connnect_Click(object sender, EventArgs e)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string iptxt = txt_ip.Text.ToString().Trim();
            IPAddress ip;
            try
            {
                ip = IPAddress.Parse(iptxt);
            }
            catch {
                MessageBox.Show("ip地址填写错误，请重试");
                return;
            }
            try {
                sock.Connect(new IPEndPoint(ip,18888));
                isConnected = true;
                txt_ip.Enabled = false;
                Connnect.Visible = false;
                Disconnect.Visible = true;
            }
            catch { MessageBox.Show("链接错误，请确保演示端已开启，稍后重试！"); }
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            if (sock != null)
            {
                sock.Close();
            }
            txt_ip.Enabled = true;
            Connnect.Visible = true;
            Disconnect.Visible = false;
        }
        private void SendData(string msg) {
            try
            {
                if (!isConnected)
                {
                    MessageBox.Show("尚未链接，请重试！");
                    return;
                }
                sock.Send(Encoding.UTF8.GetBytes(msg));
            }
            catch {
                MessageBox.Show("发送异常，请重试！");
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            txt1.Text = "0";
            txt2.Text = "0";
            txt3.Text = "0";
            txt4.Text = "0";
            txt5.Text = "0";
            txt6.Text = "0";
        }
        private void PerparSend(string gourp,string value,bool optype) {
            string msg = gourp + "-" + value + "-"+"add";
            SendData(msg);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PerparSend("0",txt1.Text.Trim(),true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PerparSend("1", txt2.Text.Trim(), true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PerparSend("2", txt3.Text.Trim(), true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PerparSend("3", txt4.Text.Trim(), true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PerparSend("4", txt5.Text.Trim(), true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PerparSend("5", txt6.Text.Trim(), true);
        }

        private void txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }
    }
}
