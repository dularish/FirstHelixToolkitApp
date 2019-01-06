using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MyFirstHelixToolkitAppToPlayAround
{
    public interface IModel3dTransformer
    {
        void Transform(Model3D model3D);
    }


    public abstract class Model3DTransformer : IModel3dTransformer
    {
        protected Model3D Model3DToTransform { get; set; }

        protected abstract void ApplyTransformation();
        
        public void Transform(Model3D model3D)
        {
            if(model3D != null)
            {
                Model3DToTransform = model3D;

                ApplyTransformation();
            }
            else
            {
                throw new Exception("NULL passed instead of a Model3D object");
            }
        }
    }

    public class TranslationModel3DTransformer : Model3DTransformer
    {
        private double _offsetXDim { get; }
        private double _offsetYDim { get; }
        private double _offsetZDim { get; }


        public TranslationModel3DTransformer(double xTranslation, double yTranslation, double zTranslation)
        {
            _offsetXDim = xTranslation;
            _offsetYDim = yTranslation;
            _offsetZDim = zTranslation;
        }

        protected override void ApplyTransformation()
        {
            var targetObject = this.Model3DToTransform;

            Transform3DGroup transform3DGroup = targetObject.Transform as Transform3DGroup ?? new Transform3DGroup();

            TranslateTransform3D translationTransformObj = transform3DGroup.Children.Select(s => s as TranslateTransform3D).Where(s => s != null).FirstOrDefault();

            if (translationTransformObj == null)
            {
                translationTransformObj = new TranslateTransform3D(new Vector3D(0, 0, 0));
                transform3DGroup.Children.Add(translationTransformObj);
            }

            translationTransformObj.OffsetX = _offsetXDim;
            translationTransformObj.OffsetY = _offsetYDim;
            translationTransformObj.OffsetZ = _offsetZDim;

            targetObject.Transform = transform3DGroup;
        }
    }

    public class RotationModel3DTransformer : Model3DTransformer
    {
        private Vector3D _axis;
        private Point3D _center;
        private double _rotationAngle;


        public RotationModel3DTransformer(Vector3D axis, Point3D center, double angle)
        {
            _axis = axis;
            _center = center;
            _rotationAngle = angle;
        }

        protected override void ApplyTransformation()
        {
            var targetObject = this.Model3DToTransform;

            Transform3DGroup transform3DGroup = targetObject.Transform as Transform3DGroup ?? new Transform3DGroup();

            //Fetching RotateTransform with similar vector direction and same center
            RotateTransform3D rotateTransform3DObj = transform3DGroup.Children.Select(s => s as RotateTransform3D)
                                                        .Where(s => s != null && (s.Rotation as AxisAngleRotation3D) != null
                                                            && Vector3D.CrossProduct(_axis, (s.Rotation as AxisAngleRotation3D).Axis).Length == 0
                                                            && s.CenterX == _center.X && s.CenterY == _center.Y && s.CenterZ == _center.Z).FirstOrDefault();


            if(rotateTransform3DObj == null)
            {
                rotateTransform3DObj = new RotateTransform3D(new AxisAngleRotation3D(_axis, _rotationAngle), _center);
                transform3DGroup.Children.Add(rotateTransform3DObj);
            }

            (rotateTransform3DObj.Rotation as AxisAngleRotation3D).Angle = _rotationAngle;

            targetObject.Transform = transform3DGroup;
        }
    }
}
