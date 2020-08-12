using System;
using System.Diagnostics;

namespace AutoClicker.Contracts
{
    interface IMainClickView
    {

        void InsertEntry(Stopwatch time, int clickNumber);
        
    }
}