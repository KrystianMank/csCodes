using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NotebookApp
{
    public partial class Form1 : Form
    {
        bool isSaved = false;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obługa menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.Z)
            {
                zamknijToolStripMenuItem_Click(sender, e);
            }
            if(e.Alt && e.KeyCode == Keys.O)
            {
                otwórzToolStripMenuItem_Click(sender, e);
            }
            if(e.Control && e.KeyCode == Keys.S)
            {
                zapiszToolStripMenuItem_Click(sender, e);
            }

        }
        

        // Kliknięcie na przycisk "Zamknij"
        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isSaved)
            {
                this.Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Nie zapisany text", "Uwaga", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if(dialogResult == DialogResult.OK)
                {
                    this.Close();
                }
                else
                {
                    richTextBox1.Text += "";
                }
            }
        }

        // Kliknięcie na przycisk "Nowy"
        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            this.Text = "Nowy plik";
            isSaved = false;
        }

        // Kliknięcie na przycisk "Otwórz"
        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Ustawienie nazwy pliku na pasku tytułowym
                this.Text = openFileDialog1.FileName;

                // Odczyt pliku
                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);

                isSaved = true;
            }
        }

        // Kliknięcie na przycisk "Zapisz"
        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Jeśli plik nie istnieje
            if (!File.Exists(this.Text))
            {
                // Otwarcie okna dialogowego
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // Zapis całości tekstu do nowego pliku
                    File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);
                    isSaved = true;
                }
            }
            else // Jeśli plik istnieje
            {
                // Zapis całości tekstu do istniejącego pliku
                File.WriteAllText(this.Text, richTextBox1.Text);
                isSaved = true;
            }
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isSaved = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            zamknijToolStripMenuItem_Click(sender, e);
        }
    }
}
