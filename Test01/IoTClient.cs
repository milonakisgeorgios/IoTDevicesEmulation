using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Test01
{
    internal class IoTClient
    {
        Socket? m_csocket;
        System.Random rnd = new Random();
        ManualResetEvent m_quitEvent;
        string serverIP;
        int serverPort;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="serverPort"></param>
        /// <param name="quitEvent"></param>
        public IoTClient(string serverIP, int serverPort, ManualResetEvent quitEvent)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            m_quitEvent = quitEvent;
        }

        public void Connect()
        {
            m_csocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_csocket.Connect(new IPEndPoint(IPAddress.Parse(this.serverIP), this.serverPort));
                Console.WriteLine("Connected to server.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send1()
        {
            if (m_csocket != null)
            {
                string message = "D30B07331C3A680003018BE6A4C23F1A";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }

        public void Send2()
        {
            if (m_csocket != null)
            {
                string message = "D30B07331C3A680003018BE6A4C23F1AD30B07331C3A680003018BE6A4C23F1A";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }
        public void Send3a()
        {
            if (m_csocket != null)
            {
                string message = "D30B07331C3A68000301";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }
        public void Send3b()
        {
            if (m_csocket != null)
            {
                string message = "8BE6A4C23F1A";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }
        public void Send4()
        {
            if (m_csocket != null)
            {
                string message = "6383097F1C3A680002001B799C42D1459642444EA342353BAC42618BAA4242D0AE420896AD423770A7425D73A8421E64AE42EF41AB42A105A442A7F2AA42033AA042FF50AB42D6E4A0425CE9AC42FDFDAF42A65091429CBE9E428284A44256E6A342E38AA94213D89242ED2D89422C01B446";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }
        public void Send5()
        {
            if (m_csocket != null)
            {
                string message = "Hello, Server!";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }
        public void Drop()
        {
            if(m_csocket != null)
            {
                m_csocket.Close();
                m_csocket = null;
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }


        public void Quit()
        {
            if (m_csocket != null)
            {
                m_csocket.Close();
                m_csocket = null;
            }
        }

        //// Send message
        //string message = "Hello, Server!";
        //byte[] data = Encoding.UTF8.GetBytes(message);
        //clientSocket.Send(data);
        //    Console.WriteLine("Sent: {0}", message);

        //    // Receive response
        //    byte[] buffer = new byte[256];
        //int bytesRead = clientSocket.Receive(buffer);
        //string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //Console.WriteLine("Received: {0}", response);

        //    // Close socket
        //    clientSocket.Close();



    }
}
