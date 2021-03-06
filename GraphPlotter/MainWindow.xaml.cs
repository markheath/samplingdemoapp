﻿using System;
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
using System.Windows.Threading;

namespace GraphPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GraphDrawer graphDrawer;
        private bool redrawNeeded = false;
        private readonly DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            graphDrawer = new GraphDrawer(canvasGraph);
            timer = new DispatcherTimer(TimeSpan.FromSeconds(0.2), DispatcherPriority.Normal, OnTimerTick, Dispatcher.CurrentDispatcher);
            this.Loaded += RedrawNeeded;
            this.SizeChanged += RedrawNeeded;
            samplesUpDown.ValueChanged += RedrawNeeded;
            sliderAmplitude.ValueChanged += RedrawNeeded;
            checkBox16Bit.Click += RedrawNeeded;
            checkBoxFade.Click += RedrawNeeded;
            this.KeyUp += MainWindow_KeyUp;
        }

        void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                checkBoxFade.IsChecked = !checkBoxFade.IsChecked.Value;
                redrawNeeded = true;
            }
            /*if (e.Key == Key.Up)
            {
                samplesUpDown.Value++;
            }
            else if (e.Key == Key.Down)
            {
                samplesUpDown.Value--;
            }*/
        }

        private void RedrawNeeded(object sender, EventArgs args)
        {
            redrawNeeded = true;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (redrawNeeded)
            {
                graphDrawer.MaxSamples = samplesUpDown.Value.GetValueOrDefault(1000);
                graphDrawer.Multiplier = sliderAmplitude.Value;
                graphDrawer.Fade = checkBoxFade.IsChecked.Value;
                

                listBoxSamples.ItemsSource = null;
                graphDrawer.Plot(canvasGraph.ActualHeight, canvasGraph.ActualWidth);
                if (checkBox16Bit.IsChecked.Value)
                {
                    listBoxSamples.ItemStringFormat = "";
                    listBoxSamples.ItemsSource = graphDrawer.Samples.Select(s => (Int16)(s * 32767)); 
                }
                else
                {
                    listBoxSamples.ItemStringFormat = "0.00";
                    listBoxSamples.ItemsSource = graphDrawer.Samples;
                }
                redrawNeeded = false;
            }
        }
    }

}
