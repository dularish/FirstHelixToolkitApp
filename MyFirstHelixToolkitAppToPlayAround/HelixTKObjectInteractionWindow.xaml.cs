using HelixToolkit.Wpf;
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
    /// Interaction logic for HelixTKObjectInteractionWindow.xaml
    /// </summary>
    public partial class HelixTKObjectInteractionWindow : Window
    {
        HelixTKObjectInteractionWindowViewModel modelUI = new HelixTKObjectInteractionWindowViewModel();

        public HelixTKObjectInteractionWindow()
        {
            InitializeComponent();

            this.DataContext = modelUI;
        }

        private void AddTriangleToGeometryModel_Click(object sender, RoutedEventArgs e)
        {
            this.AddTriangleToGeometryModelBtn.IsEnabled = false;

            //Instantiating a new window for adding Triangle
            AddTriangleWindow windowToCreate = new AddTriangleWindow();

            //For enabling to return the triangle back from child window, have added an event
            windowToCreate.TriangleConstructionCompleted += AddTriangleToGeometryModel;

            windowToCreate.Show();
        }

        private void AddTriangleToGeometryModel(Triangle triangleToAdd)
        {
            if(triangleToAdd != null)
            {
                //Since we may return null object from inner Window
                ExtractVertexIndexAndAddToExistingMesh(triangleToAdd.Vertex1Point);
                ExtractVertexIndexAndAddToExistingMesh(triangleToAdd.Vertex2Point);
                ExtractVertexIndexAndAddToExistingMesh(triangleToAdd.Vertex3Point);
            }

            this.AddTriangleToGeometryModelBtn.IsEnabled = true;
        }

        private void ExtractVertexIndexAndAddToExistingMesh(Point3DClassType pointToAdd)
        {
            List<Point3D> listOfVertex1MatchingPoints = meshMain.Positions.Where((s) => s.X == pointToAdd.X && s.Y == pointToAdd.Y && s.Z == pointToAdd.Z).ToList();

            if (listOfVertex1MatchingPoints.Count > 0)
            {
                meshMain.TriangleIndices.Add(meshMain.Positions.IndexOf(listOfVertex1MatchingPoints[0]));
            }
            else
            {
                meshMain.Positions.Add(new Point3D(pointToAdd.X, pointToAdd.Y, pointToAdd.Z));
                meshMain.TriangleIndices.Add(meshMain.Positions.Count - 1);
            }
        }

        private void HelixViewport3D_MouseMove(object sender, MouseEventArgs e)
        {
            string testName = "FirstTriangle";
            GeometryModel3D geometryModel3D =  geometryModel3dRef;
            geometryModel3D.Geometry.SetName(testName + "Geometry");

            Point point = e.GetPosition((UIElement)sender);
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(point, 0.25, 0.25);

            

            HitTestResult result = VisualTreeHelper.HitTest((Viewport3D)(((HelixViewport3D)sender).Viewport), point);

            //Try to use VisualTreeHelper without returning HitTestResult, that is by using only CallBack methods

            if (result != null)
            {
                if (result.VisualHit !=null && result.VisualHit.GetType() == typeof(ModelVisual3D) && ((GeometryModel3D)((ModelVisual3D)result.VisualHit).Content).Geometry.GetName() == (testName + "Geometry"))
                {
                    modelUI.IsGeometryHit = true;
                }
                else
                {
                    modelUI.IsGeometryHit = false;
                }
            }
            else
            {
                modelUI.IsGeometryHit = false;
            }
            

            modelUI.XCoord = point.X;
            modelUI.YCoord = point.Y;

            //To do : With the help of Positions property, implement selection of vertices from points
            //meshMain.Positions
        }

        public class HelixTKObjectInteractionWindowViewModel : INotifyPropertyChanged
        {
            private double xCoord;
            private double yCoord;
            private bool isGeometryHit;

            public event PropertyChangedEventHandler PropertyChanged;


            public double XCoord
            {
                get => xCoord;
                set
                {
                    if(xCoord != value)
                    {
                        xCoord = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            public double YCoord
            {
                get => yCoord; set
                {
                    if(yCoord != value)
                    {
                        yCoord = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            public bool IsGeometryHit
            {
                get => isGeometryHit; set
                {
                    if(IsGeometryHit != value)
                    {
                        isGeometryHit = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
