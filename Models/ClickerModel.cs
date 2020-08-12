using AutoClicker.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Threading;
namespace AutoClicker.Model
{
    
    public class ClickerModel 
    {
        public DateTime ClickTime { get; set; }
        public Stopwatch TotalClicks { get; set; }
    }
}
