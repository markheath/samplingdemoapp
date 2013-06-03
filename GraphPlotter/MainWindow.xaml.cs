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
        private readonly GraphDrawer graphDrawer;
        public MainWindow()
        {
            InitializeComponent();
            graphDrawer = new GraphDrawer(canvasGraph);
            this.Loaded += (sender, args) => graphDrawer.Plot(canvasGraph.ActualHeight, canvasGraph.ActualWidth);
            
        }
    }
}
