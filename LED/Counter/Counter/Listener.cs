using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Counter
{
    class Listener
    {
        Socket sock = null;
        private Socket m_Client=null;
        bool isListening = false;
        public int Port = 0;
        public Listener(int port)
        {
            try
            {
                Port = port;
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch
            {
                throw new Exception("创建套接字失败,请重启!");
            }
        }
        public bool StartListen()
        {
            bool flag = false;
            if (!isListening)
            {
                try
                {
                    sock.Bind(new IPEndPoint(0, Port));
                    sock.Listen(1);
                    sock.BeginAccept(AcceptCompelete, null);
                    isListening = true;
                    flag = true;
                }
                catch
                {
                    throw new Exception("启动监听失败,请重启！");
                }
            }
            return flag;
        }
        public void StopListen()
        {
            if(!isListening)
            {
                return;
            }
            m_Client.Close();
            m_Client.Dispose();
            m_Client = null;
            sock.Close();
            sock.Dispose();
            sock = null;
        }
        private void AcceptCompelete(IAsyncResult ar)
        {
            try
            {
                if (m_Client != null)
                {
                    return;
                }
                m_Client = this.sock.EndAccept(ar);
                m_Client.BeginReceive(new byte[] { },0,0,0,Recv,null);
             }
            catch
            {
                throw new Exception("链接异常，请重启！");
            }
        }
        private void CloseClient() {
            m_Client.Close();
            m_Client.Dispose();
            m_Client = null;
        }
        private void Recv(IAsyncResult ar) {
            try {
                int count = 0;
                m_Client.EndReceive(ar);
                byte[] buffer = new byte[1024];
                 count = m_Client.Receive(buffer,SocketFlags.None);
                if (count <= 0)
                {
                    CloseClient();
                    sock.BeginAccept(AcceptCompelete, null);
                    return;
                }
                Array.Resize(ref buffer,count);
                 GetMessage(Encoding.UTF8.GetString(buffer));
                m_Client.BeginReceive(new byte[] { },0,0,0,Recv,null);
            }
            catch{
                CloseClient();
                sock.BeginAccept(AcceptCompelete, null);
            }
        }
        public delegate void MessageRecv(string  msg);
        public event MessageRecv GetMessage;
    }
}
