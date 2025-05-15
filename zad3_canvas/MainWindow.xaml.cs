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

namespace zad3_canvas
{
    public class KolorItem
    {
        public string Name { get; set; }
    }

    public partial class MainWindow : Window
    {
        public List<KolorItem> Kolory { get; set; }
        string wybranyKolor;
        public MainWindow()
        {
            InitializeComponent();
            Kolory = new List<KolorItem>
            {
                new KolorItem {Name="Black"},
                new KolorItem {Name="White"},
                new KolorItem {Name="Green"},
                new KolorItem {Name="Red"},
                new KolorItem {Name="Blue"},

            };
            DataContext = this;
            comboBoxKolory.SelectedIndex = 0;
            wybranyKolor = Kolory[0].Name;

        }

        private void myCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Color color = (Color)ColorConverter.ConvertFromString(wybranyKolor);
            SolidColorBrush brush = new SolidColorBrush(color);

            Point clickPos = e.GetPosition(myCanvas);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Rectangle rect = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Fill = brush,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(rect, clickPos.X - 10);
                Canvas.SetTop(rect, clickPos.Y - 10);

                myCanvas.Children.Add(rect);
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = brush,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(ellipse, clickPos.X - 10);
                Canvas.SetTop(ellipse, clickPos.Y - 10);

                myCanvas.Children.Add(ellipse);
            }


        }

        private void comboBoxKolory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KolorItem item = (KolorItem)comboBoxKolory.SelectedItem;
            if (item != null)
            {
                wybranyKolor = item.Name;
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }
    }
}
