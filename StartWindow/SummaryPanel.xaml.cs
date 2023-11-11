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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace StartWindow
{
    /// <summary>
    /// Logika interakcji dla klasy SummaryPanel.xaml
    /// </summary>
    public partial class SummaryPanel : UserControl
    {

        public delegate void OnTickEventHandler(object sender, int count, EventArgs e);
        public event OnTickEventHandler? OnTick;

        private int counter;

        DispatcherTimer timer = new DispatcherTimer();
        public SummaryPanel()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            counter = 3;
            timer.Start();
        }

        
        private void Timer_Tick(object? sender, EventArgs e)
        {
            --counter;
            if (OnTick != null)
                OnTick(this, counter, e);
            if( counter == -1 )
            {
                timer.Stop();
            }
        }
    }
}
