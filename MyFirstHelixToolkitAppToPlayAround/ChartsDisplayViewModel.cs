using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstHelixToolkitAppToPlayAround
{
    public class ChartsDisplayViewModel : INotifyPropertyChanged
    {
        private PlotModel _ModelPlot;

        private Dictionary<string, Func<double, double>> _Functions;
        private List<string> _FunctionKeys;
        private string _FunctionSelected;

        public PlotModel ModelPlot
        {
            get => _ModelPlot; set
            {
                _ModelPlot = value;
                NotifyPropertyChanged();
            }
        }

        public Dictionary<string, Func<double, double>> Functions { get => _Functions; }
        public List<string> FunctionKeys
        {
            get => _FunctionKeys; set
            {
                _FunctionKeys = value;
                NotifyPropertyChanged();
            }
        }

        public string FunctionSelected
        {
            get => _FunctionSelected; set
            {
                _FunctionSelected = value;

                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChartsDisplayViewModel()
        {
            InitializeFunctions();

            string curveName = "Sin";

            PlotModel modelPlot = new PlotModel{ Title = "Line plot", Subtitle = curveName + " Curve" };


            LineSeries series = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColor.FromRgb(255,0, 0)
                //MarkerSize = 3,
                //MarkerStroke = OxyColors.ForestGreen,
                //MarkerType = MarkerType.Plus
            };
            double totalDataValue = 0;
            double totalDataNumbers = 0;
            double average = 0;
            List<DataPoint> dataPoints = new List<DataPoint>();
            for (int i = 0; i < 360; i++)
            {
                double dataValue = Functions[curveName](i);

                dataPoints.Add(new DataPoint(i, dataValue));

                totalDataValue += dataValue;
                totalDataNumbers++;
            }
            average = totalDataValue / totalDataNumbers;

            

            //dataPoints = dataPoints.Where(s => s.Y < 2 * average).ToList();

            foreach (DataPoint point in dataPoints)
            {
                series.Points.Add(point);
            }

            modelPlot.Series.Add(series);

            this.ModelPlot = modelPlot;
        }

        private void InitializeFunctions()
        {
            _Functions = new Dictionary<string, Func<double, double>>();

            _Functions.Add("Sin", (double i) => Math.Sin(i * Math.PI / 180));
            _Functions.Add("Cos", (double i) => Math.Cos(i * Math.PI / 180));
            _Functions.Add("Tan", (double i) => Math.Tan(i * Math.PI / 180));

            _FunctionKeys = _Functions.Keys.ToList();
        }
    }
}
