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
        private string _customExpression;
        private bool _isPredefinedFuncMode = true;
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

        private string getExpressionString()
        {
            if (IsPredefinedFuncMode)
            {
                switch (PredefinedFunctionSelected)
                {
                    case FunctionType.SinC:
                        return "sin(sqrt(x^2+y^2))/sqrt(x^2 + y^2)";
                    case FunctionType.Saddle:
                        return "x^2 + y^2";
                    case FunctionType.Ripple:
                        return "0.5sin(xy)";
                    case FunctionType.CalcPlot1: return "7xy/e^(x^2+y^2)";
                    case FunctionType.CalcPlot2: return "-4x/(x^2+y^2+1)";
                    case FunctionType.CalcPlot3: return "x^2 + y^2";
                    case FunctionType.CalcPlot4: return "1/(y-x^2)";
                    case FunctionType.CalcPlot5: return "2 - x^2 - y^2";
                    case FunctionType.CalcPlot6: return "x^2 - y^2";
                    case FunctionType.CalcPlot7: return "(x+y)/(x-y)";
                    case FunctionType.CalcPlot8: return "5-x^2-4x-y^2";
                    case FunctionType.CalcPlot9: return "-3xy/(x^3+y^3)";
                    case FunctionType.CalcPlot10: return "e^(-(x^2 + y^2))";
                    case FunctionType.CalcPlot11: return "sqrt(4-x^2-y^2)";
                    case FunctionType.CalcPlot12: return "7x/e^(x^5+y^2)";
                    case FunctionType.CalcPlot13: return "2 + x/4 + y/2";
                    case FunctionType.CalcPlot14: return "3x + 5y - 1";
                    case FunctionType.CalcPlot15: return "arcsin(x + y)";
                    case FunctionType.CalcPlot16: return "arccos(y-x^2)";
                    case FunctionType.CalcPlot17: return "e^(x^2 + 2x - y)";
                    case FunctionType.CalcPlot18: return "Sin(x^2 + y^2)";
                    case FunctionType.CalcPlot19: return "Cos(x^2 + y^2)";
                    case FunctionType.CalcPlot20: return "1/Cos(xy)";
                    case FunctionType.CalcPlot21: return "2y^3+3x^2-6xy";
                    case FunctionType.CalcPlot22: return "(10x^2y - 5x^2 - 4y^2 - x^4 - 2y^4)/2";
                    case FunctionType.CalcPlot23: return "x^4 + y^4 - 4xy + 1";
                    case FunctionType.CalcPlot24: return "3 + x^3 + y^3 - 3xy";
                    case FunctionType.CalcPlot25: return "y^4 - x^3 - 2y^2 + 3x";
                    case FunctionType.CalcPlot26: return "(x^2 + 4y^2)*e^(1-x^2-y^2)";
                    case FunctionType.CalcPlot27: return "ln(x^2-y)";
                    case FunctionType.CalcPlot28: return "ln(y-x^2)";
                    case FunctionType.CalcPlot29: return "Sin(x^2-y^2)";
                    case FunctionType.CalcPlot30: return "Cos(x)*Sin(y)";
                    case FunctionType.CalcPlot31: return "Sin(x+y)";
                    case FunctionType.CalcPlot32: return "x*sin(y)";
                    case FunctionType.CalcPlot33: return "ln(1+x^2-y)";
                    case FunctionType.CalcPlot34: return "Cos(xy)";
                    case FunctionType.CalcPlot35: return "Cos(x) - Sin(y)";
                    case FunctionType.CalcPlot36: return "Sin(2x) + Cos(y)";
                    case FunctionType.CalcPlot37: return "Cos(y)/Sin(x)";
                    case FunctionType.CalcPlot38: return "x*e^y + 1";
                    case FunctionType.CalcPlot39: return "Sqrt(x^2+y^2)";
                    case FunctionType.CalcPlot40: return "e^(abs(xy))-1";
                    case FunctionType.CalcPlot41: return "1-e^(abs(xy))";
                    case FunctionType.CalcPlot42: return "e^(xy) - 1";
                    case FunctionType.CalcPlot43: return "(2x^2+y^2)*e^(1-x^2-y^2)";
                    case FunctionType.CalcPlot44: return "e^sqrt(x^2+y^2)";
                    case FunctionType.CalcPlot45: return "int(sin(x+y)+1)";
                    case FunctionType.CalcPlot46: return "cos(sqrt(x^2+y^2))";
                    case FunctionType.CalcPlot47: return "(cos(x))^2*(cos(y))^2";
                    case FunctionType.CalcPlot48: return "sin(x^2+y^2)/(x^2 + y^2)";
                    case FunctionType.CalcPlot49: return "(2 - y^2 +x^2)*e^(1-x^2-y^2/4)";
                    case FunctionType.CalcPlot50: return "- 1 / Sqrt(x ^ 2 + y ^ 2)";
                    case FunctionType.CalcPlot51: return "(1-x^2-y^2)/sqrt(abs(1-x^2-y^2))";
                    case FunctionType.CalcPlot52: return "(x-2y)/(x^2+y^2)";
                    case FunctionType.CalcPlot53: return "((x^2-y^2)/(x^2+y^2))^2";
                    case FunctionType.CalcPlot54: return "sin(cos(x-y))";
                    case FunctionType.CalcPlot55: return "(x^2+y^2)/(xy)";
                    case FunctionType.CalcPlot56: return "x^2*y/(x^4+y^2)";
                    case FunctionType.CalcPlot57: return "(2x-y^2)/(2x^2+y)";
                    case FunctionType.CalcPlot58: return "-3xy/(x^2+y^2)";
                    case FunctionType.CalcPlot59: return "3/sqrt(x^2+y^2)-7/sqrt((x-2)^2+(y+1)^2)";
                    case FunctionType.CalcPlot60: return "xy(x^2-y^2)/(x^2+y^2)";
                    default:
                        return string.Empty;
                }
            }
            else
            {
                return CustomExpression;
            }
        }

        internal void ResetPoints()
        {
            if (IsPredefinedFuncMode)
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
                    case FunctionType.CalcPlot1: PlotPoints = getPlotPoints((x, y) => 7 * x * y / Math.Pow(Math.E, (Math.Pow(x, 2) + Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot2: PlotPoints = getPlotPoints((x, y) => -4 * x / (Math.Pow(x, 2) + Math.Pow(y, 2) + 1)); break;
                    case FunctionType.CalcPlot3: PlotPoints = getPlotPoints((x, y) => Math.Pow(x, 2) + Math.Pow(y, 2)); break;
                    case FunctionType.CalcPlot4: PlotPoints = getPlotPoints((x, y) => 1 / (y - Math.Pow(x, 2))); break;
                    case FunctionType.CalcPlot5: PlotPoints = getPlotPoints((x, y) => 2 - Math.Pow(x, 2) - Math.Pow(y, 2)); break;
                    case FunctionType.CalcPlot6: PlotPoints = getPlotPoints((x, y) => Math.Pow(x, 2) - Math.Pow(y, 2)); break;
                    case FunctionType.CalcPlot7: PlotPoints = getPlotPoints((x, y) => (x + y) / (x - y)); break;
                    case FunctionType.CalcPlot8: PlotPoints = getPlotPoints((x, y) => 5 - Math.Pow(x, 2) - 4 * x - Math.Pow(y, 2)); break;
                    case FunctionType.CalcPlot9: PlotPoints = getPlotPoints((x, y) => -3 * x * y / (Math.Pow(x, 3) + Math.Pow(y, 3))); break;
                    case FunctionType.CalcPlot10: PlotPoints = getPlotPoints((x, y) => Math.Pow(Math.E, (-(Math.Pow(x, 2) + Math.Pow(y, 2))))); break;
                    case FunctionType.CalcPlot11: PlotPoints = getPlotPoints((x, y) => Math.Sqrt(4 - Math.Pow(x, 2) - Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot12: PlotPoints = getPlotPoints((x, y) => 7 * x / Math.Pow(Math.E, (Math.Pow(x, 5) + Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot13: PlotPoints = getPlotPoints((x, y) => 2 + x / 4 + y / 2); break;
                    case FunctionType.CalcPlot14: PlotPoints = getPlotPoints((x, y) => 3 * x + 5 * y - 1); break;
                    case FunctionType.CalcPlot15: PlotPoints = getPlotPoints((x, y) => Math.Asin(x + y)); break;
                    case FunctionType.CalcPlot16: PlotPoints = getPlotPoints((x, y) => Math.Acos(y - Math.Pow(x, 2))); break;
                    case FunctionType.CalcPlot17: PlotPoints = getPlotPoints((x, y) => Math.Pow(Math.E, (Math.Pow(x, 2) + 2 * x - y))); break;
                    case FunctionType.CalcPlot18: PlotPoints = getPlotPoints((x, y) => Math.Sin(Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot19: PlotPoints = getPlotPoints((x, y) => Math.Cos(Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot20: PlotPoints = getPlotPoints((x, y) => 1 / Math.Cos(x * y)); break;
                    case FunctionType.CalcPlot21: PlotPoints = getPlotPoints((x, y) => 2 * Math.Pow(y, 3) + 3 * Math.Pow(x, 2) - 6 * x * y); break;
                    case FunctionType.CalcPlot22: PlotPoints = getPlotPoints((x, y) => (10 * Math.Pow(x, 2) * y - 5 * Math.Pow(x, 2) - 4 * Math.Pow(y, 2) - Math.Pow(x, 4) - 2 * Math.Pow(y, 4)) / 2); break;
                    case FunctionType.CalcPlot23: PlotPoints = getPlotPoints((x, y) => Math.Pow(x, 4) + Math.Pow(y, 4) - 4 * x * y + 1); break;
                    case FunctionType.CalcPlot24: PlotPoints = getPlotPoints((x, y) => 3 + Math.Pow(x, 3) + Math.Pow(y, 3) - 3 * x * y); break;
                    case FunctionType.CalcPlot25: PlotPoints = getPlotPoints((x, y) => Math.Pow(y, 4) - Math.Pow(x, 3) - 2 * Math.Pow(y, 2) + 3 * x); break;
                    case FunctionType.CalcPlot26: PlotPoints = getPlotPoints((x, y) => (Math.Pow(x, 2) + 4 * Math.Pow(y, 2)) * Math.Pow(Math.E, (1 - Math.Pow(x, 2) - Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot27: PlotPoints = getPlotPoints((x, y) => Math.Log(Math.Pow(x, 2) - y)); break;
                    case FunctionType.CalcPlot28: PlotPoints = getPlotPoints((x, y) => Math.Log(y - Math.Pow(x, 2))); break;
                    case FunctionType.CalcPlot29: PlotPoints = getPlotPoints((x, y) => Math.Sin(Math.Pow(x, 2) - Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot30: PlotPoints = getPlotPoints((x, y) => Math.Cos(x) * Math.Sin(y)); break;
                    case FunctionType.CalcPlot31: PlotPoints = getPlotPoints((x, y) => Math.Sin(x + y)); break;
                    case FunctionType.CalcPlot32: PlotPoints = getPlotPoints((x, y) => x * Math.Sin(y)); break;
                    case FunctionType.CalcPlot33: PlotPoints = getPlotPoints((x, y) => Math.Log(1 + Math.Pow(x, 2) - y)); break;
                    case FunctionType.CalcPlot34: PlotPoints = getPlotPoints((x, y) => Math.Cos(x * y)); break;
                    case FunctionType.CalcPlot35: PlotPoints = getPlotPoints((x, y) => Math.Cos(x) - Math.Sin(y)); break;
                    case FunctionType.CalcPlot36: PlotPoints = getPlotPoints((x, y) => Math.Sin(2 * x) + Math.Cos(y)); break;
                    case FunctionType.CalcPlot37: PlotPoints = getPlotPoints((x, y) => Math.Cos(y) / Math.Sin(x)); break;
                    case FunctionType.CalcPlot38: PlotPoints = getPlotPoints((x, y) => x * Math.Pow(Math.E, y) + 1); break;
                    case FunctionType.CalcPlot39: PlotPoints = getPlotPoints((x, y) => Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot40: PlotPoints = getPlotPoints((x, y) => Math.Pow(Math.E, (Math.Abs(x * y))) - 1); break;
                    case FunctionType.CalcPlot41: PlotPoints = getPlotPoints((x, y) => 1 - Math.Pow(Math.E, (Math.Abs(x * y)))); break;
                    case FunctionType.CalcPlot42: PlotPoints = getPlotPoints((x, y) => Math.Pow(Math.E, (x * y)) - 1); break;
                    case FunctionType.CalcPlot43: PlotPoints = getPlotPoints((x, y) => (2 * Math.Pow(x, 2) + Math.Pow(y, 2)) * Math.Pow(Math.E, (1 - Math.Pow(x, 2) - Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot44: PlotPoints = getPlotPoints((x, y) => Math.Pow(Math.E, Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot45: PlotPoints = getPlotPoints((x, y) => Math.Floor(Math.Sin(x + y) + 1)); break;
                    case FunctionType.CalcPlot46: PlotPoints = getPlotPoints((x, y) => Math.Cos(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot47: PlotPoints = getPlotPoints((x, y) => Math.Pow((Math.Cos(x)), 2) * Math.Pow((Math.Cos(y)), 2)); break;
                    case FunctionType.CalcPlot48: PlotPoints = getPlotPoints((x, y) => Math.Sin(Math.Pow(x, 2) + Math.Pow(y, 2)) / (Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot49: PlotPoints = getPlotPoints((x, y) => (2 - Math.Pow(y, 2) + Math.Pow(x, 2)) * Math.Pow(Math.E, (1 - Math.Pow(x, 2) - Math.Pow(y, 2) / 4))); break;
                    case FunctionType.CalcPlot50: PlotPoints = getPlotPoints((x, y) => -1 / Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot51: PlotPoints = getPlotPoints((x, y) => (1 - Math.Pow(x, 2) - Math.Pow(y, 2)) / Math.Sqrt(Math.Abs(1 - Math.Pow(x, 2) - Math.Pow(y, 2)))); break;
                    case FunctionType.CalcPlot52: PlotPoints = getPlotPoints((x, y) => (x - 2 * y) / (Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot53: PlotPoints = getPlotPoints((x, y) => Math.Pow(((Math.Pow(x, 2) - Math.Pow(y, 2)) / (Math.Pow(x, 2) + Math.Pow(y, 2))), 2)); break;
                    case FunctionType.CalcPlot54: PlotPoints = getPlotPoints((x, y) => Math.Sin(Math.Cos(x - y))); break;
                    case FunctionType.CalcPlot55: PlotPoints = getPlotPoints((x, y) => (Math.Pow(x, 2) + Math.Pow(y, 2)) / (x * y)); break;
                    case FunctionType.CalcPlot56: PlotPoints = getPlotPoints((x, y) => Math.Pow(x, 2) * y / (Math.Pow(x, 4) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot57: PlotPoints = getPlotPoints((x, y) => (2 * x - Math.Pow(y, 2)) / (2 * Math.Pow(x, 2) + y)); break;
                    case FunctionType.CalcPlot58: PlotPoints = getPlotPoints((x, y) => -3 * x * y / (Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    case FunctionType.CalcPlot59: PlotPoints = getPlotPoints((x, y) => 3 / Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) - 7 / Math.Sqrt(Math.Pow((x - 2), 2) + Math.Pow((y + 1), 2))); break;
                    case FunctionType.CalcPlot60: PlotPoints = getPlotPoints((x, y) => x * y * (Math.Pow(x, 2) - Math.Pow(y, 2)) / (Math.Pow(x, 2) + Math.Pow(y, 2))); break;
                    default:
                        PlotPoints = getPlotPoints((x, y) => 0.0);
                        break;
                }
            }
            else
            {
                PlotPoints = getPlotPoints(evalCustomExpression(CustomExpression));
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

        private Func<double,double,double> evalCustomExpression(string customExpression)
        {
            return new Func<double, double, double>((x, y) =>
            {
                Dictionary<string, string> dictForEvaluation = new Dictionary<string, string>();
                dictForEvaluation.Add("x", x.ToString());
                dictForEvaluation.Add("y", y.ToString());

                var parsedOutput = RefractoredImpl.parseAndEvaluateExpressionExpressively(customExpression, dictForEvaluation, "customExpression");


                if (parsedOutput.Item1.IsEvaluationSuccess)
                {
                    var evaluatedResultItem = (parsedOutput.Item1 as MathematicalExpressionParser.ExpressionEvaluationResult.EvaluationSuccess).Item;

                    string evaluatedResultString = string.Empty;

                    if (evaluatedResultItem.IsDouble)
                    {
                        return (evaluatedResultItem as MathematicalExpressionParser.AllowedEvaluationResultTypes.Double).Item;
                    }
                    else
                    {
                        return double.NaN;
                    }
                }
                else
                {
                    return double.NaN;
                }
            });
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
                onPropertyChanged(nameof(ViewPortTitleExpression));
            }
        }

        public string CustomExpression
        {
            get => _customExpression; set
            {
                _customExpression = value;
                onPropertyChanged();
            }
        }

        public string ViewPortTitleExpression
        {
            get => getExpressionString();
        }

        public bool IsPredefinedFuncMode
        {
            get => _isPredefinedFuncMode; set
            {
                _isPredefinedFuncMode = value;
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
            MinX = -2;
            MaxX = 2;
            MinY = -2;
            MaxY = 2;
            XPoints = 100;
            YPoints = 100;
            PredefinedFunctionSelected = FunctionType.SinC;

            ResetPoints();
        }
    }

    public enum FunctionType
    {
        SinC, Saddle, Ripple,
        CalcPlot1,
        CalcPlot2,
        CalcPlot3,
        CalcPlot4,
        CalcPlot5,
        CalcPlot6,
        CalcPlot7,
        CalcPlot8,
        CalcPlot9,
        CalcPlot10,
        CalcPlot11,
        CalcPlot12,
        CalcPlot13,
        CalcPlot14,
        CalcPlot15,
        CalcPlot16,
        CalcPlot17,
        CalcPlot18,
        CalcPlot19,
        CalcPlot20,
        CalcPlot21,
        CalcPlot22,
        CalcPlot23,
        CalcPlot24,
        CalcPlot25,
        CalcPlot26,
        CalcPlot27,
        CalcPlot28,
        CalcPlot29,
        CalcPlot30,
        CalcPlot31,
        CalcPlot32,
        CalcPlot33,
        CalcPlot34,
        CalcPlot35,
        CalcPlot36,
        CalcPlot37,
        CalcPlot38,
        CalcPlot39,
        CalcPlot40,
        CalcPlot41,
        CalcPlot42,
        CalcPlot43,
        CalcPlot44,
        CalcPlot45,
        CalcPlot46,
        CalcPlot47,
        CalcPlot48,
        CalcPlot49,
        CalcPlot50,
        CalcPlot51,
        CalcPlot52,
        CalcPlot53,
        CalcPlot54,
        CalcPlot55,
        CalcPlot56,
        CalcPlot57,
        CalcPlot58,
        CalcPlot59,
        CalcPlot60
    }
}
