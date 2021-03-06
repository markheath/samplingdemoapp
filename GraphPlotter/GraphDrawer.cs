﻿using System;
using System.Collections;
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
    internal class GraphDrawer
    {
        private readonly Canvas canvasGraph;
        private readonly Func<double, double> func;
        private readonly List<double> samples;
        public double Frequency { get; set; }
        public double SampleRate { get; set; }

        public int MaxSamples { get; set; }

        public double Multiplier { get; set; }

        public IEnumerable<double> Samples { get { return samples; } }

        public bool Fade { get; set; }

        public GraphDrawer(Canvas canvasGraph)
        {
            Frequency = 2; //16; // 2 for the first example, 16 for the aliasing example
            SampleRate = 500; // 500 for first example
            MaxSamples = 1000;
            Multiplier = 1;
            //func = (n) => Math.Sin((n * Frequency * 2 * Math.PI) / SampleRate) + 0.25 * Math.Sin(Math.PI/8+  ((n * Frequency * 8 * Math.PI) / SampleRate));
            // to create square wave sin x + sin 3x / 3 + sin 5x / 5 etc
            func = (n) => Math.Sin((n*Frequency*2*Math.PI)/SampleRate);
            this.canvasGraph = canvasGraph;
            samples = new List<double>();
        }

        private Brush CentreLineBrush
        {
            get { return Fade ? Brushes.LightGray : Brushes.Gray; }
        }

        private Brush SampleLineBrush
        {
            get { return Fade ? Brushes.LightGray : Brushes.Red; }
        }

        private Brush SignalBrush
        {
            get { return Fade ? Brushes.LightGray : Brushes.Black;  }
        }

        private double GetAmplitudeAt(double n)
        {
            var amplitude = Multiplier*func(n);
            // clip
            if (amplitude > 1.0) amplitude = 1.0;
            else if (amplitude < -1.0) amplitude = -1.0;
            return amplitude;
        }

        public void Plot(double availableHeight, double availableWidth)
        {
            canvasGraph.Children.Clear();

            var p = new Polyline();
            var centreY = availableHeight/2;
            var scaleY = (availableHeight - 10)/2;


            var centreLine = new Line()
                {
                    Stroke = CentreLineBrush,
                    X1 = 0,
                    X2 = availableWidth,
                    Y1 = centreY,
                    Y2 = centreY,
                    StrokeThickness = 2
                };
            canvasGraph.Children.Add(centreLine);

            for (int x = 0; x < availableWidth; x++)
            {
                var y = centreY - GetAmplitudeAt(x)*scaleY;
                p.Points.Add(new Point(x, y));
            }
            p.Stroke = SignalBrush;
            p.StrokeThickness = 2;
            canvasGraph.Children.Add(p);

            samples.Clear();
            for (int x = 0; x < availableWidth && samples.Count < MaxSamples; x += 30)
            {
                AddSample(centreY, x, scaleY);
            }
        }

        private void AddSample(double centreY, int x, double scaleY)
        {
            var sample = GetAmplitudeAt(x);
            samples.Add(sample);
            var y = centreY - sample*scaleY;
            var l = new Line();
            l.X1 = x;
            l.X2 = x;
            l.Y1 = centreY;
            l.Y2 = y;
            l.StrokeThickness = 2;
            l.Stroke = SampleLineBrush;
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
