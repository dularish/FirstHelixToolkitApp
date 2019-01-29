using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MyFirstHelixToolkitAppToPlayAround
{
    namespace MyCustomModulesInAmbigai
    {
        public class AbaqusInpFileParser
        {
            List<Part> _Parts = new List<Part>();
            List<Assembly> _Assemblies = new List<Assembly>();

            public List<Part> Parts { get => _Parts; private set => _Parts = value; }
            public List<Assembly> Assemblies { get => _Assemblies; }

            public AbaqusInpFileParser(string inputFilePath)
            {
                using (StreamReader reader = new StreamReader(inputFilePath))
                {
                    string line = "";

                    bool nodeStarted = false;
                    bool elementStarted = false;
                    Part currentPart = null;
                    Assembly currentAssembly = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (currentPart != null)
                        {
                            if (!line.StartsWith("*"))
                            {
                                if (nodeStarted)
                                {
                                    string[] delimitedString = line.Split(',').Select(s => s.Trim()).ToArray();

                                    if (delimitedString.All(s => string.IsNullOrEmpty(s)) || delimitedString.Length < 3 || delimitedString.Length > 4)
                                    {
                                        continue;
                                    }

                                    currentPart.Nodes.Add(new Node(Convert.ToInt32(delimitedString[0]), Convert.ToDouble(delimitedString[1]), Convert.ToDouble(delimitedString[2]), delimitedString.Length < 4 ? 0 : Convert.ToDouble(delimitedString[3])));
                                }
                                else if (elementStarted)
                                {
                                    string[] delimitedString = line.Split(',').Select(s => s.Trim()).ToArray();

                                    if (delimitedString.All(s => string.IsNullOrEmpty(s)) || delimitedString.Length < 2)
                                    {
                                        continue;
                                    }

                                    int elementId = Convert.ToInt32(delimitedString[0]);
                                    Element newElement = new Element(elementId);

                                    for (int i = 1; i < delimitedString.Length; i++)
                                    {
                                        int nodeId = Convert.ToInt32(delimitedString[i]);

                                        newElement.AddNode(currentPart.Nodes.Where(s => s.NodeNum == nodeId).First());

                                    }

                                    currentPart.Elements.Add(newElement);
                                }
                            }
                            else
                            {
                                if (line.StartsWith("*Element"))
                                {
                                    nodeStarted = false;
                                    elementStarted = true;
                                }
                                else if (line.StartsWith("*Node"))
                                {
                                    elementStarted = false;
                                    nodeStarted = true;
                                }
                                else if (line.StartsWith("*End Part"))
                                {
                                    currentPart = null;
                                    nodeStarted = false;
                                    elementStarted = false;
                                }
                                else
                                {
                                    nodeStarted = false;
                                    elementStarted = false;
                                }
                            }
                        }

                        if(currentAssembly != null)
                        {
                            if (line.StartsWith("*Instance"))
                            {
                                string instanceName = line.Split(',').Select(s => s.Trim()).Where(s => s.StartsWith("name=")).First().Split('=').Last();
                                string partName = line.Split(',').Select(s => s.Trim()).Where(s => s.StartsWith("part=")).First().Split('=').Last();

                                line = reader.ReadLine();
                                if(line != null)
                                {
                                    double xOffset = Convert.ToDouble(line.Split(',').Select(s => s.Trim()).ElementAt(0));
                                    double yOffset = Convert.ToDouble(line.Split(',').Select(s => s.Trim()).ElementAt(1));
                                    double zOffset = Convert.ToDouble(line.Split(',').Select(s => s.Trim()).ElementAt(2));

                                    AssemblyInstance instance = new AssemblyInstance(partName, instanceName, xOffset, yOffset, zOffset);
                                    currentAssembly.Instances.Add(instance);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if(line.StartsWith("*End Assembly"))
                            {
                                currentAssembly = null;
                            }
                        }

                        if (line.StartsWith("*Part"))
                        {
                            string partName = line.Split(',').Select(s => s.Trim()).Where(s => s.StartsWith("name=")).First().Split('=').Last();
                            Part newPart = new Part(partName);
                            Parts.Add(newPart);
                            currentPart = newPart;
                        }

                        if (line.StartsWith("*Assembly"))
                        {
                            string assemblyName = line.Split(',').Select(s => s.Trim()).Where(s => s.StartsWith("name=")).First().Split('=').Last();
                            Assembly assembly = new Assembly(assemblyName);
                            Assemblies.Add(assembly);
                            currentAssembly = assembly;
                        }
                    }
                }

                foreach (Part part in Parts)
                {
                    List<Tuple<Point3D, Point3D, Point3D>> listOfTriangles = new List<Tuple<Point3D, Point3D, Point3D>>();
                    
                    foreach (Element element in part.Elements)
                    {
                        if (element.Nodes.Count == 4)
                        {
                            listOfTriangles.Add(new Tuple<Point3D, Point3D, Point3D>(new Point3D(element.Nodes[0].XCoord, element.Nodes[0].YCoord, element.Nodes[0].ZCoord), new Point3D(element.Nodes[1].XCoord, element.Nodes[1].YCoord, element.Nodes[1].ZCoord), new Point3D(element.Nodes[2].XCoord, element.Nodes[2].YCoord, element.Nodes[2].ZCoord)));
                            listOfTriangles.Add(new Tuple<Point3D, Point3D, Point3D>(new Point3D(element.Nodes[2].XCoord, element.Nodes[2].YCoord, element.Nodes[2].ZCoord), new Point3D(element.Nodes[3].XCoord, element.Nodes[3].YCoord, element.Nodes[3].ZCoord), new Point3D(element.Nodes[0].XCoord, element.Nodes[0].YCoord, element.Nodes[0].ZCoord)));
                        }
                    }
                    part.Triangles = listOfTriangles;
                }
            }

            public List<Tuple<Point3D, Point3D, Point3D>> GetTrianglesForFirstPart()
            {
                List<Tuple<Point3D, Point3D, Point3D>> listOfTriangles = new List<Tuple<Point3D, Point3D, Point3D>>();

                foreach (Part part in Parts.Where(s => Parts.IndexOf(s) == 1))
                {
                    foreach (Element element in part.Elements)
                    {
                        if (element.Nodes.Count == 4)
                        {
                            listOfTriangles.Add(new Tuple<Point3D, Point3D, Point3D>(new Point3D(element.Nodes[0].XCoord, element.Nodes[0].YCoord, element.Nodes[0].ZCoord), new Point3D(element.Nodes[1].XCoord, element.Nodes[1].YCoord, element.Nodes[1].ZCoord), new Point3D(element.Nodes[2].XCoord, element.Nodes[2].YCoord, element.Nodes[2].ZCoord)));
                            listOfTriangles.Add(new Tuple<Point3D, Point3D, Point3D>(new Point3D(element.Nodes[2].XCoord, element.Nodes[2].YCoord, element.Nodes[2].ZCoord), new Point3D(element.Nodes[3].XCoord, element.Nodes[3].YCoord, element.Nodes[3].ZCoord), new Point3D(element.Nodes[0].XCoord, element.Nodes[0].YCoord, element.Nodes[0].ZCoord)));
                        }
                    }
                }

                

                return listOfTriangles;
            }
        }

        public class Assembly
        {
            public string AssemblyName { get; private set; }
            public List<AssemblyInstance> Instances { get => _Instances; private set => _Instances = value; }

            private List<AssemblyInstance> _Instances = new List<AssemblyInstance>();

            public Assembly(string assemblyName)
            {
                AssemblyName = assemblyName;
            }
        }

        public class AssemblyInstance
        {
            public string PartName { get; private set; }

            public string InstanceName { get; private set; }

            public double XOffset { get; private set; }
            public double YOffset { get; private set; }
            public double ZOffset { get; private set; }

            public AssemblyInstance(string partName, string instanceName, double xOffset, double yOffset, double zOffset)
            {
                PartName = partName;
                InstanceName = instanceName;
                XOffset = xOffset;
                YOffset = yOffset;
                ZOffset = zOffset;
            }
        }

        public class Part
        {
            public Part(string partName)
            {
                PartName = partName;
            }
            public string PartName;
            public List<Node> Nodes = new List<Node>();
            public List<Element> Elements = new List<Element>();
            public List<Tuple<Point3D, Point3D, Point3D>> Triangles = new List<Tuple<Point3D, Point3D, Point3D>>();

        }

        public class Node
        {
            int _NodeNum;
            double _XCoord;
            double _YCoord;
            double _ZCoord;

            public Node(int nodeNumber, double xCoord, double yCoord, double zCoord)
            {
                _NodeNum = nodeNumber;
                XCoord = xCoord;
                YCoord = yCoord;
                ZCoord = zCoord;
            }

            public int NodeNum { get => _NodeNum; }
            public double XCoord { get => _XCoord; private set => _XCoord = value; }
            public double YCoord { get => _YCoord; private set => _YCoord = value; }
            public double ZCoord { get => _ZCoord; private set => _ZCoord = value; }
        }

        public class Element
        {
            int _ElementNum;
            List<Node> _Nodes = new List<Node>();

            public Element(int elementNum)
            {
                _ElementNum = elementNum;
            }

            public List<Node> Nodes { get => _Nodes; private set => _Nodes = value; }

            public void AddNode(Node node)
            {
                Nodes.Add(node);
            }
        }
    }
}
