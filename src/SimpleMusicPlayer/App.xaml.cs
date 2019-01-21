using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Windows;
using Microsoft.Shell;
using ReactiveUI;
using MusicPlayer.Core;
using MusicPlayer.Core.Player;
using MusicPlayer.ViewModels;
using MusicPlayer.Views;
using TinyIoC;
using MahApps.Metro;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        public App()
        {
            TinyIoCContainer.Current.Register<AppHelper>().AsSingleton();
            TinyIoCContainer.Current.Register<CoverManager>().AsSingleton();
            TinyIoCContainer.Current.Register<PlayerSettings>().AsSingleton();
            TinyIoCContainer.Current.Register<PlayerEngine>().AsSingleton();
            TinyIoCContainer.Current.Register<IReactiveObject, MedialibViewModel>();
            TinyIoCContainer.Current.Register<IReactiveObject, MainViewModel>();

            var appHelper = TinyIoCContainer.Current.Resolve<AppHelper>();
            appHelper.ConfigureApp(this, Assembly.GetExecutingAssembly().GetName().Name);

            // Enable Multi-JIT startup
            var profileRoot = Path.Combine(appHelper.ApplicationDataPath(), "ProfileOptimization");
            Directory.CreateDirectory(profileRoot);
            // Define the folder where to save the profile files
            ProfileOptimization.SetProfileRoot(profileRoot);
            // Start profiling and save it in Startup.profile
            ProfileOptimization.StartProfile("Startup.profile");
        }
        public static string[] Args { get; internal set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            App.Args = e.Args;
            // get the current app style (theme and accent) from the application
            // you can then use the current theme and custom accent instead set a new theme
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            PlayerSettings settings;
            settings=PlayerSettingsExtensions.Load();
            // now set the Green accent and dark theme
            if(settings.AccentColor!=null)
                ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(settings.AccentColor.Name),
                                        ThemeManager.GetAppTheme(settings.AppThemes)); // or appStyle.Item1
            else
            {
                // set the Red accent and dark theme only to the current window
                ThemeManager.ChangeAppStyle(this,
                                            ThemeManager.GetAccent("Green"),
                                            ThemeManager.GetAppTheme("BaseDark"));
            }
            //string[] arg1 = { "D:\\hamed\\Music\\audios\\AUD-20161112-WA0029.mp3" };
            //App.Args = arg1;
            MainWindow = TinyIoCContainer.Current.Resolve<MainWindow>();
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            TinyIoCContainer.Current.Resolve<AppHelper>().OnExitApp(this);
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                WindowExtensions.Unminimize(this.MainWindow);
            }
            else
            {
                WindowExtensions.ShowAndActivate(this.MainWindow);
            }
            return this.ProcessCommandLineArgs(this.MainWindow as MusicPlayer.Views.MainWindow, args);
        }

        private bool ProcessCommandLineArgs(MusicPlayer.Views.MainWindow window, IEnumerable<string> args)
        {
            if (window != null)
            {
                var vm = window.DataContext as MusicPlayer.ViewModels.MainViewModel;
                if (vm != null)
                {
                    vm.PlayListsViewModel.CommandLineArgs = new ReactiveList<string>(args);
                }
            }
            return true;
        }
    }
}