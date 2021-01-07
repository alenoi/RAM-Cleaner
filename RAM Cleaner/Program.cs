using System;
using System.Threading;

namespace RAM_Cleaner
{
    class Program
    {

        static void Main(string[] args)
        {
            Text text = new Text();
            text.Init();
            Console.Title = text.Title;
            ;
            Cleaner cleaner = new Cleaner();
            Thread i = new Thread(new ThreadStart(cleaner.Control));
            i.Start();


            while (cleaner.State != "end")
            {
                Thread.Sleep(1000);
            }

            ConsoleKey key = waitforkeypress();
            if (key == ConsoleKey.Escape)
            {
                return;
            }
            else if (key == ConsoleKey.Enter)
            {
                cleaner.Tstop1 = true;
                Console.Clear();
                Main(args);
            }
        }
        static ConsoleKey waitforkeypress()
        {
            ConsoleKey key = ConsoleKey.NumPad0;
            while (true)
            {
                key = Console.ReadKey(false).Key;
                if (key == ConsoleKey.Escape || key == ConsoleKey.Enter)
                {
                    break;
                }
            }
            return key;
        }
    }

}
