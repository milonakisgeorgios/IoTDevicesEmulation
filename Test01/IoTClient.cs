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
