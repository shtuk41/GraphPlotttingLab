using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace GraphPlottingLab
{
    public class Graphic
    {
        #region fields

        private int datasize;
        private double[] data;
        private Line []  lines;
        private Rectangle[] points;
        private Line[] axis = new Line[2];
        private Line[] ticksX = new Line[30];
        private Line[] ticksY = new Line[30];
        private TextBlock[] ticksXText = new TextBlock[30];
        private TextBlock[] ticksYText = new TextBlock[30];
        private Canvas canvasReference;
        private TextBlock mouseOverValue = new TextBlock();
        

        #endregion

        public Graphic(int dataSize, Canvas canvas)
        {
            canvasReference = canvas;
            datasize = dataSize;
            data = new double[dataSize];
            lines = new Line[dataSize - 1];
            points = new Rectangle[dataSize];

            axis[0] = new Line();
            axis[0].Stroke = System.Windows.Media.Brushes.DarkRed;
            axis[0].HorizontalAlignment = HorizontalAlignment.Left;
            axis[0].VerticalAlignment = VerticalAlignment.Center;
            axis[0].StrokeThickness = 1;

            canvas.Children.Add(axis[0]);

            axis[1] = new Line();
            axis[1].Stroke = System.Windows.Media.Brushes.DarkRed;
            axis[1].HorizontalAlignment = HorizontalAlignment.Left;
            axis[1].VerticalAlignment = VerticalAlignment.Center;
            axis[1].StrokeThickness = 1;

            canvas.Children.Add(axis[1]);


            for (int ii = 0; ii < 30; ii++)
            {
                ticksX[ii] = new Line();

                ticksX[ii].Stroke = System.Windows.Media.Brushes.White;
                ticksX[ii].HorizontalAlignment = HorizontalAlignment.Left;
                ticksX[ii].VerticalAlignment = VerticalAlignment.Center;
                ticksX[ii].StrokeThickness = 1;
                canvas.Children.Add(ticksX[ii]);

                ticksY[ii] = new Line();

                ticksY[ii].Stroke = System.Windows.Media.Brushes.White;
                ticksY[ii].HorizontalAlignment = HorizontalAlignment.Left;
                ticksY[ii].VerticalAlignment = VerticalAlignment.Center;
                ticksY[ii].StrokeThickness = 1;
                //ticksY[ii].StrokeDashArray = new DoubleCollection(new double[2] {4,4});
                canvas.Children.Add(ticksY[ii]);

                ticksXText[ii] = new TextBlock();
                ticksXText[ii].Text = string.Empty;
                ticksXText[ii].Foreground = System.Windows.Media.Brushes.YellowGreen;
                ticksXText[ii].FontSize = 8;
                canvas.Children.Add(ticksXText[ii]);

                ticksYText[ii] = new TextBlock();
                ticksYText[ii].Text = string.Empty;
                ticksYText[ii].Foreground = System.Windows.Media.Brushes.YellowGreen;
                ticksYText[ii].FontSize = 8;
                canvas.Children.Add(ticksYText[ii]);
            }

            for (int ii = 0; ii < dataSize - 1; ii++)
            {
                lines[ii] = new Line();
                lines[ii].Stroke = System.Windows.Media.Brushes.YellowGreen;
                lines[ii].HorizontalAlignment = HorizontalAlignment.Left;
                lines[ii].VerticalAlignment = VerticalAlignment.Center;
                lines[ii].StrokeThickness = 3;
                //canvas.Children.Add(lines[ii]);
            }

            for (int ii = 0; ii < dataSize; ii++)
            {
                points[ii] = new Rectangle();
                points[ii].Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                points[ii].StrokeThickness = 2;
                SolidColorBrush rectangleBrush = new SolidColorBrush();
                rectangleBrush.Color = System.Windows.Media.Brushes.LightSteelBlue.Color;
                points[ii].Fill = rectangleBrush;

                canvas.Children.Add(points[ii]);
            }

            mouseOverValue.Text = string.Empty;
            mouseOverValue.Foreground = System.Windows.Media.Brushes.YellowGreen;
            mouseOverValue.Background = System.Windows.Media.Brushes.Black;
            canvas.Children.Add(mouseOverValue);

        }

        public void Update(Point[] pts, System.Windows.Point mousePosition)
        {
            double ymin = double.MaxValue;
            double ymax = double.MinValue;
            double xmin = double.MaxValue;
            double xmax = double.MinValue;

            for (int ii = 0; ii < datasize; ii++)
            {
                if (pts[ii].X < xmin)
                    xmin = pts[ii].X;
                if (pts[ii].X > xmax)
                    xmax = pts[ii].X;
                if (pts[ii].Y < ymin)
                    ymin = pts[ii].Y;
                if (pts[ii].Y > ymax)
                    ymax = pts[ii].Y;
            }

            xmin = xmin - 0.1 * (xmax - xmin);
            xmax = xmax + 0.1 * (xmax - xmin);
            double xtotal = xmax - xmin;
            double xres = canvasReference.ActualWidth / xtotal;
          
            ymin = ymin - 0.1 * (ymax - ymin);
            ymax = ymax + 0.1 * (ymax - ymin);
            double ytotal = ymax - ymin;
            double yres = canvasReference.ActualHeight / ytotal;

            double xX1 = 0;
            double xX2 = canvasReference.ActualWidth;
            double xY1 = canvasReference.ActualHeight - (0 - ymin) * yres;
            double xY2 = canvasReference.ActualHeight - (0 - ymin) * yres;

            double yX1 = (0 - xmin) * xres;
            double yX2 = (0 - xmin) * xres;
            double yY1 = 0;
            double yY2 = canvasReference.ActualHeight;

            axis[0].X1 = xX1;
            axis[0].X2 = xX2;
            axis[0].Y1 = axisLimit(xY1,0,canvasReference.ActualHeight);
            axis[0].Y2 = axisLimit(xY2,0,canvasReference.ActualHeight);

            axis[1].X1 = axisLimit(yX1,0,canvasReference.ActualWidth);
            axis[1].X2 = axisLimit(yX2,0,canvasReference.ActualWidth);
            axis[1].Y1 = yY1;
            axis[1].Y2 = yY2;

            //ticks

            for (int ii = 0; ii < 30; ii++)
            {
                ticksX[ii].X1 = axis[1].X1 + 50 * (ii - 15);
                ticksX[ii].X2 = axis[1].X1 + 50 * (ii - 15);
                ticksX[ii].Y1 = axis[0].Y1 - 5;
                ticksX[ii].Y2 = axis[0].Y1 + 5;

                ticksY[ii].X1 = axis[1].X1 - 5;
                ticksY[ii].X2 = axis[1].X1 + 5;
                ticksY[ii].Y1 = axis[0].Y1 + 50 * (ii - 15);
                ticksY[ii].Y2 = axis[0].Y1 + 50 * (ii - 15);

                double tickValueX = (ticksX[ii].X1 - yX1) / xres;
                double tickValueY = -(ticksY[ii].Y1 - xY1) / yres;

                ticksXText[ii].Text = ii == 15 ? "0" : string.Format("{0:0.00}", tickValueX);
                Canvas.SetLeft(ticksXText[ii], ticksX[ii].X1 - 5);
                Canvas.SetBottom(ticksXText[ii], canvasReference.ActualHeight - ticksX[ii].Y1);

                ticksYText[ii].Text = ii == 15 ? string.Empty : string.Format("{0:0.00}", tickValueY);
                Canvas.SetRight(ticksYText[ii], canvasReference.ActualWidth - ticksY[ii].X1);
                Canvas.SetBottom(ticksYText[ii], canvasReference.ActualHeight - ticksY[ii].Y1);

            }


           for (int ii = 0; ii < datasize; ii++)
           {
               /*if (ii < datasize - 2)
               {
                   lines[ii].X1 = yX1 + pts[ii].X * xres;
                   lines[ii].X2 = yX1 + pts[ii + 1].X * xres;
                   lines[ii].Y1 = xY1 - pts[ii].Y * yres;
                   lines[ii].Y2 = xY1 - pts[ii + 1].Y * yres;
               }*/

               double pointX = yX1 + pts[ii].X * xres;
               double pointY = xY1 - pts[ii].Y * yres;

               Canvas.SetLeft(points[ii], pointX);
               Canvas.SetTop(points[ii], pointY);
           }

            double mouseX = mousePosition.X;
            double mouseY = mousePosition.Y;

            double graphXMouse = (mouseX - yX1) / xres;
            double graphYMouse = -(mouseY - xY1) / yres;

            mouseOverValue.Text = string.Format("x: {0:0.00}, y: {1:0.00}", graphXMouse, graphYMouse);

            Canvas.SetLeft(mouseOverValue, mousePosition.X);
            Canvas.SetBottom(mouseOverValue, canvasReference.ActualHeight - mousePosition.Y);
        }

        private double axisLimit(double val, double low, double up)
        {
            if (val > up)
                val = up;
            else if (val < low)
                val = low;

            return val;
        }
    }
}
