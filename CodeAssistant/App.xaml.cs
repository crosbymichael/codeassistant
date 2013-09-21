/*****************************************/
/*           Code Assistant              */
/*****************************************/
/*   Copyright (c) 2012 Michael Crosby   */
/*****************************************/
/*      michael@crosbymichael.com        */
/*      http://crosbymichael.com         */
/*****************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows;
using CodeAssistant.Views;
using CodeAssistant.Resources;
using CodeAssistant.Factories;
using CodeAssistant.ViewModel;
using CodeAssistant.Services;
using ICSharpCode.AvalonEdit.Highlighting;
using MVVM_Async.ViewModel;

namespace CodeAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Keep reference to singletons
        Preferences preferences;
        ApplicationViewModel applicationViewModel;
        
        void OnStartup(object sender, StartupEventArgs e)
        {
            preferences = Preferences.Instance;
            var viewFactory = new EditorViewFactory();
            applicationViewModel = new ApplicationViewModel(viewFactory, Shutdown);
            DispatcherUnhandledException += applicationViewModel.UnhandledException;
            applicationViewModel.Start();
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            Janitor.CleanDirectory(preferences.TempDirectoryPath);
        }
    }
}
