using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphPlotter
{
    class GraphDrawer
    {
        private readonly Canvas canvasGraph;
        private readonly Func<double, double> func;

        public double Frequency { get; set; }
        public double SampleRate { get; set; }

        public int MaxSamples { get; set; }

        public double Multiplier { get; set; }

        public GraphDrawer(Canvas canvasGraph)
        {
            Frequency = 2;
            SampleRate = 500;
            MaxSamples = 1000;
            Multiplier = 1;
            //func = (n) => Math.Sin((n * Frequency * 2 * Math.PI) / SampleRate) + 0.25 * Math.Sin(Math.PI/8+  ((n * Frequency * 8 * Math.PI) / SampleRate));
            func = (n) => Math.Sin((n * Frequency * 2 * Math.PI) / SampleRate);
            this.canvasGraph = canvasGraph;
        }

        private double GetAmplitudeAt(double n)
        {
            var amplitude = Multiplier * func(n);
            // clip
            if (amplitude > 1.0) amplitude = 1.0;
            else if (amplitude < -1.0) amplitude = -1.0;
            return amplitude;
        }

        public void Plot(double availableHeight, double availableWidth)
        {
            canvasGraph.Children.Clear();

            var p = new Polyline();
            var centreY = availableHeight / 2;
            var scaleY = (availableHeight - 10) / 2;


            var centreLine = new Line() { Stroke = Brushes.Gray, X1 = 0, X2 = availableWidth, Y1 = centreY, Y2 = centreY, StrokeThickness = 2 };
            canvasGraph.Children.Add(centreLine);

            for (int x = 0; x < availableWidth; x++)
            {
                var y = centreY - GetAmplitudeAt(x) * scaleY;
                p.Points.Add(new Point(x, y));
            }
            p.Stroke = Brushes.Black;
            p.StrokeThickness = 2;
            canvasGraph.Children.Add(p);

            var samples = 0;
            for (int x = 0; x < availableWidth && samples++ < MaxSamples; x += 30)
            {
                AddSample(centreY, x, scaleY);
            }
        }

        private void AddSample(double centreY, int x, double scaleY)
        {
            var y = centreY - GetAmplitudeAt(x)*scaleY;
            var l = new Line();
            l.X1 = x;
            l.X2 = x;
            l.Y1 = centreY;
            l.Y2 = y;
            l.StrokeThickness = 2;
            l.Stroke = Brushes.Red;
            canvasGraph.Children.Add(l);
            var e = new Ellipse();
            e.Fill = Brushes.Red;
            e.Width = 10;
            e.Height = 10;
            Canvas.SetLeft(e, x - 5);
            Canvas.SetTop(e, y - 5);
            canvasGraph.Children.Add(e);
        }
    }
}
