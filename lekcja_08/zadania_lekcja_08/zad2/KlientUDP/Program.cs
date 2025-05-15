using System.Net.Sockets;
using System.Text;

namespace UdpKlientApp
{
    class UDPKlient
    {
        public UDPKlient() { }

        static async Task Main(string[] args)
        {
            try
            {
                UdpClient udpClient = new UdpClient();
                while (true)
                {
                    
                    //wysyłanie wiadomości
                    Console.Write("Wpisz wiadomość: ");
                    string message = Console.ReadLine() + "";
                    byte[] bytes = Encoding.UTF8.GetBytes(message);
                    await udpClient.SendAsync(bytes, bytes.Length, "192.168.1.15", 9000);

                    //odbieranie wiadomości
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    string receivedMessage = Encoding.UTF8.GetString(result.Buffer);
                    Console.WriteLine($"Odpowiedź serwera: {receivedMessage}");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}