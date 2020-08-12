using System;
using System.Windows.Input;
using System.Diagnostics;
using AutoClicker.Model;
using AutoClicker.Contracts;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;

namespace AutoClicker.ViewModel
{
    public class MainClickerView :IMainClickView
    {
        private  IDbConnection db;
        private ClickerModel _cm;
        IConfiguration config { get;  set; }
        public MainClickerView()
        {

             InitConnString();
            this.db = new SqlConnection(config.GetSection("ConnectionString")["DefaultConnection"]) ;
            _cm = new ClickerModel();
        }
       
        private void InitConnString()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            jsonPath = jsonPath.Remove(23);
            jsonPath += "\\ConnString\\appsettings.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonPath, false, reloadOnChange: true);
            config = builder.Build();
            
        }
        public void InsertEntry(Stopwatch ClickTime, int ClickNumber)
        {
            this.db.Execute(@"INSERT INTO ClickTimeSess.ClickWithTime (ClickTime, ClickNumber) VALUES (@ClickTime, @ClickNumber)", new { ClickTime = ClickTime.Elapsed, ClickNumber = ClickNumber });
        }
    }
}
