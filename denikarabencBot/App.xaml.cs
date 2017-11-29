﻿using denikarabencBot.ViewModels;
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
            BotLogger.Logger.Log("Starting Application");
            MainWindowViewModel vm = new MainWindowViewModel();
            MainWindow mv = new MainWindow(vm);
            mv.Show();
        }
    }
}
