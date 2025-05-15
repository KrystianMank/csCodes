using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;

namespace ToDoList
{
    public class Zadanie : INotifyPropertyChanged
    {
        private string nazwa;
        private string opis;
        private bool czyWykonano;
        private string statusZadania;

        public string Nazwa
        {
            get => nazwa;
            set
            {
                nazwa = value;
                OnPropertyChanged(nameof(Nazwa));
            }
        }

        public string Opis
        {
            get => opis;
            set
            {
                opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        public bool CzyWykonano
        {
            get => czyWykonano;
            set
            {
                czyWykonano = value;
                OnPropertyChanged(nameof(czyWykonano));
                // Aktualizujemy StatusZadania przy zmianie czyWykonano
                StatusZadania = czyWykonano ? "tak" : "nie";
            }
        }

        public string StatusZadania
        {
            get => statusZadania;
            set
            {
                statusZadania = value;
                OnPropertyChanged(nameof(StatusZadania));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        private ObservableCollection<Zadanie> zadaniaKolekcja;
        private string path;
        public MainWindow()
        {
            InitializeComponent();
            zadaniaKolekcja = new ObservableCollection<Zadanie>()
            {
                new Zadanie {Nazwa = "Przykład 1", Opis="opis", CzyWykonano = false, StatusZadania="nie"},
            };
            listaZadanListView.ItemsSource = zadaniaKolekcja;

            
        }

        private void dodajButton_Click(object sender, RoutedEventArgs e)
        {
            string nazwaZadania = nazwaTextBox.Text;
            string opisZadania = opisTextBox.Text;
            if (!string.IsNullOrWhiteSpace(nazwaZadania))
            {
                zadaniaKolekcja.Add(new Zadanie { Nazwa = nazwaZadania, Opis = opisZadania, CzyWykonano = false, StatusZadania = "nie" });
            }
            else
            {
                MessageBox.Show("Nazwa zadania jest obowiązkowa!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GridViewColumn_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("");
        }

        private void listaZadanListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var wybranyElement = listaZadanListView.SelectedItem as Zadanie;
            
            if (wybranyElement != null)
            {
                wybranyElement.CzyWykonano = !wybranyElement.CzyWykonano;
            }
            if (wybranyElement.CzyWykonano == true)
            {
                wybranyElement.StatusZadania = "tak";
            }
            else
            {
                wybranyElement.StatusZadania = "nie";
            }

        }

        private void listaZadanListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void zapiszButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int i = 0; i < zadaniaKolekcja.Count; i++)
                    {
                        sw.WriteLine(i + ". " + zadaniaKolekcja.ElementAt(i).Nazwa + " " + zadaniaKolekcja.ElementAt(i).Opis + ". Status zadania: " + zadaniaKolekcja.ElementAt(i).StatusZadania );
                    }
                }
            }
        }
    }
}
