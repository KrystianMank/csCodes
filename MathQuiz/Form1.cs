using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathQuiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Utworzenie obiektu Random do generowania liczb losowych
        Random randomizer = new Random();

        //Zmienne całkowite przechowujące liczby do problemów matematycznych:
        //Dodawania
        int addend1, addend2;

        //Odejmowania
        int minuend, subtrahend;

        //Mnożenia
        int multiplicand, multiplier;

        //Dzielenia
        int dividend, divisor;

        //Liczba całkowita do śledzenia pozostałego czasu
        int timeLeft;

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Gdy metoda CheckAnswers zwróci true, zatrzymuje czas i wyświetla końcowe okienko
            if (CheckAnswers())
            {
                timer1.Stop();
                startButton.Enabled = true;
                MessageBox.Show("Wyszystkie problemy rozwiązane poprawnie !","Brawo!");
            }
            //Gdy metoda CheckAnswers zróci false, odlicza dalej czas w dół i wyświetla go
            else if(timeLeft > 0)
            {
                timeLeft -= 1;
                timeLabel.Text = timeLeft.ToString();
            }
            //Gdy czas się skończy, zatrzyuje czas, wyświetla końcowe okienko i pokazuje wyniki
            else
            {
                timer1.Stop();
                timeLabel.Text = "Koniec czasu!";
                MessageBox.Show("Nie udało Ci ukończyć przed czasem","Niestety!");
                sum.Value = addend1 + addend2;
                difference.Value = minuend - subtrahend;
                product.Value = multiplicand * multiplier;
                quotient.Value = dividend / divisor;
                startButton.Enabled = true;
            }

            if(timeLeft <= 5)
            {
                timeLabel.BackColor = Color.Red;
                Console.Beep();
            }
        }

        private void anwer_Enter(object sender, EventArgs e)
        {
            //Zaznacz całą odpowiedź w kontrolce NumericUpDown
            NumericUpDown answerBox = sender as NumericUpDown;

            if(answerBox != null)
            {
                int lenghtOfAnswer = answerBox.Value.ToString().Length;
                answerBox.Select(0,lenghtOfAnswer);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartQuiz();
            startButton.Enabled = false;
        }

   


        //Metoda do rozpoczęcia quizu, wypełnia problemy matematyczne i rozpoczyna odliczać czas
        public void StartQuiz()
        {
            //Przypisanie do zmiennych 'addend1' i 'addend2' liczb całkowitych wygenerowanych losowwo
            addend1 = randomizer.Next(51);
            addend2 = randomizer.Next(51);

            //Wpisanie do odpowiednich etykiet przekonwetowanych zmiennych 'addend1' i 'addend2'
            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();

            //Wyzerowanie miesjsca do wpisywania sumy
            sum.Value = 0;

            //Uzupełnienie problemu odejmowania
            minuend = randomizer.Next(1, 101);
            subtrahend = randomizer.Next(1, minuend);
            minusLeftLabel.Text = minuend.ToString();
            minusRightLabel.Text = subtrahend.ToString();
            difference.Value = 0;

            //Uzupełnienie problemu mnożenia
            multiplicand = randomizer.Next(2,11);
            multiplier = randomizer.Next(2,11);
            timesLeftLabel.Text = multiplicand.ToString();
            timesRightLabel.Text = multiplier.ToString();
            product.Value = 0;

            //Uzupełnienie problemu dzielenia
            divisor = randomizer.Next(2, 11);
            int temporaryQuotient = randomizer.Next(2, 11);
            dividend = divisor * temporaryQuotient;
            divideLeftLabel.Text = dividend.ToString();
            divideRightLabel.Text = divisor.ToString();
            quotient.Value = 0;

            //Uruchomienie czasomierza
            timeLeft = 30;
            timeLabel.Text = "30";
            timer1.Start();

        }


        //Metoda do sprawdzania poprawności odpowiedzi
        private bool CheckAnswers()
        {
            if ((addend1 + addend2 == sum.Value)
                && (minuend - subtrahend == difference.Value)
                && (multiplier * multiplicand == product.Value)
                && (dividend / divisor == quotient.Value))

                return true;
            else
                return false;
        }

        private void timeLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
