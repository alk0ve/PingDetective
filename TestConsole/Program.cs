using System;
using libping;


namespace TestConsole
{
    /*
     * I'm using this class for quick and dirty testing.
     */
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var hostname = args[0];
                Console.WriteLine("Pinging {0}", hostname);

                if (libping.SimplePing.isAccessible(hostname)) {
                    Console.WriteLine("{0} is accessible :)", hostname);
                }
                else {
                    Console.WriteLine("{0} is NOT accessible...", hostname);
                }
            }
        }
    }
}

