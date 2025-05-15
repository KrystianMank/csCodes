using System;
using System.Net.Sockets;
using System.Text;

class TcpClientApp
{
    static void Main()
    {
        try
        {
            // Połączenie z serwerem na lokalnym hoście i porcie 8000

            TcpClient client = new TcpClient("127.0.0.1", 8000);

            // Używanie strumienia do komunikacji
            NetworkStream stream = client.GetStream();

            // Odbiór powitania od serwera
            byte[] buffer = new byte[256];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string serverMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine(serverMessage);


            // Wysłanie wiadomości do serwera
            Console.Write("Podaj wiadomość: ");
            string clientMessage = Console.ReadLine() + "";
            byte[] clientBytes = Encoding.UTF8.GetBytes(clientMessage);
            stream.Write(clientBytes, 0, clientBytes.Length);
            Console.WriteLine("Wysłano wiadomość do serwera.");

            // Odbiór odpowiedzi zwrotnej od serwera
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string responseMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Odebrano odpowiedź od serwera: " + responseMessage);

            // Zamknięcie połączenia
            client.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Wystąpił błąd: " + e.Message);
        }
    }
}
