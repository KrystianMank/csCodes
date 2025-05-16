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
    /// Logika interakcji dla klasy MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public TranactionPanel tranactionPanel;
        public TransactionHistory transactionHistoryPanel;
        public List<UserControl> panels;
        private int panelsIndex = 0;

        public MainPage()
        {
            InitializeComponent();
            tranactionPanel = new TranactionPanel();
            transactionHistoryPanel = new TransactionHistory();
            panels = new List<UserControl>
            {
                tranactionPanel, transactionHistoryPanel
            };
            PanelMainPage.Content = panels[0];
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            panelsIndex--;
            if (panelsIndex < 0) panelsIndex = panels.Count - 1;
            PanelMainPage.Content = panels[panelsIndex];
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            panelsIndex++;
            if (panelsIndex >= panels.Count) panelsIndex = 0;
            PanelMainPage.Content = panels[panelsIndex];
        }
    }
}
