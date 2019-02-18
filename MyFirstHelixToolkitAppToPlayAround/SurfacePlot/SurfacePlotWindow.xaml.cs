﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyFirstHelixToolkitAppToPlayAround.SurfacePlot
{
    /// <summary>
    /// Interaction logic for SurfacePlotWindow.xaml
    /// </summary>
    public partial class SurfacePlotWindow : Window
    {
        public SurfacePlotWindow()
        {
            InitializeComponent();

            this.DataContext = new SurfacePlotViewModel();
        }
    }
}