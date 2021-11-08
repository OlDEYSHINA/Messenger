using System;

namespace Server
{
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var networkManager = new NetworkManager();
                networkManager.Start();

                Console.ReadLine();

                networkManager.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
