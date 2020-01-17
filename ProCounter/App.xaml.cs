using System;
using System.Threading;
using System.Windows;
using System.Runtime.InteropServices;

namespace ProCounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string cls, string win);
        [DllImport("user32")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        static Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            const string mutexName = @"Global\ProCounter by Viktor";
            mutex = new Mutex(true, mutexName,
                out var isNewInstance);
            if (!isNewInstance)
            {
                ActivateOtherWindow();
                Shutdown();
            }
        }

        private static void ActivateOtherWindow()
        {
            var other = FindWindow(null, "ProCounter");
            if (other != IntPtr.Zero)
            {
                SetForegroundWindow(other);
            }
        }
    }
}
