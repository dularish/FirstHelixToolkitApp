using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace MyFirstHelixToolkitAppToPlayAround
{
    /// <summary>
    /// Interaction logic for AddTriangleWindow.xaml
    /// </summary>
    public partial class AddTriangleWindow : Window
    {
        #region << Public Properties, Events>>

        /// <summary>
        /// This is triggered when the user has finished selecting points and a proper triangle dimensions are available
        /// </summary>
        public event Action<Triangle> TriangleConstructionCompleted;

        #endregion

        #region <<Private fields>>
        Triangle triangleToBind = new Triangle();

        #endregion

        public AddTriangleWindow()
        {
            InitializeComponent();

            InitializeDataContextBinding();
        }

        private void InitializeDataContextBinding()
        {
            this.DataContext = triangleToBind;
        }

        private void AddTriangleBtn_Click(object sender, RoutedEventArgs e)
        {
            TriangleConstructionCompleted?.Invoke(triangleToBind);

            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            TriangleConstructionCompleted?.Invoke(null);
        }
    }

    /// <summary>
    /// This holds all the vertices of a triangle
    /// </summary>
    public class Triangle : INotifyPropertyChanged
    {
        private Point3DClassType vertex1Point;
        private Point3DClassType vertex2Point;
        private Point3DClassType vertex3Point;
        private string triangleName;


        public Point3DClassType Vertex1Point
        {
            get => vertex1Point; set
            {
                vertex1Point = value;
                NotifyPropertyChanged();
            }
        }
        public Point3DClassType Vertex2Point
        {
            get => vertex2Point; set
            {
                vertex2Point = value;
                NotifyPropertyChanged();
            }
        }
        public Point3DClassType Vertex3Point
        {
            get => vertex3Point; set
            {
                vertex3Point = value;
                NotifyPropertyChanged();
            }
        }
        public string TriangleName
        {
            get => triangleName; set
            {
                triangleName = value;
                NotifyPropertyChanged();
            }
        }

        public Triangle()
        {
            Vertex1Point = new Point3DClassType(10, 10, 10);
            Vertex2Point = new Point3DClassType(0, 0, 0);
            Vertex3Point = new Point3DClassType(0, 0, 0);
            TriangleName = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// This is constructed to address the need to be able to bind point coordinates to UI controls as there is no class form of a Point(It's defined struct in msdn classes)
    /// </summary>
    public class Point3DClassType : INotifyPropertyChanged
    {
        private double x;
        private double y;
        private double z;


        public double X
        {
            get => x; set
            {
                x = value;
            }
        }
        public double Y
        {
            get => y; set
            {
                y = value;
            }
        }
        public double Z
        {
            get => z; set
            {
                z = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Point3DClassType(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
