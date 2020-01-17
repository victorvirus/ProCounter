using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProCounter
{
    public class CounterViewModel : INotifyPropertyChanged
    {
        private CounterModel _counter;
        private string _label;
        public int[] FontSizes { get; set; } = new int[93];
        private bool _somethingChanged = true;
        private bool _runOnWinStartup;
        public bool UnsavedChanges { get; set; }
        private const string ProgramName = "ProCounter";
        private const string InfoMessage = "Right mouse click to configure";
        private const string RegisterStartupFolderPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string RegisterStartupApprovedFolderPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        private readonly string _settingsPath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments), ProgramName, "settings.dat");
        private readonly byte[] _isAppEnabledInWinStartupValue = { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // this value means that app runs on startup
        public event Action FormRefreshNeeded;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveCounterDataCommand { get; set; }
        public string IsStickedTooltipText { get; } = "You can drag and drop the counter while it is unchecked";
        public string DropdownTooltipText { get; } = "You can scroll while hovering the dropdown to see the results immediately";

        public CounterModel Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                OnPropertyChanged("Counter");
            }
        }
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }
 
        public bool RunOnWinStartup
        {
            get => _runOnWinStartup;
            set
            {
                _runOnWinStartup = value;
                OnPropertyChanged("RunOnWinStartup");
                UnsavedChanges = true;
            }
        }

        public CounterViewModel()
        {
            SaveCounterDataCommand = new Command(SaveCounterData, () => UnsavedChanges);
            for (int i = 0; i < 93; i++)
            {
                FontSizes[i] = i + 8;
            }

            LoadCounterData();

            if (_counter == null)
            {
                _counter = new CounterModel();
            }

            if (!_counter.SelectedDate.HasValue)
            {
                Label = InfoMessage;
            }

            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Counter_Tick);
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();
            UnsavedChanges = false;
            _counter.PropertyChanged += (x, y) => _somethingChanged = UnsavedChanges = true;
            RunOnWinStartup = CheckIfIsInStartup();
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void Counter_Tick(object o, EventArgs args)
        {
            if (!_counter.SelectedDate.HasValue)
            {
                if (_somethingChanged)
                {
                    FormRefreshNeeded?.Invoke();
                    _somethingChanged = false;
                }

                return;
            }

            if (!_counter.ShowYears && !_counter.ShowMonths && !_counter.ShowWeeks 
                && !_counter.ShowDays && !_counter.ShowHours && !_counter.ShowMinutes 
                && !_counter.ShowSeconds && !_counter.ShowMilliseconds)
            {
                Label = InfoMessage;
                FormRefreshNeeded?.Invoke();
                return;
            }

            TimeSpan dateToShow = _counter.SelectedDate.Value.Subtract(DateTime.Now);
            string labelToShow = "";

            if (_counter.ShowYears)
            {
                long years = dateToShow.Days / 365;
                dateToShow = dateToShow.Subtract(TimeSpan.FromDays(365 * years));
                labelToShow += CountTimeValue(years, "year", "years");
            }
            if (_counter.ShowMonths)
            {
                long months = dateToShow.Days / 30;
                dateToShow = dateToShow.Subtract(TimeSpan.FromDays(30 * months));
                labelToShow += CountTimeValue(months, "month", "months");
            }
            if (_counter.ShowWeeks)
            {
                long weeks = dateToShow.Days / 7;
                dateToShow = dateToShow.Subtract(TimeSpan.FromDays(7 * weeks));
                labelToShow += CountTimeValue(weeks, "week", "weeks");
            }
            if (_counter.ShowDays)
            {
                long days = dateToShow.Days;
                dateToShow = dateToShow.Subtract(TimeSpan.FromDays(days));
                labelToShow += CountTimeValue(days, "day", "days");
            }
            if (_counter.ShowHours)
            {
                long hours = (long) dateToShow.TotalHours;
                dateToShow = dateToShow.Subtract(TimeSpan.FromHours(hours));
                labelToShow += CountTimeValue(hours, "hour", "hours");
            }
            if (_counter.ShowMinutes)
            {
                long minutes = (long) dateToShow.TotalMinutes;
                dateToShow = dateToShow.Subtract(TimeSpan.FromMinutes(minutes));
                labelToShow += CountTimeValue(minutes, "minute", "minutes");
            }
            if (_counter.ShowSeconds)
            {
                long seconds = (long) dateToShow.TotalSeconds;
                dateToShow = dateToShow.Subtract(TimeSpan.FromSeconds(seconds));
                labelToShow += CountTimeValue(seconds, "second", "seconds");
            }
            if (_counter.ShowMilliseconds)
            {
                long milliseconds = (long) dateToShow.TotalMilliseconds;
                dateToShow = dateToShow.Subtract(TimeSpan.FromMilliseconds(milliseconds));
                labelToShow += CountTimeValue(milliseconds, "ms", "ms");
            }

            Label = labelToShow;

            if (_somethingChanged)
            {
                FormRefreshNeeded?.Invoke();
                _somethingChanged = false;
            }
        }

        private string CountTimeValue(long value, string singleForm, string pluralForm)
        {
            string formattedValue = value.ToString("N0");
            return (value > 0 ? (value > 1 ? formattedValue + " " + pluralForm : "1 " + singleForm) : "0 " + pluralForm) + Counter.Separator;
        }

        private void LoadCounterData()
        {
            if (!File.Exists(_settingsPath))
            {
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using FileStream fs = new FileStream(_settingsPath, FileMode.OpenOrCreate);
            _counter = (CounterModel)formatter.Deserialize(fs);
        }

        public void SaveCounterData(object sender)
        {
            _counter.PositionLeft = Application.Current?.MainWindow?.Left;
            _counter.PositionTop = Application.Current?.MainWindow?.Top;
            _counter.WindowHeight = Application.Current?.MainWindow?.Height;
            _counter.WindowWidth = Application.Current?.MainWindow?.Width;

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), ProgramName));

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(_settingsPath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _counter);
            }

            if (_runOnWinStartup)
            {
                SetToRunOnStartup();
            }
            else
            {
                SetNotToRunOnStartup();
            }

            UnsavedChanges = false;
        }

        private void SetToRunOnStartup()
        {
            var appPath = Process.GetCurrentProcess().MainModule?.FileName;

            if (appPath != null)
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegisterStartupFolderPath, true);
                key?.SetValue(ProgramName, appPath);

                Microsoft.Win32.RegistryKey approvedStartupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegisterStartupApprovedFolderPath, true);
                approvedStartupKey?.SetValue(ProgramName, _isAppEnabledInWinStartupValue);
            }
        }

        private void SetNotToRunOnStartup()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegisterStartupFolderPath, true);
            key?.DeleteValue(ProgramName, false);
        }

        private bool CheckIfIsInStartup()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegisterStartupFolderPath, true);
            if (key?.GetValue(ProgramName) == null)
            {
                return false;
            }

            Microsoft.Win32.RegistryKey approvedStartupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegisterStartupApprovedFolderPath, true);
            var keyValue = approvedStartupKey?.GetValue(ProgramName);

            if (keyValue == null)
            {
                return false;
            }

            if (keyValue is byte[] k)
            {
                return k.SequenceEqual(_isAppEnabledInWinStartupValue);
            }

            return false;
        }
    }
}

