using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AutoClicker.Contracts;
using AutoClicker.ViewModel;
using Microsoft.Extensions.Configuration;
using AutoClicker.Model;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace SortingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //clicking features
        [DllImport("user32.dll", CharSet=CharSet.Auto, CallingConvention =CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy,
            uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENF_LEFTDOWN = 0x02;
        private const int MOUSEEVENF_LEFTUP = 0x04;
      
        //responsible for detecting keyboard input
        public static RoutedCommand MyCommand = new RoutedCommand();
      
        private IMainClickView _view { get; set; }
        private ClickerModel _cm { get; set; }
        private Stopwatch s1 { get; set; }
        private DispatcherTimer t1 { get; set; }
        private bool button1wasClicked = false;
        private bool button2wasClicked = false;
        private int clickCount { get; set; }

        public MainWindow()
        {
            
            InitializeComponent();
            MainWindow_OnLoad();
            _view = new MainClickerView();
        }
        /// <summary>
        /// Init Stopwatch and EventHandler for timer view component 
        /// </summary>
        private void MainWindow_OnLoad()
        {
            s1= new Stopwatch();
            t1 = new DispatcherTimer();
            t1.Tick += new EventHandler(dt_Tick);
            t1.Interval = new TimeSpan(0, 0, 1);
            DisplayCount.Text = String.Format($"{clickCount}");
           
        }
       
        //public override string ToString()
        //{
        //    return this.s1.Elapsed.ToString("{0:00}:{1:00}:{2:00}");
        //}
        private void dt_Tick(object sender, EventArgs e)
        {
            TimeStamp.Text = s1.Elapsed.ToString();  
        }
        /// <summary>
        /// Simulates left clicking, upon click timer starts.
        /// Method then waits for Stop_Click method to be invoked by click or by key+modifier
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e"></param>
        private async void Single_Click(object sender, RoutedEventArgs e)
        {
            //ctrl+key = exit
            MyCommand.InputGestures.Add(new KeyGesture(Key.X, ModifierKeys.Control));
            MyCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(MyCommand, Stop_Click));
            
            button1wasClicked = true;
            
            uint X = 0;
            uint Y = 0;
            var rand = new Random();
            string update = "Single click ended";

            //invoked if stop button is clicked, resets everything
            if (!s1.IsRunning && !t1.IsEnabled)
            {
                clickCount = 0;
                s1.Restart();
                t1.Start();
                s1.Start();
            }
            while (!button2wasClicked)
            {
                await Task.Delay(rand.Next(2000, 4000));
                mouse_event(MOUSEEVENF_LEFTDOWN | MOUSEEVENF_LEFTUP, X, Y, 0, 0);
                ++clickCount;
                DisplayCount.Text = clickCount.ToString("0");
            }
            button2wasClicked = false;
            MessageBox.Show(update);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            //deactivate left click
            button2wasClicked = true;
            _view.InsertEntry(s1, clickCount);
            if (button1wasClicked)
            {
                //reset timer and reset left click buton
                t1.Stop();
                s1.Stop();
                button1wasClicked = false;
            }
            else
            {
                MessageBox.Show("Must start mouse click to stop button");
                button2wasClicked = false;
            }
        }
    }
}
