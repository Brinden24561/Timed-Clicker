using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
namespace SortingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
 
        [DllImport("user32.dll", CharSet=CharSet.Auto, CallingConvention =CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy,
            uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENF_LEFTDOWN = 0x02;
        private const int MOUSEEVENF_LEFTUP = 0x04;
        //public ClickerView ViewModel { get; } will prob implement this in next version
        Stopwatch s1;
        DispatcherTimer t1;
        bool button1wasClicked = false;
        bool button2wasClicked = false;
        uint clickCount =0;
        public MainWindow()
        {
            InitializeComponent();
            MainWindow_OnLoad();
           // DataContext = new MainClickerView(new AutoClicker.Model.Clicker());
        }
        private void MainWindow_OnLoad()
        {
            s1 = new Stopwatch();
            t1 = new DispatcherTimer();
            t1.Tick += new EventHandler(dt_Tick);
            t1.Interval = new TimeSpan(0, 0, 1);
            DisplayCount.Text = String.Format($"{0}",clickCount);
            t1.Start();
            s1.Start();
        }
        public override string ToString()
        {
            return this.s1.Elapsed.ToString("{0:00}:{1:00}:{2:00}");
        }
        private void dt_Tick(object sender, EventArgs e)
        {
            TimeStamp.Text = s1.Elapsed.ToString();  
        }
        /// <summary>
        /// Clicks Button once  
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e"></param>
        private async void Single_Click(object sender, RoutedEventArgs e)
        {
            button1wasClicked = true;
            uint X = 0;
            uint y = 0;
            var rand = new Random();
            string update = "Single click ended";
            if(!s1.IsRunning && !t1.IsEnabled)
            {
                s1.Restart();
                clickCount = 0;
                t1.Start();
                s1.Start();
            }
            while(!button2wasClicked)
            {
                await Task.Delay(rand.Next(2000, 4000));
                mouse_event(MOUSEEVENF_LEFTDOWN | MOUSEEVENF_LEFTUP, X, y, 0, 0);
                ++clickCount;
                DisplayCount.Text = clickCount.ToString("0");
            }
            MessageBox.Show(update);
            button2wasClicked = false;
        }

        private void Double_Click(object sender, RoutedEventArgs e)
        {
            
        }/// <summary>
         /// Might have to change to int. Thinking of using this to constantly display the amount of time that passes
         /// This will have to update on property changes
         /// </summary>
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            button2wasClicked = true;
            if (button1wasClicked == true)
            {
                t1.Stop();
                s1.Stop();
            }
        }
    }
}
