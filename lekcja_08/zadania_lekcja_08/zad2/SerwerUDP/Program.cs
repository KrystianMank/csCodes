using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpSerwerApp
{
    class UDPSerwer
    {
        static async Task Main(string[] args)
        {
            using (UdpClient udpServer = new UdpClient(9000)) // Nasłuch na porcie 9000
            {
                Console.WriteLine("Serwer nasłuchuje na porcie 9000...");

                while (true) // Pętla nasłuchiwania
                {
                    try
                    {
                        // Odbiór wiadomości od dowolnego klienta
                        UdpReceiveResult result = await udpServer.ReceiveAsync();
                        string receivedMessage = Encoding.UTF8.GetString(result.Buffer);
                        Console.WriteLine($"Otrzymano wiadomość od {result.RemoteEndPoint}: {receivedMessage}");

                        // Przygotowanie odpowiedzi
                        string responseMessage = "Wiadomość otrzymana: " + receivedMessage;
                        byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);

                        // Odpowiedź do klienta, który wysłał wiadomość
                        await udpServer.SendAsync(responseBytes, responseBytes.Length, result.RemoteEndPoint);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                    }
                }
            }
        }
    }
}
