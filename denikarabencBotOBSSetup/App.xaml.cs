using Common.Models;
using denikarabencBotOBSSetup.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace denikarabencBotOBSSetup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            BotLogger.Logger.Log(LoggingType.Info, "[Configuration] -> Starting Application");
            MainWindowViewModel vm = new MainWindowViewModel();
            MainWindow mv = new MainWindow(vm);
            mv.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            BotLogger.Logger.Log(LoggingType.Error, "[Configuration] -> Unexpected error happened", e.ExceptionObject as Exception);
        }
    }
}
