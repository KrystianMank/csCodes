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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace WczytanieZdjec
{
    public class Zdjecie
    {
        public string pathImage { get; set; } 
    }
  
    public partial class MainWindow : Window
    {
        ObservableCollection<Zdjecie> images;
        public MainWindow()
        {
            InitializeComponent();
            images = new ObservableCollection<Zdjecie>();
            listaZdjecListView.ItemsSource = images;
            DataContext = this;
        }

        private void pobierzButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "jpg|*.jpg|png|*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                DuzyImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                images.Add(new Zdjecie { pathImage = openFileDialog.FileName });

            }
        }

        private void listaZdjecListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var wybranyElement = listaZdjecListView.SelectedItem as Zdjecie;
            if (wybranyElement != null)
            {
                DuzyImage.Source = new BitmapImage(new Uri(wybranyElement.pathImage));
            }
        }
    }
}
