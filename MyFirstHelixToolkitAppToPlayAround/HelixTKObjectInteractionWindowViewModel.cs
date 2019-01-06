using HelixToolkit.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MyFirstHelixToolkitAppToPlayAround
{
    public class HelixTKObjectInteractionWindowViewModel : INotifyPropertyChanged
    {
        private double xCoord;
        private double yCoord;
        private bool isGeometryHit;
        private double translationX;
        private double translationY;
        private double translationZ;
        private string modelHitName;
        private GeometryModel3D geometryModel3DHit;
        private Material materialInfoBackedUpForGeometryModel3DHit;
        
        private Model3DGroup basemodel3DGroup;
        private Model3D activeModel3D;
        private string activeModel3DName;
        private Model3DTransformation activeModel3DTransformation;
        private Dictionary<Model3D, Model3DTransformation> model3DTransformationDataMap = new Dictionary<Model3D, Model3DTransformation>();


        public event PropertyChangedEventHandler PropertyChanged;


        public double XCoord
        {
            get => xCoord;
            set
            {
                if (xCoord != value)
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
                if (yCoord != value)
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
                if (IsGeometryHit != value)
                {
                    isGeometryHit = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double TranslationX
        {
            get => translationX; set
            {
                if (translationX != value)
                {
                    translationX = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double TranslationY
        {
            get => translationY; set
            {
                if (translationY != value)
                {
                    translationY = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double TranslationZ
        {
            get => translationZ; set
            {
                if (translationZ != value)
                {
                    translationZ = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ModelHitName
        {
            get => modelHitName; set
            {
                if (modelHitName != value)
                {
                    modelHitName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public GeometryModel3D GeometryModel3DHit
        {
            get => geometryModel3DHit; set
            {
                if (geometryModel3DHit != value)
                {
                    if (geometryModel3DHit != null)
                    {
                        geometryModel3DHit.Material = this.materialInfoBackedUpForGeometryModel3DHit;
                    }

                    geometryModel3DHit = value;

                    if (geometryModel3DHit == null)
                    {
                        IsGeometryHit = false;
                        ModelHitName = string.Empty;
                        this.materialInfoBackedUpForGeometryModel3DHit = null;
                    }
                    else
                    {
                        IsGeometryHit = true;
                        ModelHitName = geometryModel3DHit.GetName();
                        this.materialInfoBackedUpForGeometryModel3DHit = geometryModel3DHit.Material;
                        geometryModel3DHit.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Orange));
                    }
                }
            }
        }

        public string ActiveModel3DName
        {
            get => activeModel3DName; set
            {
                if(activeModel3DName != value)
                {
                    activeModel3DName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Model3DGroup Basemodel3DGroup
        {
            get => basemodel3DGroup; set
            {
                basemodel3DGroup = value;
                model3DTransformationDataMap = new Dictionary<Model3D, Model3DTransformation>();
                ActiveModel3D = value;
                ActiveModel3D.SetName("All Models");
                ActiveModel3DName = ActiveModel3D.GetName();
            }
        }

        public Model3DTransformation ActiveModel3DTransformation
        {
            get => activeModel3DTransformation; set
            {
                if(activeModel3DTransformation != value)
                {
                    activeModel3DTransformation = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Model3D ActiveModel3D
        {
            get => activeModel3D; set
            {
                activeModel3D = value;
                ActiveModel3DName = activeModel3D.GetName();

                if (!model3DTransformationDataMap.ContainsKey(activeModel3D))
                {
                    model3DTransformationDataMap.Add(activeModel3D, new Model3DTransformation());
                }

                ActiveModel3DTransformation = model3DTransformationDataMap[activeModel3D];
            }
        }

        public HelixTKObjectInteractionWindowViewModel(Model3DGroup model3DGroup)
        {
            Basemodel3DGroup = model3DGroup;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Model3DTransformation : INotifyPropertyChanged
    {
        private double translationX;
        private double translationY;
        private double translationZ;

        private double rotationAngleByXAxis;
        private double rotationAngleByYAxis;
        private double rotationAngleByZAxis;

        public double TranslationX
        {
            get => translationX; set
            {
                if (translationX != value)
                {
                    translationX = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double TranslationY
        {
            get => translationY; set
            {
                if(translationY != value)
                {
                    translationY = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double TranslationZ
        {
            get => translationZ; set
            {
                if (translationZ != value)
                {
                    translationZ = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double RotationAngleByXAxis
        {
            get => rotationAngleByXAxis; set
            {
                if (rotationAngleByXAxis != value)
                {
                    rotationAngleByXAxis = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double RotationAngleByYAxis
        {
            get => rotationAngleByYAxis; set
            {
                if (rotationAngleByYAxis != value)
                {
                    rotationAngleByYAxis = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public double RotationAngleByZAxis
        {
            get => rotationAngleByZAxis; set
            {
                if (rotationAngleByZAxis != value)
                {
                    rotationAngleByZAxis = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Model3DTransformation()
        {
            //All double fields get automatically assigned to zero, hence no work here
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
