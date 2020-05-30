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

namespace MyFirstHelixToolkitAppToPlayAround
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HelixTKObjectInteractionWindow nextWindow = new HelixTKObjectInteractionWindow();
            this.Close();
            nextWindow.Show();
        }

        private void ChartsView_Click(object sender, RoutedEventArgs e)
        {
            ChartsDisplayWindow chartsDisplayWindow = new ChartsDisplayWindow();
            this.Close();
            chartsDisplayWindow.Show();
        }

        private void ThreadingExp_Click(object sender, RoutedEventArgs e)
        {
            ThreadingExpWindow threadingExpWindow = new ThreadingExpWindow();
            this.Close();
            threadingExpWindow.Show();
        }

        private void surfacePlotNavigationBtn_Click(object sender, RoutedEventArgs e)
        {
            SurfacePlot.SurfacePlotWindow surfacePlotWindow = new SurfacePlot.SurfacePlotWindow();
            Close();
            surfacePlotWindow.Show();
        }
    }
}
