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


namespace zad4_canvas
{
    public class Figures
    {
        public string Name { get; set; }
    }
    public partial class MainWindow : Window
    {
        public List<Figures> figures { get; set; }
        private string wybranaFigura;
        private List<Point> clickPos;
        public MainWindow()
        {
            InitializeComponent();
            figures = new List<Figures>
            {
                new Figures {Name="Linia"},
                new Figures {Name="Prostokąt"},
                new Figures {Name="Elipsa"},
                new Figures {Name="Okrąg"},
            };
            DataContext = this;
            comboBoxFigury.SelectedIndex = 0;
            wybranaFigura = figures[0].Name;
            clickPos = new List<Point>();
        }

        private void myCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clickPos.Add(e.GetPosition(myCanvas));
            if(clickPos.Count == 2)
            {
                if (wybranaFigura.Equals("Linia"))
                {
                    Line line = new Line
                    {
                        X1 = clickPos[0].X,
                        Y1 = clickPos[0].Y,
                        X2 = clickPos[1].X,
                        Y2 = clickPos[1].Y,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3
                    };
                    myCanvas.Children.Add(line);
                    clickPos.Clear();
                }
                if (wybranaFigura.Equals("Prostokąt") || wybranaFigura.Equals("Elipsa"))
                {
                    double width = Math.Abs(clickPos[1].X - clickPos[0].X);
                    double height = Math.Abs(clickPos[1].Y - clickPos[0].Y);

                    double left = Math.Min(clickPos[0].X, clickPos[1].X);
                    double top = Math.Min(clickPos[0].Y, clickPos[1].Y);

                    
                    if (wybranaFigura.Equals("Prostokąt")){
                        Rectangle rectangle = new Rectangle
                        {
                            Width = width,
                            Height = height,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                        };
                        Canvas.SetLeft(rectangle, left);
                        Canvas.SetTop(rectangle, top);
                        myCanvas.Children.Add(rectangle);
                    }
                    if (wybranaFigura.Equals("Elipsa"))
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Width = width,
                            Height = height,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2,
                        };
                        Canvas.SetLeft(ellipse, left);
                        Canvas.SetTop(ellipse, top);
                        myCanvas.Children.Add(ellipse);
                    }

                    clickPos.Clear();
                }
                
                if (wybranaFigura.Equals("Okrąg"))
                {
                    // średnica obliczona ze wzoru na odleglość między punktami (twierdznie Pitagorasa)
                    double srednica = Math.Sqrt(Math.Pow(clickPos[1].X - clickPos[0].X, 2) + Math.Pow(clickPos[1].Y - clickPos[0].Y, 2));

                    //środek okręgu
                    double centerX = (clickPos[0].X + clickPos[1].X) / 2;
                    double centerY = (clickPos[0].Y + clickPos[1].Y) / 2;

                    //wyśrodkowanie lewego górnego rogu okręgu
                    double left = centerX - srednica / 2;
                    double top = centerY - srednica / 2;
                    Ellipse circle = new Ellipse
                    {
                        Width = srednica,
                        Height = srednica,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                    };

                    Canvas.SetLeft(circle, left);
                    Canvas.SetTop(circle, top);
                    myCanvas.Children.Add(circle);
                    clickPos.Clear();
                }
            }
            

        }

        private void comboBoxFigury_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Figures figure = (Figures)comboBoxFigury.SelectedItem;
            if (figure != null)
            {
                wybranaFigura = figure.Name;

                clickPos.Clear();

            }
        }
    }
}
