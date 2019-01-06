using HelixToolkit.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        HelixTKObjectInteractionWindowViewModel modelUI;

        public HelixTKObjectInteractionWindow()
        {
            InitializeComponent();
            modelUI = new HelixTKObjectInteractionWindowViewModel(modelVisual3dRef.Content as Model3DGroup);
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
            geometryModel3D.SetName(testName + "Geometry");

            Point point = e.GetPosition((UIElement)sender);
            EllipseGeometry expandedHitTestArea = new EllipseGeometry(point, 0.25, 0.25);




            //Try to use VisualTreeHelper without returning HitTestResult, that is by using only CallBack methods

            if (VisualTreeHelper.HitTest((Viewport3D)(((HelixViewport3D)sender).Viewport), point) is RayMeshGeometry3DHitTestResult result)
            {
                if (result.VisualHit != null && result.VisualHit.GetType() == typeof(ModelVisual3D))
                {
                    if(result != null && result.ModelHit != null)
                    {
                        modelUI.GeometryModel3DHit = result.ModelHit as GeometryModel3D;
                    }
                    else
                    {
                        modelUI.GeometryModel3DHit = result.ModelHit as GeometryModel3D;
                    }
                    //The below lines are commented as a record to how things were done previously
                    /*Type typeOfContent = ((ModelVisual3D)result.VisualHit).Content.GetType();
                    if (typeOfContent == typeof(GeometryModel3D) && ((GeometryModel3D)((ModelVisual3D)result.VisualHit).Content).Geometry.GetName() == (testName + "Geometry"))
                    {
                        modelUI.IsGeometryHit = true;
                    }
                    else if (typeOfContent == typeof(Model3DGroup))
                    {
                        modelUI.IsGeometryHit = false;
                    }
                    else
                    {
                        modelUI.IsGeometryHit = false;
                    }*/
                    
                }
                else
                {
                    modelUI.GeometryModel3DHit = result.ModelHit as GeometryModel3D;
                }
            }
            else
            {
                modelUI.GeometryModel3DHit = null;
            }


            modelUI.XCoord = point.X;
            modelUI.YCoord = point.Y;

            //To do : With the help of Positions property, implement selection of vertices from points
            //meshMain.Positions
        }

        private void AddModelFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {

                //openFileDialog.InitialDirectory = "D:\\";
                //Have found out by trial that .stl, .3ds, .obj 3d files get rendered correctly
                Filter = "Supported 3d files(*.stl,*.3ds,*.obj)|*.stl;*.3ds;*.obj|All Files(*.*)|*.*",
                FilterIndex = 0
            };

            openFileDialog.ShowDialog();

            string filePath = openFileDialog.FileName;

            if(filePath != null && File.Exists(filePath))
            {
                Model3DCollection modelCollectionFromFilePath = Get3DModelFromFilePath(filePath).Children;
                if(modelCollectionFromFilePath.Count == 0)
                {
                    //Added breakpoint for debugging
                }
                for (int i = 0; i < modelCollectionFromFilePath.Count; i++)
                {
                    Model3D modelItem = modelCollectionFromFilePath[i];
                    string modelName = string.IsNullOrEmpty(modelItem?.GetName()?.Trim()) ? 
                        string.IsNullOrEmpty((modelItem as GeometryModel3D)?.Geometry?.GetName()?.Trim()) ? 
                            System.IO.Path.GetFileNameWithoutExtension(filePath) + (modelCollectionFromFilePath.Count > 1 ? "-" + (i + 1) : "") 
                            : (modelItem as GeometryModel3D)?.Geometry?.GetName() 
                        : modelItem.GetName();
                    
                    if((modelItem as GeometryModel3D)?.Geometry != null)
                    {
                        modelItem.SetName(modelName);
                        (modelItem as GeometryModel3D).Geometry.SetName(modelName);
                    }
                    (modelVisual3dRef.Content as Model3DGroup).Children.Add(modelItem);
                }
            }
        }

        /// <summary>
        /// Gets Model3D object from a physical file
        /// throws exception if unsupported fileFormat file is provided as input
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private Model3DGroup Get3DModelFromFilePath(string filePath)
        {
            Model3DGroup modelToReturn = null;
            try
            {
                ModelImporter importer = new ModelImporter();

                modelToReturn = importer.Load(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught : " + ex.Message);
            }

            return modelToReturn;
        }

        private void Rotate90DegCCWBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Transform3DGroup transform3DGroup = modelVisual3dRef.Content.Transform as Transform3DGroup ?? new Transform3DGroup();
                
                AxisAngleRotation3D axisAngleRotation3D = new AxisAngleRotation3D(new Vector3D(-4, 0, 0), 90);

                RotateTransform3D rotateTransform3D = new RotateTransform3D(axisAngleRotation3D,0,0,0);

                transform3DGroup.Children.Add(rotateTransform3D);

                modelVisual3dRef.Content.Transform = transform3DGroup;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught : " + ex.Message);
            }
        }

        private void SliderRotX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetObject = modelUI.ActiveModel3D;

            Vector3D axis = new Vector3D(-4, 0, 0);
            Point3D center = new Point3D(0, 0, 0);

            Model3DTransformer transformer = new RotationModel3DTransformer(axis, center, modelUI.ActiveModel3DTransformation.RotationAngleByXAxis);

            transformer.Transform(targetObject);
        }

        private void SliderRotY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetObject = modelUI.ActiveModel3D;

            Vector3D axis = new Vector3D(0, -4, 0);
            Point3D center = new Point3D(0, 0, 0);

            Model3DTransformer transformer = new RotationModel3DTransformer(axis, center, modelUI.ActiveModel3DTransformation.RotationAngleByYAxis);

            transformer.Transform(targetObject);
        }

        private void SliderRotZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetObject = modelUI.ActiveModel3D;

            Vector3D axis = new Vector3D(0, 0, -4);
            Point3D center = new Point3D(0, 0, 0);

            Model3DTransformer transformer = new RotationModel3DTransformer(axis, center, modelUI.ActiveModel3DTransformation.RotationAngleByZAxis);

            transformer.Transform(targetObject);
        }

        private void TextBoxTranslationX_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetObject = modelUI.ActiveModel3D;
            
            Model3DTransformer transformer = new TranslationModel3DTransformer(modelUI.ActiveModel3DTransformation.TranslationX, modelUI.ActiveModel3DTransformation.TranslationY, modelUI.ActiveModel3DTransformation.TranslationZ);

            transformer.Transform(targetObject);
        }

        private void ComputationButton_Click(object sender, RoutedEventArgs e)
        {
            Vector3D vectorA = new Vector3D(1, 2, 3);
            Vector3D vectorB = new Vector3D(2, 4, 6);

            Vector3D crossProduct = Vector3D.CrossProduct(vectorA, vectorB);

            return;
        }

        private void TextBoxTranslationY_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetObject = modelUI.ActiveModel3D;

            Model3DTransformer transformer = new TranslationModel3DTransformer(modelUI.ActiveModel3DTransformation.TranslationX, modelUI.ActiveModel3DTransformation.TranslationY, modelUI.ActiveModel3DTransformation.TranslationZ);

            transformer.Transform(targetObject);
        }

        private void TextBoxTranslationZ_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetObject = modelUI.ActiveModel3D;

            Model3DTransformer transformer = new TranslationModel3DTransformer(modelUI.ActiveModel3DTransformation.TranslationX, modelUI.ActiveModel3DTransformation.TranslationY, modelUI.ActiveModel3DTransformation.TranslationZ);

            transformer.Transform(targetObject);
        }

        private void TextBoxTranslationX_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //For one scroll unit, delta value changes by 120, hence assuming it as single unit
            modelUI.ActiveModel3DTransformation.TranslationX += e.Delta/120;
        }

        private void TextBoxTranslationY_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //For one scroll unit, delta value changes by 120, hence assuming it as single unit
            modelUI.ActiveModel3DTransformation.TranslationY += e.Delta / 120;
        }

        private void TextBoxTranslationZ_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //For one scroll unit, delta value changes by 120, hence assuming it as single unit
            modelUI.ActiveModel3DTransformation.TranslationZ += e.Delta / 120;
        }

        private void UpTranslationXBtn_Click(object sender, RoutedEventArgs e)
        {
            modelUI.ActiveModel3DTransformation.TranslationX++;
        }

        private void DownTranslationXBtn_Click(object sender, RoutedEventArgs e)
        {
            modelUI.ActiveModel3DTransformation.TranslationX--;
        }

        private void UpTranslationYBtn_Click(object sender, RoutedEventArgs e)
        {
            modelUI.ActiveModel3DTransformation.TranslationY++;
        }

        private void DownTranslationYBtn_Click(object sender, RoutedEventArgs e)
        {
            modelUI.ActiveModel3DTransformation.TranslationY--;
        }

        private void DownTranslationZBtn_Click(object sender, RoutedEventArgs e)
        {
            modelUI.ActiveModel3DTransformation.TranslationZ--;
        }

        private void UpTranslationZBtn_Click(object sender, RoutedEventArgs e)
        {
            modelUI.ActiveModel3DTransformation.TranslationZ++;
        }

        private void HelixViewport3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(modelUI.GeometryModel3DHit == null)
            {
                modelUI.ActiveModel3D = modelUI.Basemodel3DGroup;
            }
            else
            {
                modelUI.ActiveModel3D = modelUI.GeometryModel3DHit;
            }
        }
    }
}
