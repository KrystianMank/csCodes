using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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

namespace richTextBoxy
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tekst = textBoxAdd.Text;
            if (!string.IsNullOrEmpty(tekst))
            {
                Run run = new Run(tekst);
                if(chk1.IsChecked == true)
                {
                    run.FontWeight = FontWeights.Bold;
                }
                if (chk2.IsChecked == true)
                {
                    run.FontStyle = FontStyles.Italic;
                }
                if (chk3.IsChecked == true)
                {
                    run.TextDecorations = TextDecorations.Underline;
                }

                Paragraph akapit = new Paragraph();
                akapit.Inlines.Add(run);


                richTextBox1.Document.Blocks.Add(akapit);
                textBoxAdd.Clear();

            }
            else
            {
                MessageBox.Show("Nie podano tekstu","Błąd tekstu",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Pliki RTF (*.rtf)|*.rtf|Wszystkie pliki (*.*)|*.*",
            };
            if (saveFileDialog.ShowDialog() == true)
            {

                using (System.IO.FileStream fileStream = new System.IO.FileStream(saveFileDialog.FileName, System.IO.FileMode.OpenOrCreate))
                {
                    TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);

                    textRange.Save(fileStream, DataFormats.Rtf);
                }
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki RTF (*.rtf)|*.rtf|Wszystkie pliki (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {

                using (System.IO.FileStream fileStream = new System.IO.FileStream(openFileDialog.FileName, System.IO.FileMode.OpenOrCreate))
                {
                    TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);

                    textRange.Load(fileStream, DataFormats.Rtf);
                }
            }
        }

        private void chk1_Checked(object sender, RoutedEventArgs e)
        {
            TextSelection textSelection = richTextBox1.Selection;
            if (!textSelection.IsEmpty)
            {
                textSelection.ApplyPropertyValue(TextElement.FontWeightProperty,FontWeights.Bold);
            }
        }
        private void chk1_Unchecked(object sender, RoutedEventArgs e)
        {
            TextSelection textSelection = richTextBox1.Selection;
            if (!textSelection.IsEmpty)
            {
                textSelection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void chk2_Checked(object sender, RoutedEventArgs e)
        {
            TextSelection textSelection = richTextBox1.Selection;
            if (!textSelection.IsEmpty)
            {
                textSelection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
        }
        private void chk2_Unchecked(object sender, RoutedEventArgs e)
        {
            TextSelection textSelection = richTextBox1.Selection;
            if (!textSelection.IsEmpty)
            {
                textSelection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void chk3_Checked(object sender, RoutedEventArgs e)
        {
            TextSelection textSelection = richTextBox1.Selection;
            if (!textSelection.IsEmpty)
            {
                textSelection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
        }
        private void chk3_Unchecked(object sender, RoutedEventArgs e)
        {
            TextSelection textSelection = richTextBox1.Selection;
            if (!textSelection.IsEmpty)
            {
                textSelection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
        }

        private void comboBoxFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                double content = Double.Parse(selectedItem.Content.ToString());
                TextSelection textSelection = richTextBox1.Selection;
                if (!textSelection.IsEmpty)
                {
                    textSelection.ApplyPropertyValue(TextElement.FontSizeProperty, content);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string content = selectedItem.Content.ToString();
                TextSelection textSelection = richTextBox1.Selection;
                if (!textSelection.IsEmpty)
                {
                    textSelection.ApplyPropertyValue(TextElement.FontFamilyProperty, content);
                }
            }
        }
    }
}
