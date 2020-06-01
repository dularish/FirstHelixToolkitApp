using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MyFirstHelixToolkitAppToPlayAround.SurfacePlot
{
    public class SurfacePlotViewModel : INotifyPropertyChanged
    {
        private FunctionType _predefinedFunctionSelected;
        private double _minX;
        private double _maxX;
        private int _xPoints;
        private double _minY;
        private double _maxY;
        private int _yPoints;
        private Point3D[,] _plotPoints;
        public Array FunctionTypesList
        {
            get
            {
                return Enum.GetValues(typeof(FunctionType));
            }
        }
        public FunctionType PredefinedFunctionSelected
        {
            get => _predefinedFunctionSelected; internal set
            {
                _predefinedFunctionSelected = value;
                onPropertyChanged();
            }
        }

        public double MinX
        {
            get => _minX; set
            {
                _minX = value;
                onPropertyChanged();
            }
        }

        public double MaxX
        {
            get => _maxX; set
            {
                _maxX = value;
                onPropertyChanged();
            }
        }

        internal void ResetPoints(bool? isPredefinedPoints)
        {
            if (isPredefinedPoints.GetValueOrDefault())
            {
                switch (PredefinedFunctionSelected)
                {
                    case FunctionType.SinC:
                        PlotPoints = getPlotPoints((x, y) => 10 * (Math.Sin(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)))) / Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
                        break;
                    case FunctionType.Saddle:
                        PlotPoints = getPlotPoints((x, y) => Math.Pow(x, 2.0) - Math.Pow(y, 2.0));
                        break;
                    case FunctionType.Ripple:
                        PlotPoints = getPlotPoints((x, y) => 0.5 * Math.Sin(x * y));
                        break;
                }
            }
        }

        private Point3D[,] getPlotPoints(Func<double, double, double> function)
        {
            var points = new Point3D[XPoints, YPoints];
            var xInterval = (MaxX - MinX) / XPoints;
            var yInterval = (MaxY - MinY) / YPoints;

            for (int i = 0; i < XPoints; i++)
            {
                for (int j = 0; j < YPoints; j++)
                {
                    double x = MinX + (i * xInterval);
                    double y = MinY + (j * yInterval);
                    double z = function(x,y);
                    points[i, j] = new Point3D(x, y, z);
                }
            }

            return points;
        }

        public int XPoints
        {
            get => _xPoints; set
            {
                _xPoints = value;
                onPropertyChanged();
            }
        }

        public double MinY
        {
            get => _minY; set
            {
                _minY = value;
                onPropertyChanged();
            }
        }

        public double MaxY
        {
            get => _maxY; set
            {
                _maxY = value;
                onPropertyChanged();
            }
        }

        public int YPoints
        {
            get => _yPoints; set
            {
                _yPoints = value;
                onPropertyChanged();
            }
        }

        public Point3D[,] PlotPoints
        {
            get => _plotPoints; set
            {
                _plotPoints = value;
                onPropertyChanged();
            }
        }

        private void onPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SurfacePlotViewModel()
        {
            MinX = -10;
            MaxX = 10;
            MinY = -10;
            MaxY = 10;
            XPoints = 200;
            YPoints = 200;
            PredefinedFunctionSelected = FunctionType.SinC;

            ResetPoints(true);
        }
    }

    public enum FunctionType
    {
        SinC, Saddle, Ripple
    }
}
