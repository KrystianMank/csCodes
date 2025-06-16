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
using FinanceManager.ViewModels;
using Npgsql;

namespace FinanceManager
{
    /// <summary>
    /// Logika interakcji dla klasy TranactionPanel.xaml
    /// </summary>
    public partial class TranactionPanel : UserControl
    {
        public TranactionPanel()
        {
            InitializeComponent();
            DataContext = TransactionViewModel.Instance; // Ustawienie DataContext na instancję TransactionViewModel
        }

        
    }
}
