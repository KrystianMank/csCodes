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

namespace FinanceManager
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

        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MainPageTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainPageContent.Content = new MainPage();
        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedTab = (TabItem)Tabs.SelectedItem;
            if (selectedTab != null)
            {
                string tabname = selectedTab.Header.ToString();
                if(tabname == "Strona główna")
                {
                    MainPageContent.Content = new MainPage().Content;   
                }
                else if(tabname == "Wizualizacja danych")
                {
                    DataVisualisationContent.Content = new DataVisualisation().Content;
                }
                else if(tabname == "Eksport/Import danych")
                {
                    SharingDataContent.Content = new SharingData().Content;
                }
            }
        }
    }
}
