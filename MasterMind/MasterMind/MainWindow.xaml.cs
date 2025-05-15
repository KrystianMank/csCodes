using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterMind
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Tablica dostępnych kolorów pinów
        private readonly Color[] pinColors =
        {
            Colors.Red,Colors.Green, Colors.Blue, Colors.Orange, Colors.Violet,Colors.Yellow
        };

        private Color[] secretPins; // Sekretny kod do zgadnięcia
        private List<Ellipse> circlesGuess = new List<Ellipse>(); // Lista zaznaczonych kółek
        private static int index = -1; // Indeks aktualnego kółka
        private int rowPanelIndex = 0; // Indeks aktualnego wiersza

        public MainWindow()
        {
            InitializeComponent();
        }

        // Generowanie głównej planszy gry
        private void GenerateMainBoard()
        {
            for (int i = 0; i < 10; i++) // 10 wierszy (prób)
            {
                // Panel pojedynczego wiersza
                StackPanel rowPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(5)
                };
                rowPanel.Tag = i;
                rowPanelIndex = i;

                // Panel głównych kółek (zgadywanych kolorów)
                StackPanel mainCirclesPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                };

                // Tworzenie 4 kółek w panelu
                for (int j = 0; j < 4; j++)
                {
                    mainCirclesPanel.Children.Add(CreateCircle(30, i == 9, true)); // Tylko ostatni wiersz początkowo aktywny
                }

                // Panel feedbacku (czarne/białe kropki)
                Grid feedback = new Grid
                {
                    Width = 60,
                    Height = 60,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                feedback.RowDefinitions.Add(new RowDefinition());
                feedback.RowDefinitions.Add(new RowDefinition());
                feedback.ColumnDefinitions.Add(new ColumnDefinition());
                feedback.ColumnDefinitions.Add(new ColumnDefinition());

                // Tworzenie 4 kółek feedbacku (2x2)
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        Ellipse ellipse = CreateCircle(20, false, false);
                        Grid.SetRow(ellipse, x);
                        Grid.SetColumn(ellipse, y);
                        feedback.Children.Add(ellipse);
                    }
                }

                rowPanel.Children.Add(mainCirclesPanel);
                rowPanel.Children.Add(feedback);
                MainPanel.Children.Add(rowPanel);
            }
        }

        // Tworzenie pojedynczego kółka
        private Ellipse CreateCircle(int size, bool isEnabled, bool isMainCircle)
        {
            Ellipse circle = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Margin = new Thickness(5),
                IsEnabled = isEnabled,
            };

            // Przypisanie zdarzeń w zależności od typu kółka
            if (isMainCircle)
            {
                // Kółko główne - wybór koloru
                circle.MouseRightButtonDown += (sender, e) =>
                {
                    OpenColorPicker(sender as Ellipse, pinColors);
                    circlesGuess.Add(circle);
                    index++;
                };
            }
            else
            {
                // Kółko feedbacku - tylko czarne/białe
                circle.MouseRightButtonDown += (sender, e) =>
                {
                    OpenColorPicker(sender as Ellipse, new Color[] { Colors.Black, Colors.White });
                };
            }

            return circle;
        }

        // Otwieranie menu wyboru koloru
        private void OpenColorPicker(Ellipse circle, Color[] pinColors)
        {
            ContextMenu colorMenu = new ContextMenu();
            foreach (Color color in pinColors)
            {
                MenuItem item = new MenuItem
                {
                    Header = GetColorName(color), // Nazwa koloru zamiast wartości HEX
                    Background = new SolidColorBrush(color)
                };

                item.Click += (sender, e) =>
                {
                    circle.Fill = new SolidColorBrush(color);
                    circlesGuess[index].Fill = circle.Fill;
                };
                colorMenu.Items.Add(item);
            }
            circle.ContextMenu = colorMenu;
            colorMenu.IsOpen = true;
        }

        // Konwersja koloru na czytelną nazwę
        string GetColorName(Color color)
        {
            string colorName = color.ToString().Replace("#FF", "");
            switch (colorName)
            {
                case "FF0000": return "Red";
                case "008000": return "Green";
                case "0000FF": return "Blue";
                case "FFA500": return "Orange";
                case "EE82EE": return "Violet";
                case "FFFF00": return "Yellow";
            }
            return colorName;
        }

        // Generowanie sekretnego kodu bez powtórzeń
        private void generateSecretPins()
        {
            Random random = new Random();
            secretPins = pinColors.OrderBy(c => random.Next()).Take(4).ToArray();
        }

        // Generowanie sekretnego kodu z powtórzeniami
        private void generateSecretPinsPowt()
        {
            Random random = new Random();
            secretPins = Enumerable.Range(0, 4).Select(_ => pinColors[random.Next(pinColors.Length)]).ToArray();
        }

        // Zamykanie okna
        protected override void OnClosing(CancelEventArgs e)
        {
            var odp = MessageBox.Show("Czy na pewno chcesz skończyć grę?", "Zamknij", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (odp != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void zakonczMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Rozpoczęcie nowej gry
        private void nowaGraMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var odp = MessageBox.Show("Czy wylosować kolory pinów z powtórzeniami?", "Jaka gra", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (odp == MessageBoxResult.Yes)
            {
                generateSecretPinsPowt();
            }
            else
            {
                generateSecretPins();
            }
            MainPanel.Children.Clear();
            GenerateMainBoard();
        }

        // Sprawdzanie kombinacji
        private void btnSprawdz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int currentRow = rowPanelIndex;

                StackPanel currentPanel = MainPanel.Children[currentRow] as StackPanel;
                currentPanel.Background = new SolidColorBrush(Colors.LightGray);

                if (currentPanel == null) return;

                StackPanel mainCirclePins = currentPanel.Children[0] as StackPanel;

                if (mainCirclePins == null) return;

                Color[] guess = new Color[4]; // Zgadywane kolory
                bool?[] wyniki = new bool?[4]; // Wyniki (true = dobra pozycja, false = dobry kolor)
                bool[] uzyte = new bool[4]; // Flagi użytych pinów

                // Pobieranie kolorów z kółek
                for (int i = 0; i < 4; i++)
                {
                    if (mainCirclePins.Children[i] is Ellipse circle && circle.Fill is SolidColorBrush brush)
                    {
                        guess[i] = brush.Color;
                        
                        if (brush.Color == Colors.Gray)
                        {
                            MessageBox.Show("Zaznacz wszystkie piny!");
                            return;
                        }
                        circle.IsEnabled = false; // Dezaktywacja sprawdzonego kółka
                    }
                    

                }



                // Sprawdzanie poprawnych pozycji
                for (int i = 0; i < secretPins.Length; i++)
                {
                    if (secretPins[i] == guess[i])
                    {
                        wyniki[i] = true;
                        uzyte[i] = true;
                    }
                }

                // Sprawdzanie poprawnych kolorów (ale złej pozycji)
                for (int i = 0; i < guess.Length; i++)
                {
                    if (wyniki[i] != true)
                    {
                        for (int j = 0; j < secretPins.Length; j++)
                        {
                            if (!uzyte[j] && guess[i] == secretPins[j])
                            {
                                wyniki[i] = false;
                                uzyte[j] = true;
                                break;
                            }
                        }
                    }
                }

                Grid feedback = currentPanel.Children[1] as Grid;
                if (feedback == null) return;

                List<Ellipse> feedbackCircles = new List<Ellipse>();

                // Pobieranie kółek feedbacku
                for (int row = 0; row < 2; row++)
                {
                    for (int col = 0; col < 2; col++)
                    {
                        var ellipse = feedback.Children
                            .OfType<Ellipse>()
                            .FirstOrDefault(item => Grid.GetRow(item) == row && Grid.GetColumn(item) == col);

                        if (ellipse != null)
                        {
                            feedbackCircles.Add(ellipse);
                        }
                    }
                }

                int blackPins = 0; // Liczba czarnych pinów (dobra pozycja)
                int whitePins = 0; // Liczba białych pinów (dobry kolor)

                // Liczenie wyników
                for (int i = 0; i < wyniki.Length; i++)
                {
                    if (wyniki[i] == true) blackPins++;
                    else if (wyniki[i] == false) whitePins++;
                }

                // Wypełnianie feedbacku (najpierw czarne, potem białe)
                int feedbackIndex = 0;

                // Czarne piny (dobre pozycje)
                for (int i = 0; i < blackPins; i++)
                {
                    if (feedbackIndex < feedbackCircles.Count)
                    {
                        feedbackCircles[feedbackIndex].Fill = new SolidColorBrush(Colors.Black);
                        feedbackIndex++;
                    }
                }

                // Białe piny (dobre kolory)
                for (int i = 0; i < whitePins; i++)
                {
                    if (feedbackIndex < feedbackCircles.Count)
                    {
                        feedbackCircles[feedbackIndex].Fill = new SolidColorBrush(Colors.White);
                        feedbackIndex++;
                    }
                }

                // Czyszczenie pozostałych
                while (feedbackIndex < feedbackCircles.Count)
                {
                    feedbackCircles[feedbackIndex].Fill = Brushes.Gray;
                    feedbackIndex++;
                }

                rowPanelIndex--;

                // Aktywacja następnego wiersza
                if (rowPanelIndex >= 0)
                {
                    StackPanel nextPanel = MainPanel.Children[rowPanelIndex] as StackPanel;
                    if (nextPanel != null)
                    {
                        nextPanel.Background = new SolidColorBrush(Colors.LightGray);
                        StackPanel nextMainCirclesPanel = nextPanel.Children[0] as StackPanel;
                        if (nextMainCirclesPanel != null)
                        {
                            foreach (Ellipse circle in nextMainCirclesPanel.Children)
                            {
                                circle.IsEnabled = true;
                            }
                        }
                    }
                }

                List<string> colorNames = new List<string>();
                foreach (Color color in secretPins)
                {
                    colorNames.Add(GetColorName(color));
                }
                //MessageBox.Show($"{string.Join(", ", colorNames)}");

                // Sprawdzanie wygranej/przegranej
                if (blackPins == 4)
                {
                    MessageBox.Show("YOU WON");
                }
                else if (rowPanelIndex == -1)
                {
                    MessageBox.Show($"YOU LOST\nCorrect Colors:{string.Join(", ", colorNames)}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd");
            }
        }

        private void oProgramieMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("v.0.3\nGra MasterMind\nAutor:Krystian Mankiewicz", "O programie",MessageBoxButton.OK ,MessageBoxImage.Information);
        }

        private void zasadyGryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Gra polega na odgadnięciu 4 ukrytych kul.\nGra zostanie rozwiązana, jeśli w ciągu 9 tur gracz odgadnie te kule.\nW każdej turze gracz wybiera 4 kule, po czym sprawdza czy trafił.\nKażda prawidłowo odgadnięta kula (kula o właściwym kolorze na właściwym miejscu) sygnalizowana jest czarną kropką.\nJeśli gracz odgadł kolor kuli, nie odgadł zaś jej lokalizacji, jest to sygnalizowane białą kropką.\nGracz nie wie, które kule są właściwe, które zaś nie.",
                "Zasady gry", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}