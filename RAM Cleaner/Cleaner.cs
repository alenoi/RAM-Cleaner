using System;
using System.Collections.Generic;
using System.Threading;

namespace RAM_Cleaner
{
    class Cleaner
    {
        ThreadStart TfillStart;
        Thread Tfill;
        ThreadStart TgetramStart;
        Thread Tgetram;
        ThreadStart TdisplayStart;
        Thread Tdisplay;
        ThreadStart TinputStart;
        Thread Tinput;

        Text text = new Text();
        private List<string[]> dispList = new List<string[]>();
        private List<byte[]> alist = new List<byte[]>();
        private DateTime enddate;
        private DateTime startdate;
        private Int64 startphav;
        private Int64 endphav;
        private Int64 phav;
        private Int64 tot;
        private double percFree;
        private double percUsed;
        private string state;
        private string option;

        private double size;
        private double barlenght;
        private int wait;
        private int i;
        private bool end = false;
        private bool Tstop = false;

        public DateTime StartDate { get => startdate; }
        public long Phav { get => phav; }
        public long Tot { get => tot; }
        public double Size { get => size; }
        public double Barlenght { get => barlenght; }
        public int Wait { get => wait; }
        public double PercFree { get => percFree; }
        public double PercUsed { get => percUsed; }
        public bool End { get => end; }
        public string State { get => state; }
        public bool Tstop1 { get => Tstop; set => Tstop = value; }

        public void Control()
        {
            Console.Clear();
            state = "start";
            text.Init();
            Init();
            Tgetram.Start();
            Tdisplay.Start();
            Tinput.Start();
        }
        public void Init()
        {
            state = "init";
            Console.CursorVisible = false;

            TgetramStart = new ThreadStart(GetRam);
            Tgetram = new Thread(TgetramStart);
            Tgetram.IsBackground = true;

            TfillStart = new ThreadStart(Fill);
            Tfill = new Thread(TfillStart);
            Tfill.IsBackground = true;

            TdisplayStart = new ThreadStart(Display);
            Tdisplay = new Thread(TdisplayStart);
            Tdisplay.IsBackground = true;

            TinputStart = new ThreadStart(Input);
            Tinput = new Thread(TinputStart);
            Tinput.IsBackground = true;

            displayAdd("input", text.Inputtxt + text.InputAsk);


        }

        private void RAMstatTable()
        {
            string table;
            table = "\n";
            table += text.TableMaker(Tot, Phav, PercFree, PercUsed) + "\n";

            displayAdd("ramtable", table);
        }



        private void GetRam()
        {
            while (!Tstop)
            {
                tot = PerformanceInfo.GetTotalMemoryInMiB();
                phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
                percFree = Math.Round(((double)phav / tot) * 100, 0);
                percUsed = Math.Round(100 - percFree, 0);
                barlenght = (tot / size) * 1.1;
                Thread.Sleep(1000);
            }
        }

        private void Display()
        {

            while (!Tstop)
            {
                Console.SetCursorPosition(0, 0);
                RAMstatTable();
                switch (state)
                {
                    default:
                        break;
                    case "start":
                        break;
                    case "init":
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(displayGet("ramtable"));
                        break;
                    case "input":
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(displayGet("ramtable"));
                        Console.SetCursorPosition(0, 9);
                        Console.WriteLine(displayGet("input"));
                        break;
                    case "clean":
                        CleanInfo();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(displayGet("ramtable"));
                        Console.SetCursorPosition(0, 10);
                        Console.WriteLine(displayGet("cleaninfo"));
                        Console.SetCursorPosition(0, 14);
                        Console.WriteLine(displayGet("bar"));
                        break;
                    case "end":
                        CleanEnd();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(displayGet("ramtable"));
                        Console.SetCursorPosition(0, 10);
                        Console.WriteLine(displayGet("cleanend"));
                        Thread.Sleep(1000);
                        break;
                }

            }
        }

