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
using System.Windows.Shapes;

namespace MyFirstHelixToolkitAppToPlayAround.SurfacePlot
{
    /// <summary>
    /// Interaction logic for SurfacePlotWindow.xaml
    /// </summary>
    public partial class SurfacePlotWindow : Window
    {
        private SurfacePlotViewModel _dataContext;

        public SurfacePlotWindow()
        {
            InitializeComponent();

            _dataContext = new SurfacePlotViewModel();
            this.DataContext = _dataContext;
        }

        private void _funcTypeCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _dataContext.PredefinedFunctionSelected = (FunctionType)_funcTypeCmbBox.SelectedItem;
        }

        private void _plotCurveBtn_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.ResetPoints();
        }
    }
}
