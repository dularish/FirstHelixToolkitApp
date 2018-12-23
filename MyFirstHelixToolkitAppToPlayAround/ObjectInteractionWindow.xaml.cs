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
    /// Interaction logic for ObjectInteractionWindow.xaml
    /// </summary>
    public partial class ObjectInteractionWindow : Window
    {
        public ObjectInteractionWindow()
        {
            InitializeComponent();
        }

        private void AddTriangleToGeometryModel_Click(object sender, RoutedEventArgs e)
        {
            this.AddTriangleToGeometryModel.IsEnabled = false;
        }
    }
}
