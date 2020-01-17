using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProCounter
{
    [Serializable]
    public class CounterModel : INotifyPropertyChanged
    {
        private DateTime? _selectedDate;
        private bool _showYears;
        private bool _showMonths;
        private bool _showWeeks;
        private bool _showDays;
        private bool _showHours;
        private bool _showMinutes;
        private bool _showSeconds;
        private bool _showMilliseconds;

        private double? _positionLeft;
        private double? _positionTop;
        private double? _windowWidth = 1230;
        private double? _windowHeight = 100;
        private int _fontSize = 30;
        private string _fontFamily = "Jokerman";
        private string _fontColor = "#FFA9A9A9";
        private string _separator = "\n";
        private bool _dropShadow = true;
        private bool _isSticked;

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }
        public bool ShowYears
        {
            get => _showYears;
            set
            {
                _showYears = value;
                OnPropertyChanged("ShowYears");
            }
        }
        public bool ShowMonths
        {
            get => _showMonths;
            set
            {
                _showMonths = value;
                OnPropertyChanged("ShowMonths");
            }
        }
        public bool ShowWeeks
        {
            get => _showWeeks;
            set
            {
                _showWeeks = value;
                OnPropertyChanged("ShowWeeks");
            }
        }
        public bool ShowDays
        {
            get => _showDays;
            set
            {
                _showDays = value;
                OnPropertyChanged("ShowDays");
            }
        }
        public bool ShowHours
        {
            get => _showHours;
            set
            {
                _showHours = value;
                OnPropertyChanged("ShowHours");
            }
        }
        public bool ShowMinutes
        {
            get => _showMinutes;
            set
            {
                _showMinutes = value;
                OnPropertyChanged("ShowMinutes");
            }
        }
        public bool ShowSeconds
        {
            get => _showSeconds;
            set
            {
                _showSeconds = value;
                OnPropertyChanged("ShowSeconds");
            }
        }
        public bool ShowMilliseconds
        {
            get => _showMilliseconds;
            set
            {
                _showMilliseconds = value;
                OnPropertyChanged("ShowMilliseconds");
            }
        }
        public double? PositionLeft
        {
            get => _positionLeft;
            set
            {
                _positionLeft = value;
                OnPropertyChanged("PositionLeft");
            }
        }
        public double? PositionTop
        {
            get => _positionTop;
            set
            {
                _positionTop = value;
                OnPropertyChanged("PositionTop");
            }
        }
        public double? WindowWidth
        {
            get => _windowWidth;
            set
            {
                _windowWidth = value;
                OnPropertyChanged("WindowWidth");
            }
        }
        public double? WindowHeight
        {
            get => _windowHeight;
            set
            {
                _windowHeight = value;
                OnPropertyChanged("WindowHeight");
            }
        }
        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }
        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                OnPropertyChanged("FontFamily");
            }
        }
        public string FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;
                OnPropertyChanged("FontColor");
            }
        }
        public string Separator
        {
            get => _separator;
            set
            {
                _separator = value;
                OnPropertyChanged("Separator");
            }
        }
        public bool DropShadow
        {
            get => _dropShadow;
            set
            {
                _dropShadow = value;
                OnPropertyChanged("DropShadow");
            }
        }
        public bool IsSticked
        {
            get => _isSticked;
            set
            {
                _isSticked = value;
                OnPropertyChanged("IsSticked");
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}