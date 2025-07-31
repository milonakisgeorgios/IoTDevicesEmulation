using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test01
{
    internal class IoTClient : IDisposable
    {
        int _disposed = 0;// Whether Dispose has been called.
        Thread? m_thread;
        Socket? m_csocket;
        System.Random rnd = new Random();
        ManualResetEvent m_quitEvent;
        readonly Object _sync = new object();
        readonly ManualResetEvent m_stopEvent;
        readonly byte[] _rcvBuffer;
        string serverIP;
        int serverPort;


        public DateTime LastConnectDT { get; protected set; } = DateTime.MinValue;
        public DateTime LastDisconnectDT { get; protected set; } = DateTime.MinValue;

        public bool IsConnected
        {
            get
            {
                if (m_csocket != null)
                {
                    lock (_sync)
                    {
                        if (m_csocket != null)
                        {
                            return m_csocket.Connected;
                        }
                    }
                }
                return false;
            }
        }


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

            _rcvBuffer = new byte[2048];

            m_stopEvent = new ManualResetEvent(false);
        }


        #region Dispose
        /// <summary>
        /// 
        /// </summary>
        ~IoTClient() => Dispose(false);


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return;
            }


            if (disposing)
            {
                //From user code...
                Console.WriteLine("Dispose(disposing = true)");
            }
            else
            {
                Console.WriteLine("Dispose(disposing = false)");
            }

            _CloseAndInitialize();

            _disposed = 1;
        }

        void ThrowIfDisposed()
        {
            if (_disposed != 0)
            {
                ThrowObjectDisposedException();
            }

            void ThrowObjectDisposedException() => throw new ObjectDisposedException(GetType().FullName);
        }
        #endregion


        void _CloseAndInitialize()
        {
            m_stopEvent.Set();

            if (m_csocket != null)
            {
                Disconnect();
                Thread.Sleep(100);
            }

            if (m_thread?.IsAlive == true)
            {
                m_thread?.Join(1000);
            }

            m_stopEvent.Reset();
        }


        public void Connect()
        {
            ThrowIfDisposed();
            _CloseAndInitialize();

            try
            {
                Console.WriteLine($"Try to connect to {this.serverIP}:{this.serverPort}");

                lock (_sync)
                {
                    m_csocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_csocket.NoDelay = true;
                    m_csocket.Connect(new IPEndPoint(IPAddress.Parse(this.serverIP), this.serverPort));

                    Console.WriteLine($"Connected to {m_csocket.RemoteEndPoint}");


                    //Κραταμε τι ωρα συνδεθηκαμε
                    LastConnectDT = DateTime.Now;

                    //Και τωρα δημιουργουμε το νημα που διαβαζει συνεχως τα μηνυματα που ερχονται...
                    m_thread = new Thread(new ThreadStart(this._ReceiveLoop));
                    m_thread.Name = "_ReceiveLoop";
                    m_thread.Start();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void Stop()
        {
            Console.WriteLine($"Stop()");

            ThrowIfDisposed();

            try
            {

                _CloseAndInitialize();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="invokeEvent"></param>
        /// <param name="errorcode"></param>
        void Disconnect(bool invokeEvent = false, int errorcode = 0)
        {
            if (Thread.CurrentThread.IsThreadPoolThread)
                Console.WriteLine($"Disconnect() called by ThreadPoolThread (Id = {Thread.CurrentThread.ManagedThreadId}).");
            else
                Console.WriteLine($"Disconnect() called by '{Thread.CurrentThread.Name}'.");

            if (m_csocket != null)
            {
                lock (_sync)
                {
                    if (m_csocket != null)
                    {
                        LastDisconnectDT = DateTime.Now;

                        try
                        {
                            try
                            {
                                /*
                                 * When using a connection-oriented Socket, always call the 
                                 * Shutdown method before closing the Socket. This ensures 
                                 * that all data is sent and received on the connected socket 
                                 * before it is closed.
                                 */
                                m_csocket.Shutdown(SocketShutdown.Both);
                            }
                            catch (SocketException ex)
                            {
                                Console.WriteLine($"At m_socket.Shutdown(), SocketException with ErrorCode={ex.ErrorCode}, Message='{ex.Message},'!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"At m_socket.Shutdown(), Exception with Message='{ex.Message}'!");
                            }
                            finally
                            {
                                m_csocket.Close();
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine($"In Disconnect(), SocketException with ErrorCode={ex.ErrorCode}, Message='{ex.Message},'!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"In Disconnect(), Exception with Message='{ex.Message}'!");
                        }


                        m_csocket = null;


                        if (Thread.CurrentThread.IsThreadPoolThread)
                            Console.WriteLine($"Disconnected..... by ThreadPoolThread (Id = {Thread.CurrentThread.ManagedThreadId}).");
                        else
                            Console.WriteLine($"Disconnected..... by '{Thread.CurrentThread.Name}'.");
                    }
                }
            }
        }





        void _ReceiveLoop()
        {
            Console.WriteLine("_ReceiveLoop Started");

            try
            {
                while (true)
                {
                    if (m_stopEvent.WaitOne(0))
                    {
                        Console.WriteLine("m_stopEvent isSet!");
                        break;
                    }

                    var numOfBytes = m_csocket!.Receive(_rcvBuffer, _rcvBuffer.Length, SocketFlags.None);

                    if (numOfBytes > 0)
                    {
                        string response = Encoding.UTF8.GetString(_rcvBuffer, 0, numOfBytes);
                        Console.WriteLine("Received: {0}", response);
                    }
                    else
                    {
                        Console.WriteLine("Received 0 bytes!");

                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(string.Format("_ReceiveLoop() -> ErrorCode= {0}, Message = {1}", ex.ErrorCode, ex.Message));
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine(string.Format("_ReceiveLoop() -> {0}", ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("_ReceiveLoop() -> {0}", ex.Message));
            }

            Disconnect(invokeEvent: true, errorcode: 0);
            Console.WriteLine("_ReceiveLoop Terminated");
        }




        public void Send1a(bool show=true)
        {
            if (m_csocket != null)
            {
                string message = "210B0185E24D680005000100CD05";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send1b(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "210B0185E24D680005000100CD05C67907D49CDC390003000BE1EB4102C4";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send1c(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "C8790ADD9CDC39000000240580E74D686309790985099209A509BA097CE94D68A90900DFB6E25CABED07CBE24D6800030145F7AAC28502AFED010FE34D680005000000C30D";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send1d(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "B3ED014BE34D680005000000D9EF1CA508A6E34D68000000C86BC7420100E506BA420200E27BC0420300F05DC14204005BFDBA422C0112AB2A0B0772E34D68000300DB3EE5416DCA2C0B0A75E34D680000002405D8E94D6858096309790985099209A50958EA4D68770900DFA286E9";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send1e(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "A93A0A559451680000002405609A5168B80C9E0DED0DF40D020EDA0D309B5168620C00DF9C435AAA3A045594516800012405000001ED39AB3A015594516800050001006273AC3A078094516800030027B5134205CDAD3A07809451680003018BE6A4C2FDA8AE3A0A919451680000002405609A5168B80C9E0DED0DF40D020EDA0D589B5168620C00DF9B54A0AF3A019194516800050001003202B03A04CD945168000124050000008B8CB23A01CD94516800050000006DAAB33A07F8945168000300FFC612422A16B43A07F89451680003018BE6A4C21CE7B53A0A099551680000002405609A5168B80C9E0DED0DF40D020EDA0DF89B5168620C00DF9B8517B63A040995516800012405000001B064B73A01099551680005000100AAAC";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send2a(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "D37907C49DDC3900";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send2b(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "0301BEE6A4C20560";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send3a(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "D37907C49DDC39000";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        public void Send3b(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "301BEE6A4C20560";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }
        public void Send4a(bool show = true)
        {
            if (m_csocket != null)
            {
                string part1 = "6383097F1C3A68000";
                string part2 = "2001B799C42D1459642444EA342353BAC4"; 
                string part3 = "2618BAA4242D0AE420896AD423770A7425D73A8421E64AE42EF41AB42A";
                string part4 = "105A442A7F2AA42033AA042FF50AB42D6E";
                string part5 = "4A0425CE9AC42FDFDAF42A65091429CBE9E428284A44256E6A342E38AA94213D89242ED2D89422C01B44";
                string part6 = "6";

                byte[] data = Encoding.UTF8.GetBytes(part1);
                m_csocket.Send(data);
                if (show) Console.Write("Sent: {0}", part1);
                Thread.Sleep(500);

                data = Encoding.UTF8.GetBytes(part2);
                m_csocket.Send(data);
                if (show) Console.Write(part2);
                Thread.Sleep(500);

                data = Encoding.UTF8.GetBytes(part3);
                m_csocket.Send(data);
                if (show) Console.Write(part3);
                Thread.Sleep(500);

                data = Encoding.UTF8.GetBytes(part4);
                m_csocket.Send(data);
                if (show) Console.Write(part4);
                Thread.Sleep(500);
                data = Encoding.UTF8.GetBytes(part5);
                m_csocket.Send(data);
                if (show) Console.Write(part5);
                Thread.Sleep(800);
                data = Encoding.UTF8.GetBytes(part6);
                m_csocket.Send(data);
                if (show) Console.Write(part6);

                if (show) Console.WriteLine("\nDONE!");
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }



        /// <summary>
        /// 
        /// 0x0A with 1 repeated blocks
        /// </summary>
        public void Send4b(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "582B0ADA85D6390000002405E8D04768CF0AA60A750A4F0A430A380A6CD247680C0B00DFB4E048";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        /// <summary>
        /// 
        /// 0x0A with 2 repeated blocks
        /// </summary>
        public void Send4c(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "D5920A2BA9CA390000001E05B8EE3B68510925090E09D0086B080E080FF13B687E0900DBA30000240568F33B684F083B0822080208EC07CC078CF53B68910800E0B4C800";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        /// <summary>
        /// 0x0A with 3 repeated blocks
        /// </summary>
        public void Send4d(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "78B70AF17DCD390000002305A0C73E68B60C8F0C870C580C3D0C370C99C93E680A0000DE9A00002405F8C93E689F0A6A0A590A480A2B0A1D0A64CA3E68C20A01E0B200002505A0C73E685A0DBF0DBF0DC70DBD0D110EE6C73E68150000DE9CE19F";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }

        /// <summary>
        /// 2A0B0772E34D68000300DB3EE5416DCA
        /// BAD CRC
        /// </summary>
        public void Send5a(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "2A0B0772E34D68000300DB3EE5416DCC";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        /// <summary>
        /// 2A0B0772E34D68000300DB3EE5416DCA
        /// BAD CONTENT
        /// </summary>
        public void Send5b(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "2A0B0772H34D68000300DB3EE5416DCA";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        /// <summary>
        /// 2A0B0772E34D68000300DB3EE5416DCA
        /// BAD DELIMITER
        /// </summary>
        public void Send5c(bool show = true)
        {
            if (m_csocket != null)
            {
                string message = "2A0BBC72E34D68000300DB3EE5416DCA";
                byte[] data = Encoding.UTF8.GetBytes(message);
                m_csocket.Send(data);
                if (show) Console.WriteLine("Sent: {0}", message);
            }
            else
            {
                if (show) Console.WriteLine("Client is not connected");
            }
        }
        /// <summary>
        /// 522B0A6285D63900 00002405E8D04768CF0AA60A750A4F0A430A380AF4D14768CD0A00DFB4 DE6F
        /// BUFFER OVERFLOW
        /// </summary>
        public void Send5d(bool show = true)
        {
            if (m_csocket != null)
            {
                string part1 = "522B0A6285D6390000002405E8D04768CF0AA60A750A4F0A430A380AF4D14768CD0A00DFB4";
                byte[] data = Encoding.UTF8.GetBytes(part1);
                for (int i=1; i<=1000; i++)
                {
                    m_csocket.Send(data);

                    if (show) Console.Write(".", part1);
                }
                if (show) Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }

        /// <summary>
        /// 522B0A6285D63900 00002405E8D04768CF0AA60A750A4F0A430A380AF4D14768CD0A00DFB4 DE6F
        /// BUFFER OVERFLOW
        /// </summary>
        public void Send5e(bool show = true)
        {
            if (m_csocket != null)
            {
                string part1 = "522B0A6285D63900";
                byte[] data = Encoding.UTF8.GetBytes(part1);
                m_csocket.Send(data);
                Console.Write("Sent: {0}", part1);

                string part2 = "00002405E8D04768CF0AA60A750A4F0A430A380AF4D14768CD0A00DFB4";
                byte[] data2 = Encoding.UTF8.GetBytes(part2);
                for (int i = 1; i <= 1000; i++)
                {
                    m_csocket.Send(data2);

                    if (show) Console.Write(".", part2);
                }
                if (show) Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }


        public void del(int delim, bool show = true)
        {
            if (m_csocket != null)
            {
                IList<string> sample = Samples.sample1;
                if (delim == 1)
                {
                    sample = Samples.sample1;
                }
                else if (delim == 2)
                {
                    sample = Samples.sample2;
                }
                else if (delim == 3)
                {
                    sample = Samples.sample3;
                }
                else if (delim == 4)
                {
                    sample = Samples.sample4;
                }
                else if (delim == 5)
                {
                    sample = Samples.sample5;
                }
                else if (delim == 6)
                {
                    sample = Samples.sample6;
                }
                else if (delim == 7)
                {
                    sample = Samples.sample7;
                }
                else if (delim == 8)
                {
                    sample = Samples.sample8;
                }
                else if (delim == 9)
                {
                    sample = Samples.sample9;
                }
                else if (delim == 101)
                {
                    sample = Samples.sample10a;
                }
                else if (delim == 102)
                {
                    sample = Samples.sample10b;
                }
                else if (delim == 103)
                {
                    sample = Samples.sample10c;
                }
                else if (delim == 11)
                {
                    sample = Samples.sample11;
                }
                else if (delim == 12)
                {
                    sample = Samples.sample12;
                }
                else if (delim == 13)
                {
                    sample = Samples.sample13;
                }
                else if (delim == 14)
                {
                    sample = Samples.sample14;
                }

                foreach (var s in sample)
                {
                    byte[] data = Encoding.UTF8.GetBytes(s);
                    m_csocket.Send(data);
                    if(show) Console.WriteLine("Sent: {0}", s);
                    //Console.Write("Press a key to continue...");
                    //Console.ReadKey();
                    Thread.Sleep(50);
                }
                if (show) Console.WriteLine("Done!");
            }
            else
            {
                Console.WriteLine("Client is not connected");
            }
        }

    }
}