        private void CleanInfo()
        {
            string row1 = "\nRAM usage before: " + (100 - Math.Round((double)startphav / tot * 100, 2)) + " %";
            string row2 = "Clean started: " + startdate + "  |  Elapsed time: " + (DateTime.Now - startdate).ToString().Substring(3, 5);

            string cleaninfo = option + "\n" + row1 + "\n" + row2 + "\n";

            DrawBar();
            displayAdd("cleaninfo", cleaninfo);
        }
        private void CleanEnd()
        {
            string row1 = "\nRAM usage before: " + (100 - Math.Round((double)startphav / tot * 100, 2)) + " %";
            string row2 = "Clean started: " + startdate + "  |  Elapsed time: " + (enddate - startdate).ToString().Substring(3, 5);

            row1 += "  |  RAM usage after: " + (100 - Math.Round(endphav / (decimal)tot * 100, 2)) + " %";
            row2 += "  |  Clean finished: " + enddate;

            string cleanend = option + "\n" + row1 + "\n" + row2 + "\n\n\n\n\n" + text.Exit;

            displayAdd("cleanend", cleanend);
        }

        private void Input()
        {
            state = "input";

            ConsoleKey key = waitforkeypress();

            if (state == "input" && (key == ConsoleKey.NumPad1 || key == ConsoleKey.D1))
            {
                startdate = DateTime.Now;
                option = "Fast Clean";
                size = 100;
                wait = 100;
                Tfill.Start();
                Tfill.Join();
            }
            else if (state == "input" && (key == ConsoleKey.NumPad2 || key == ConsoleKey.D2))
            {
                startdate = DateTime.Now;
                option = "Normal Clean";
                size = 100;
                wait = 1000;
                Tfill.Start();
                Tfill.Join();
            }
            else if (state == "input" && (key == ConsoleKey.NumPad3 || key == ConsoleKey.D3))
            {
                startdate = DateTime.Now;
                option = "Deep Clean";
                size = 1;
                wait = 10;
                Tfill.Start();
                Tfill.Join();
            }

        }

        private static ConsoleKey waitforkeypress()
        {
            ConsoleKey key = ConsoleKey.NumPad0;
            while (true)
            {
                key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.NumPad1 || key == ConsoleKey.NumPad2 || key == ConsoleKey.NumPad3 || key == ConsoleKey.D1 || key == ConsoleKey.D2 || key == ConsoleKey.D3)
                {
                    break;
                }
            }
            return key;
        }

        public void Fill()
        {
            state = "clean";
            GC.Collect();
            Console.Clear();
            startphav = phav;
            for (i = 0; i <= barlenght; i++)
            {
                if (phav > size * 3)
                {
                    RamFill(ref alist, Size * 1024 * 1024);
                }
                else
                { Thread.Sleep(wait); }
            }
            state = "end";
            enddate = DateTime.Now;
            Clear();
        }
        public void Clear()
        {
            RamClear(ref alist);
            GC.Collect();
            Thread.Sleep(500);
            endphav = phav;
        }
        private static void RamFill(ref List<byte[]> alist, double size)
        {
            byte[] data = new byte[(int)size];
            Array.Clear(data, 0, data.Length);
            alist.Add(data);
        }
        private static void RamClear(ref List<byte[]> alist)
        {
            alist.Clear();
        }
        private void displayAdd(string type, string todisplay)
        {
            bool found = false;
            foreach (var dispitem in dispList)
            {
                if (dispitem[0] == type)
                {
                    dispitem[1] = todisplay;
                    found = true;
                }
            }
            if (!found)
            {
                string[] dispstring = new string[2];
                dispstring[0] = type;
                dispstring[1] = todisplay;
                dispList.Add(dispstring);
            }
        }
        private string displayGet(string type)
        {
            string todisplay = "Where is [" + type + "]";
            foreach (var dispitem in dispList)
            {
                if (dispitem[0] == type)
                {
                    todisplay = dispitem[1];
                }
            }
            return todisplay;
        }

        private void DrawBar()
        {
            string bar = "[";
            double v = (i + 1) / barlenght;
            double v1 = v * 100;

            for (int i = 0; i < v1; i++)
            {
                bar += "#";
            }

            for (int i = 0; i < 100 - v1; i++)
            {
                bar += "-";
            }
            bar += $"] {Math.Round(v1 - 0.5)} %";

            displayAdd("bar", bar);
        }
    }
}
