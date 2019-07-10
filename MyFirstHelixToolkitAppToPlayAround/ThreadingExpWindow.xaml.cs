using System;
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

namespace MyFirstHelixToolkitAppToPlayAround
{
    /// <summary>
    /// Interaction logic for ThreadingExpWindow.xaml
    /// </summary>
    public partial class ThreadingExpWindow : Window
    {
        private ThreadExecuter _threadExecuter = new ThreadExecuter();
        public ThreadingExpWindow()
        {
            InitializeComponent();

            _threadExecuter.ThreadProgressReportEvent += _threadExecuter_ThreadProgressReportEvent;
        }

        private void _threadExecuter_ThreadProgressReportEvent(object sender, ThreadProgressReportEventArgs e)
        {
            var actionToUpdateProgress = new Action(() =>
            {
                _progressBar.Value = e.ProgressValue;
                _progressBarTextBlock.Text = e.Message;
            });
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(actionToUpdateProgress);   
            }
            else
            {
                actionToUpdateProgress.Invoke();
            }
        }

        private void _parameterlessThreadStart_Click(object sender, RoutedEventArgs e)
        {
            _threadExecuter.ParameterlessThreadStart();
            _progressBar.Foreground = Brushes.Green;
        }

        private void _parameterlessThreadAbort_Click(object sender, RoutedEventArgs e)
        {
            _threadExecuter.ParameterlessThreadAbort();
            _progressBar.Foreground = Brushes.Red;
        }

        private void _parameterlessThreadInterrupt_Click(object sender, RoutedEventArgs e)
        {
            _threadExecuter.ParameterlessThreadInterrupt();
        }

        private void _parameterizedThreadStart_Click(object sender, RoutedEventArgs e)
        {
            _threadExecuter.ParameterizedThreadStart((object)10);
            _progressBar.Foreground = Brushes.DarkGreen;
        }

        private void _parameterizedThreadAbort_Click(object sender, RoutedEventArgs e)
        {
            _threadExecuter.ParameterizedThreadAbort();
            _progressBar.Foreground = Brushes.DarkRed;
        }

        private void _parameterizedThreadInterrupt_Click(object sender, RoutedEventArgs e)
        {
            _threadExecuter.ParameterizedThreadInterrupt();
        }
    }
}
