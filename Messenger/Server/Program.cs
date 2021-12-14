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
                Console.WriteLine("Сервер прекратил работу по причине\n"+ex);
                Console.ReadLine();
            }
        }
    }
}
