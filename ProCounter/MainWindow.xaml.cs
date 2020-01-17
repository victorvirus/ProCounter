using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;

namespace ProCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OptionsWindow _optionsWindow;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CounterViewModel();
            Graphics graphics = Graphics.FromImage(new Bitmap(1, 1));
            var vmContext = (CounterViewModel)DataContext;
            var counter = vmContext.Counter;
           
            vmContext.FormRefreshNeeded += () =>
            {
               var size = graphics.MeasureString(vmContext.Label, new Font(counter.FontFamily, counter.FontSize));
               this.Width = size.Width;
               this.Height = size.Height;
               this.MinWidth = size.Width;
               this.MinHeight = size.Height;
               this.MaxWidth = size.Width;
               this.MaxHeight = size.Height;
            };

            if (counter.PositionTop.HasValue && counter.PositionLeft.HasValue)
            {
                this.Top = counter.PositionTop.Value;
                this.Left = counter.PositionLeft.Value;
            }

            counter.PropertyChanged+= (x,y) =>
            {
                if (y.PropertyName == "IsSticked")
                {
                    if (x is CounterModel counterModel)
                    {
                        if (counterModel.IsSticked)
                        {
                            this.MouseDown -= Window_MouseDown;
                        }
                        else
                        {
                            this.MouseDown += Window_MouseDown;
                        }
                    }
                }
            };

            counter.IsSticked = counter.IsSticked; //fire the PropertyChanged event
        }

        private void ContextMenuItem1_Click(object sender, RoutedEventArgs e)
        {
            if (_optionsWindow == null)
            {
                _optionsWindow = new OptionsWindow()
                {
                    DataContext = this.DataContext,
                    Owner = this
                };
                _optionsWindow.Show();
            }
            else
            {
                _optionsWindow.Show();
            }
        }

        private void ContextMenuItem2_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((CounterViewModel)DataContext).SaveCounterData(this);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
                ((CounterViewModel)DataContext).UnsavedChanges = true;
            }
        }
    }
}
