using Common.Models;
using denikarabencBot.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace denikarabencBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; ;
            BotLogger.Logger.Log(LoggingType.Info, "Starting Application");
            MainWindowViewModel vm = new MainWindowViewModel();
            MainWindow mv = new MainWindow(vm);
            mv.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Error, "Application crashed! Unexpected error happened", e.ExceptionObject as Exception);
        }
    }
}
