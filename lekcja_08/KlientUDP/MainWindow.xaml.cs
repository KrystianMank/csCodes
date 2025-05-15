using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KlientUDP
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<string> SendMessageAsync(string message)
        {
            UdpClient client = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes(message);

            //Wysyłanie wiadomości do serwera
            await client.SendAsync(data, data.Length, "127.0.0.1", 9001);

            //Odbieranie odpowiedzi od serwera
            UdpReceiveResult receivedResult = await client.ReceiveAsync();
            string responseMessage = Encoding.UTF8.GetString(receivedResult.Buffer);

            return responseMessage;
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Wprowadź wiadomość do wysłania");
                return;
            }
            try
            {
                string response = await SendMessageAsync(message);
                ResponseTextBlock.Text = $"Odpowiedź z serwera: {response}";
            }
            catch (Exception ex)
            {
                ResponseTextBlock.Text = ex.Message;
            }
        }
    }
}
