using System;
using System.Threading;

namespace MyFirstHelixToolkitAppToPlayAround
{
    internal class ThreadExecuter
    {
        private Thread _parameterlessThread;
        private Thread _parameterizedThread;
        private void parameterlessRun()
        {
            double value = 0.0;
            ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(value, "Parameterless thread started"));
            while(value <= 100)
            {
                value += 1;
                Thread.Sleep(100);
                ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(value, "Parameterless thread running"));
            }

            ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(value, "Parameterless thread ended"));
        }

        public event EventHandler<ThreadProgressReportEventArgs> ThreadProgressReportEvent;
        public ThreadExecuter()
        {
            _parameterlessThread = new Thread(new ThreadStart(parameterlessRun));
            _parameterizedThread = new Thread(new ParameterizedThreadStart(parameterizedRun));
        }

        private void parameterizedRun(object obj)
        {
            double value = 0.0;
            ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(value, "Parameterized thread started with parameter : " + obj.ToString()));
            while (value <= 100)
            {
                value += 1;
                Thread.Sleep(100);
                ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(value, "Parameterized thread running with parameter : " + obj.ToString()));
            }

            ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(value, "Parameterized thread ended"));
        }

        internal void ParameterlessThreadStart()
        {
            _parameterlessThread.Start();
        }

        internal void ParameterlessThreadAbort()
        {
            _parameterlessThread.Abort();
            //_parameterlessThread.Join();
            ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(0, "Parameterless thread abort successful from ThreadExecuter"));
        }

        internal void ParameterizedThreadAbort()
        {
            _parameterizedThread.Abort();
        }

        internal void ParameterizedThreadStart(object v)
        {
            _parameterizedThread.Start(v);
        }

        internal void ParameterizedThreadInterrupt()
        {
            _parameterizedThread.Interrupt();
        }

        internal void ParameterlessThreadInterrupt()
        {
            _parameterlessThread.Interrupt();
            ThreadProgressReportEvent?.Invoke(this, new ThreadProgressReportEventArgs(0, "Parameterless thread interrupt successful from ThreadExecuter"));
        }
    }

    public class ThreadProgressReportEventArgs : EventArgs
    {
        private double _ProgressValue;
        private string _Message;

        public ThreadProgressReportEventArgs(double progressValue, string message)
        {
            _ProgressValue = progressValue;
            _Message = message;
        }

        public double ProgressValue { get => _ProgressValue; }
        public string Message { get => _Message; }
    }
}