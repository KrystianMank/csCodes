using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lekcja_08_serwerTCP
{
    class TcpServer
    {
        private TcpListener _listener;

        public TcpServer()
        {
            _listener = new TcpListener(IPAddress.Any, 9000);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine("Serwer TCP nasłuchuje na porice 9000...");
            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Otrzymano wiadomość: {receivedMessage}");

                //odpowiedź do klienta
                string responseMessage = "Wiadomość odebrana!";
                byte[] responseData = Encoding.UTF8.GetBytes(receivedMessage);
                await stream.WriteAsync(responseData, 0, responseData.Length);


            }
            client.Close();
        }
        //Main
        static async Task Main(string[] args)
        {
            TcpServer server = new TcpServer();
            await server.StartAsync(); //uruchomienie serwera
        }
    }
}