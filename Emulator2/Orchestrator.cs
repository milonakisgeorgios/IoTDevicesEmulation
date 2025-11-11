using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator2
{
    internal class Orchestrator
    {
        string _svrIP = "127.0.0.1";
        int _svrPORT = 2001;
        ManualResetEvent quitEvent = new ManualResetEvent(false);
        int _state = 0;


        void PrintInitialMenu()
        {
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\t*        1 -> Connect & Listen Cmds"); 
            Console.WriteLine("\t\t*        2 -> Connect & Send packets");
            Console.WriteLine("\t\t*        3 -> Connect & Run scenarios");
            Console.WriteLine("\t\t*        4 -> Firmware Upgrade");
            Console.WriteLine("\t\t*        Q -> Quit Emulator");
            Console.WriteLine("\t\t*******************************************");
        }

        public string InitialMenuLoop()
        {
            while (true)
            {
                PrintInitialMenu();
                var input1 = Console.ReadKey(true).KeyChar.ToString().ToUpperInvariant();

                if (input1 == "Q")
                {
                    return "Q";
                }
                if (input1 == "1")
                {
                    return "1";
                }
                if (input1 == "2")
                {
                    return "2";
                }
                if (input1 == "3")
                {
                    return "3";
                }
                if (input1 == "4")
                {
                    return "4";
                }
            }
        }






        public void Start()
        {
            while (true)
            {
                Console.Clear();
                if(_state == 0)
                {
                    var input = InitialMenuLoop();
                    if (input == "Q")
                        break;
                    if (input == "1")
                        _state = 1;
                    else if(input == "2")
                        _state = 2;
                    else if (input == "3")
                        _state = 3;
                    else
                        _state = 4;
                    continue;
                }

                if(_state == 1)
                {
                    try
                    {
                        var emulator = new Emulator1(_svrIP, _svrPORT);
                        emulator.Start();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    _state = 0;
                }

                if (_state == 2)
                {
                    try
                    {
                        var emulator = new Emulator2(_svrIP, _svrPORT);
                        emulator.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    _state = 0;
                }

                if (_state == 3)
                {
                    try
                    {
                        var emulator = new Emulator3(_svrIP, _svrPORT);
                        emulator.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    _state = 0;
                }
                if (_state == 4)
                {
                    try
                    {
                        var emulator = new FirmwareUpgradeEmulator(_svrIP, _svrPORT);
                        emulator.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    _state = 0;
                }

            }


            Console.WriteLine();
            Console.WriteLine("Emulator closed...");
        }
    }
}
