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

namespace MyFirstHelixToolkitAppToPlayAround
{
    /// <summary>
    /// Interaction logic for ChartsDisplayWindow.xaml
    /// </summary>
    public partial class ChartsDisplayWindow : Window
    {
        ChartsDisplayViewModel modelUI;
        public ChartsDisplayWindow()
        {
            InitializeComponent();
            modelUI = new ChartsDisplayViewModel();
            this.DataContext = modelUI;

        }
    }
}
