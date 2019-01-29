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
        private int _XDomainMin;
        private int _XDomainMax;
        private double _YDomainMin;
        private double _YDomainMax;
        

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
                FunctionSelected = FunctionKeys[0];
                
            }
        }

        public string FunctionSelected
        {
            get => _FunctionSelected; set
            {
                _FunctionSelected = value;

                NotifyPropertyChanged();
                RedrawCurveForModelPlot();
            }
        }

        public int XDomainMin
        {
            get => _XDomainMin; set
            {
                _XDomainMin = value;

                NotifyPropertyChanged();
                RedrawCurveForModelPlot();
            }
        }
        public int XDomainMax
        {
            get => _XDomainMax; set
            {
                _XDomainMax = value;

                NotifyPropertyChanged();
                RedrawCurveForModelPlot();
            }
        }
        public double YDomainMin
        {
            get => _YDomainMin; set
            {
                _YDomainMin = value;

                NotifyPropertyChanged();
                RedrawCurveForModelPlot();
            }
        }
        public double YDomainMax
        {
            get => _YDomainMax; set
            {
                _YDomainMax = value;

                NotifyPropertyChanged();
                RedrawCurveForModelPlot();
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
            FunctionSelected = FunctionKeys[0];

            XDomainMin = 0;
            XDomainMax = 360;
            YDomainMin = -2;
            YDomainMax = 2;
            

            RedrawCurveForModelPlot();
        }

        private void RedrawCurveForModelPlot()
        {
            if (!_Functions.Keys.Contains(FunctionSelected))
            {
                throw new Exception("Curve not found");
            }

            PlotModel modelPlot = new PlotModel { Title = "Line plot", Subtitle = FunctionSelected + " Curve" };


            LineSeries series = new LineSeries
            {
                StrokeThickness = 1,
                Color = OxyColor.FromRgb(255, 0, 0)
                //MarkerSize = 3,
                //MarkerStroke = OxyColors.ForestGreen,
                //MarkerType = MarkerType.Plus
            };
            double totalDataValue = 0;
            double totalDataNumbers = 0;
            double average = 0;
            List<DataPoint> dataPoints = new List<DataPoint>();
            for (int i = XDomainMin; i < XDomainMax; i++)
            {
                double dataValue = Functions[FunctionSelected](i);

                if(dataValue > YDomainMin && dataValue < YDomainMax)
                {
                    dataPoints.Add(new DataPoint(i, dataValue));

                    totalDataValue += dataValue;
                    totalDataNumbers++;
                }
                
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
            _Functions.Add("Log", (double i) => Math.Log10(i));
            _Functions.Add("Linear", (double i) => (i * 0.15 + 0.25));
            _Functions.Add("Exp", (double i) => Math.Exp(i)/100000);

            _FunctionKeys = _Functions.Keys.ToList();
        }
    }
}
