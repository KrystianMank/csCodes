using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using System.ComponentModel;

namespace StoliceUTC
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<StoliceClass> Stolice { get; set; }
        public ObservableCollection<StoliceClass> SelectedStolice { get; set; }

        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            Stolice = new ObservableCollection<StoliceClass>();
            SelectedStolice = new ObservableCollection<StoliceClass>();

            LoadDataFromCsv();
            InitializeTimer();

            DataContext = this;
        }

        private void LoadDataFromCsv()
        {
            string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "czasy.csv");

            if (File.Exists(csvPath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(csvPath);

                    for (int i = 1; i < lines.Length; i++) // Skip the header row
                    {
                        string[] parts = lines[i].Split(',');
                        if (parts.Length == 2)
                        {
                            string name = parts[0].Trim();
                            string utcOffsetStr = parts[1].Trim();

                            if (TimeSpan.TryParseExact(utcOffsetStr, @"\+h\:mm", null, out TimeSpan utcOffset) ||
                                TimeSpan.TryParseExact(utcOffsetStr, @"\-h\:mm", null, out utcOffset))
                            {
                                try
                                {
                                    TimeZoneInfo timeZone = TimeZoneInfo.CreateCustomTimeZone(name, utcOffset, name, name);
                                    Stolice.Add(new StoliceClass { Name = name, TimeZone = timeZone });
                                }
                                catch (Exception tzEx)
                                {
                                    MessageBox.Show($"Błąd tworzenia strefy czasowej: {tzEx.Message}");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Nieprawidłowy format UTC offset: {utcOffsetStr}");
                            }
                        }
                    }

                    // Bind ComboBox to Stolice collection
                    ComboBoxStolice.ItemsSource = Stolice;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd: {ex.Message}");
                }
            }
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // Update every second
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the current time for each selected city
            foreach (var stolica in SelectedStolice)
            {
                stolica.CurrentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, stolica.TimeZone).ToString("HH:mm:ss");
            }
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            var selectedStolica = ComboBoxStolice.SelectedItem as StoliceClass;
            if (selectedStolica != null && !SelectedStolice.Contains(selectedStolica))
            {
                // Create a new instance to avoid binding issues
                var newStolica = new StoliceClass
                {
                    Name = selectedStolica.Name,
                    TimeZone = selectedStolica.TimeZone,
                    CurrentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, selectedStolica.TimeZone).ToString("HH:mm:ss")
                };

                SelectedStolice.Add(newStolica);
            }
        }
    }

    public class StoliceClass : INotifyPropertyChanged
    {
        private string _name;
        private string _currentTime;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public TimeZoneInfo TimeZone { get; set; }

        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}