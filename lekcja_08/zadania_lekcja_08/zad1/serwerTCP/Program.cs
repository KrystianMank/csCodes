using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
    static void Main()
    {
        TcpListener server = null;
        try
        {
            // Ustawienie portu 8000 i nasłuchiwanie na wszystkich dostępnych interfejsach
            int port = 8000;
            IPAddress localAddr = IPAddress.Any;
            server = new TcpListener(localAddr, port);

            // Rozpoczęcie nasłuchiwania
            server.Start();
            Console.WriteLine("Serwer nasłuchuje na porcie 8000...");

            // Akceptacja połączenia od klienta
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Połączono z klientem.");

            // Używanie strumienia do komunikacji
            NetworkStream stream = client.GetStream();

            // Wysłanie powitania do klienta
            string welcomeMessage = "Witaj na serwerze TCP!";
            byte[] welcomeBytes = Encoding.UTF8.GetBytes(welcomeMessage);
            stream.Write(welcomeBytes, 0, welcomeBytes.Length);
            Console.WriteLine("Wysłano powitanie do klienta.");

            // Odbiór wiadomości od klienta
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Otrzymano wiadomość od klienta: " + clientMessage);

            // Wysłanie odpowiedzi zwrotnej do klienta
            string responseMessage = "Wiadomość odebrana: " + clientMessage;
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            stream.Write(responseBytes, 0, responseBytes.Length);
            Console.WriteLine("Wysłano odpowiedź do klienta.");

            // Zamknięcie połączenia
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Wystąpił błąd: " + e.Message);
        }
        finally
        {
            // Zamknięcie serwera
            server?.Stop();
            Console.WriteLine("Serwer zatrzymany.");
        }
    }
}
