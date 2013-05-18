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

namespace GraphPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PlotGraph();
        }

        private void PlotGraph()
        {
            var p = new Polyline();
            var f = 2;
            var fs = 500;
            //Func<double,double> func = (n) => Math.Sin((n * f * 2 * Math.PI)/fs);
            Func<double, double> func = (n) => Math.Sin((n * f * 2 * Math.PI) / fs) - 0.25 * Math.Sin((n * f * 8 * Math.PI) / fs);
            var centreY = 300;
            var scaleY = 200;
            var maxPixelsX = 1800;

            var centreLine = new Line() { Stroke = Brushes.Gray, X1 = 0, X2 = maxPixelsX, Y1 = centreY, Y2 = centreY, StrokeThickness = 1 };
            canvasGraph.Children.Add(centreLine);

            for (int n = 0; n < maxPixelsX; n++)
            {
                //var y = centreY - Math.Sin(n/(f * 2 * Math.PI)) * scaleY;
                var y = centreY - func(n) * scaleY;
                p.Points.Add(new Point(n, y));
            }
            //p.Points.Dump();
            p.Stroke = Brushes.Black;
            p.StrokeThickness = 1;
            canvasGraph.Children.Add(p);

            for (int n = 0; n < maxPixelsX; n += 30)
            {
                //var y = centreY - Math.Sin(n/(f * 2 * Math.PI)) * 200;
                var y = centreY - func(n) * scaleY;
                var l = new Line();
                l.X1 = n;
                l.X2 = n;
                l.Y1 = centreY;
                l.Y2 = y;
                l.StrokeThickness = 2;
                l.Stroke = Brushes.Red;
                canvasGraph.Children.Add(l);
                var e = new Ellipse();
                e.Fill = Brushes.Red;
                e.Width = 10;
                e.Height = 10;
                Canvas.SetLeft(e, n - 5);
                Canvas.SetTop(e, y - 5);
                canvasGraph.Children.Add(e);
            }
        }
    }
}
