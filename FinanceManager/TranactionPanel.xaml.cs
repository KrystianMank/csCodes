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
using TransactionModel;

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
            tranasctionTypeComboBox.SelectionChanged += tranasctionTypeComboBox_SelectionChanged;
        }

        private void tranasctionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void transactionSourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addTranasctionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void formResetButton_Click(object sender, RoutedEventArgs e)
        {
            titleTextBox.Text = string.Empty;
            amountTextBox.Text = string.Empty;
            dateDatePicker.Text = string.Empty;
            descriptionTextBox.Text = string.Empty;
            todaysDateCheckBox.IsChecked = false;
            tranasctionTypeComboBox.SelectedIndex = 0;
            transactionSourceComboBox.SelectedIndex = 0;
        }
    }
}
