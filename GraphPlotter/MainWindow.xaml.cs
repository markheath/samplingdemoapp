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
            timer = new DispatcherTimer(TimeSpan.FromSeconds(0.5), DispatcherPriority.Normal, OnTimerTick, Dispatcher.CurrentDispatcher);
            this.Loaded += (sender, args) => redrawNeeded = true;
            this.SizeChanged += (sender, args) => redrawNeeded = true;

        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (redrawNeeded)
            {
                graphDrawer.Plot(canvasGraph.ActualHeight, canvasGraph.ActualWidth);
                redrawNeeded = false;
            }
        }
    }

}
