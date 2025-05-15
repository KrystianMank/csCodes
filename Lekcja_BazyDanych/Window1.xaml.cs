using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lekcja_BazyDanych
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=szkola";

        public Window1()
        {
            InitializeComponent();
        }

        private void btnUsunUcznia_Click(object sender, RoutedEventArgs e)
        {
        
            try
            {
                int uczen = int.Parse(NumerTextBox.Text);
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("DELETE FROM uczen WHERE uczen_id=@uczen", conn))
                    {
                        cmd.Parameters.AddWithValue("uczen", uczen);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Pomyślnie usunięto ucznia!");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}");
                this.DialogResult = false;
            }
        }
    }
}
