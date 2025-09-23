using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Emulator2
{
    internal class IoTClient : IDisposable
    {
        string serverIP;
        int serverPort;
        Socket? m_csocket;
        readonly Object _sync = new object();
        Thread? m_thread;
        readonly byte[] _rcvBuffer = new byte[2048];
        readonly ManualResetEvent m_stopEvent;
        int _disposed = 0;// Whether Dispose has been called.
        Action<byte[], int>? ReceiveDataProcessor;



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
        public IoTClient(string serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;

            m_stopEvent = new ManualResetEvent(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="serverPort"></param>
        /// <param name="processor"></param>
        public IoTClient(string serverIP, int serverPort, Action<byte[], int> processor)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            this.ReceiveDataProcessor = processor;

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
            catch (Exception ex)
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

                    var numOfBytes = m_csocket!.Receive(_rcvBuffer, 0, _rcvBuffer.Length, SocketFlags.None);

                    if (numOfBytes > 0)
                    {
                        if(this.ReceiveDataProcessor != null)
                        {
                            this.ReceiveDataProcessor.Invoke(_rcvBuffer, numOfBytes);
                        }
                        else
                        {
                            string response = Encoding.UTF8.GetString(_rcvBuffer, 0, numOfBytes);
                            Console.WriteLine("Received: {0}", response);
                        }
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



        public void Send(byte[] packetData)
        {
            if(this.m_csocket!= null)
            {
                this.m_csocket.Send(packetData);
            }
        }
    }
}
