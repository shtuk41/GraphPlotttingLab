using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;


namespace GraphPlottingLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Graphic graphic;

        public MainWindow()
        {
            InitializeComponent();

            graphic = new Graphic(1024, animationCanvas);
        }

        double aplitudeLast = 10;
        double frequencyLast = 1;

        private void DrawSineWave(object sender, EventArgs e)
        {
            //animationCanvas.Children.Clear();

            Point[] spline = new Point[1024];

            double amplitude = aplitudeLast;
            double f = frequencyLast;

            for (int ii = 0; ii < 1024; ii++)
            {
                spline[ii].X = ii;
                spline[ii].Y = amplitude * Math.Sin(2.0 * Math.PI * f * ii / 1024.0) + 40;
            }

            graphic.Update(spline, Mouse.GetPosition(animationCanvas));

            aplitudeLast += 1.0;
            frequencyLast += 0.1;

            if (aplitudeLast > 100)
            { 
                aplitudeLast = 10;
                frequencyLast = 1.0;
            }
        }


        private void image_MouseMoveHandler(object sender, MouseEventArgs e)
        {

        }

        private void image_MouseLeaveHandler(object sender, MouseEventArgs e)
        {

        }

        private void image_MouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            //aplitudeLast = 10;
            //frequencyLast = 1;

            CompositionTarget.Rendering += DrawSineWave;
        }

        private void image_MouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            CompositionTarget.Rendering -= DrawSineWave;
        }
    }
}
