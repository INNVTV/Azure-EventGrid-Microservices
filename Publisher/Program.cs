using System;
using System.Threading;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                //Send a "TopicOne" notification every 15 seconds
                // and a "TopicTwo" notification every 30 seconds:

                Thread.Sleep(15000);
                Console.WriteLine("Sending 'TopicOne' event to the grid...");

                Thread.Sleep(15000);
                Console.WriteLine("Sending 'TopicOne' event to the grid...");
                Console.WriteLine("Sending 'TopicTwo' event to the grid...");

            }
        }
    }
}
