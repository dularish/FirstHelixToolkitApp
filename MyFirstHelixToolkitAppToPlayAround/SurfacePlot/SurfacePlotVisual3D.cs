using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MyFirstHelixToolkitAppToPlayAround.SurfacePlot
{
    public class SurfacePlotVisual3D : ModelVisual3D
    {
        public static Point3D[,] DefaultPoints
        {
            get
            {
                int n = 100;
                var points = new Point3D[2*n, 2*n];

                for (int i = -n; i < n; i++)
                {
                    for (int j = -n; j < n; j++)
                    {
                        double x = i / ((double)n) * Math.PI;
                        double y = j / ((double)n) * Math.PI;
                        double z = Math.Pow(x, 2.0) - Math.Pow(y, 2.0);
                        //10 * (Math.Sin(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)))) / Math.Sqrt(Math.Pow(x,2) + Math.Pow(y,2));//Sin curve
                        //0.5 * Math.Sin(x*y);//Ripple like
                        //Math.Pow(x,2.0) - Math.Pow(y,2.0);//Saddle
                        points[i + n, j + n] = new Point3D(x, y, z);
                    }
                }

                return points;
            }
        }

        public Point3D[,] PlotPoints
        {
            get { return (Point3D[,])GetValue(PlotPointsProperty); }
            set { SetValue(PlotPointsProperty, value); }
        }

        public static readonly DependencyProperty PlotPointsProperty =
            DependencyProperty.Register("PlotPoints", typeof(Point3D[,]), typeof(SurfacePlotVisual3D), new PropertyMetadata(DefaultPoints, OnPlotChange));



        public Brush SurfaceBrush
        {
            get { return (Brush)GetValue(SurfaceBrushProperty); }
            set { SetValue(SurfaceBrushProperty, value); }
        }

        public static readonly DependencyProperty SurfaceBrushProperty =
            DependencyProperty.Register("SurfaceBrush", typeof(Brush), typeof(SurfacePlotVisual3D), new PropertyMetadata(Brushes.Green, OnPlotChange));

        public SurfacePlotVisual3D()
        {
            Children.Clear();

            Content = CreatePlot();
        }

        private static void OnPlotChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SurfacePlotVisual3D)?.UpdatePlot();
        }

        private void UpdatePlot()
        {
            Children.Clear();

            Content = CreatePlot();
        }

        private Model3D CreatePlot()
        {
            var modelGroup = new Model3DGroup();

            MeshBuilder surfaceMeshBuilder = new MeshBuilder();
            surfaceMeshBuilder.AddRectangularMesh(PlotPoints);

            var solidModel = new GeometryModel3D(surfaceMeshBuilder.ToMesh(), MaterialHelper.CreateMaterial(SurfaceBrush));
            solidModel.BackMaterial = solidModel.Material;

            modelGroup.Children.Add(solidModel);

            return modelGroup;
        }
    }
}
