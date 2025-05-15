using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Lekcja_BazyDanych
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=szkola";
        public MainWindow()
        {
            InitializeComponent();
            ZaladujUczniow();
        }

        // Metoda dodawania ucznia do bazy danych
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string imie = ImieTextBox.Text;
            string nazwisko = NazwiskoTextBox.Text;
            string plec = (PlecComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime? dataUr = DataUrDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(imie) || string.IsNullOrWhiteSpace(nazwisko) || string.IsNullOrWhiteSpace(plec) || dataUr == null)
            {
                MessageBox.Show("Wszystkie pola wymagane!");
                return;
            }
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand("INSERT INTO uczen (imie, nazwisko, plec, data_ur) VALUES (@imie, @nazwisko, @plec, @data_ur)", connection))
                    {
                        cmd.Parameters.AddWithValue("imie", imie);
                        cmd.Parameters.AddWithValue("nazwisko", nazwisko);
                        cmd.Parameters.AddWithValue("plec", plec[0]);
                        cmd.Parameters.AddWithValue("data_ur", dataUr.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Dodano ucznia pomyślnie!");
                WyczyscFormularz();
                ZaladujUczniow(); //Po dodaniu ucznia załaduj zaaktualizowane dane
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        //Metoda do pobierania uczniow z bazy danych
        private void ZaladujUczniow()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM uczen", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Tworzymy DataTable, aby przechowywać wyniki
                            DataTable uczniowieTable = new DataTable();
                            uczniowieTable.Load(reader);
                            UczniowieDataGrid.ItemsSource = uczniowieTable.DefaultView; //Powiązanie z DataGrid
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania uczniów: {ex.Message}");
            }
        }

        //Metoda do czyszczenia formularza
        private void WyczyscFormularz()
        {
            ImieTextBox.Clear();
            NazwiskoTextBox.Clear();
            PlecComboBox.SelectedIndex = -1;
            DataUrDatePicker.SelectedDate = null;
        }

        private void btnUsunUcznia_Click(object sender, RoutedEventArgs e)
        {
           Window1 dialog = new Window1();
             
            if (dialog.ShowDialog() == true)
            {
                ZaladujUczniow();
                
            }
        }

        
    }
}
