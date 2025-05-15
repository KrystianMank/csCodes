using System.Net.Sockets;
using System.Text;

namespace UdpServerApp
{
    class UdpServer
    {
        private UdpClient _client;

        public UdpServer()
        {
            _client = new UdpClient(9001);
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Serwer UDP nasłuchuje na porcie 9001...");

            while (true)
            {
                UdpReceiveResult receivedResult = await _client.ReceiveAsync();
                string receivedMessage = Encoding.UTF8.GetString(receivedResult.Buffer);
                Console.WriteLine($"Otrzymano wiadomość: {receivedMessage}");
                
                //odpowiedź do klienta
                string responseMessage = "Wiadomość odebrana!";
                byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);

                //wysyłanie odpowiedźi do klienta
                await _client.SendAsync(responseData, responseData.Length, receivedResult.RemoteEndPoint);
            }
        }
        static async Task Main(string[] args)
        {
            UdpServer server = new UdpServer();
            await server.StartAsync(); //uruchomienie serwera UDP
        }
    }
}