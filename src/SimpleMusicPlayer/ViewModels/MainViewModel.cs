using System.Linq;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;
using MusicPlayer.Core;
using MusicPlayer.Core.Interfaces;
using MusicPlayer.Core.Player;
using TinyIoC;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace MusicPlayer.ViewModels
{
    public class MainViewModel : ReactiveObject, IKeyHandler
    {
        public MainViewModel()
        {
            var container = TinyIoCContainer.Current;

            this.PlayerSettings = container.Resolve<PlayerSettings>().Update();

            this.PlayerEngine = container.Resolve<PlayerEngine>().Configure();

            this.PlayListsViewModel = new PlayListsViewModel();

            this.PlayControlInfoViewModel = new PlayControlInfoViewModel(this);

            this.ShowOnGitHubCmd = new DelegateCommand(this.ShowOnGitHub, () => true);
            ShowOnSettingCmd = ReactiveCommand.CreateFromTask(() =>ShowSetting());
            
        }
        public async System.Threading.Tasks.Task OpenArgsAsync()
        {
            await this.PlayListsViewModel.OpenFilesAsync(new System.Collections.Generic.List<string>(App.Args));
        }
        public PlayerEngine PlayerEngine { get; private set; }

        public PlayerSettings PlayerSettings { get; private set; }

        public CustomWindowPlacementSettings WindowPlacementSettings { get; set; }

        public PlayControlInfoViewModel PlayControlInfoViewModel { get; private set; }

        public PlayListsViewModel PlayListsViewModel { get; private set; }

        public ICommand ShowOnGitHubCmd { get; }
        public ICommand ShowOnSettingCmd { get; }
        public bool IsSettingsOpen { get; private set; }

        private void ShowOnGitHub()
        {
            System.Diagnostics.Process.Start("https://github.com/punker76/simple-music-player");
        }
        private async Task ShowSetting()
        {
            this.IsSettingsOpen = true;
            var view = new Views.SettingsView() { ViewModel = new SettingsViewModel(this.PlayerSettings) };
            view.Closing += (sender, args) => this.IsSettingsOpen = false;
            view.Show();
        }
        public void ShutDown()
        {
            foreach (var w in Application.Current.Windows.OfType<Window>())
            {
                w.Close();
            }
            this.PlayerSettings.Save();
            this.PlayerEngine.CleanUp();
            this.PlayListsViewModel.SavePlayList();
        }

        public bool HandleKeyDown(Key key)
        {
            if (this.PlayControlInfoViewModel.PlayControlViewModel.HandleKeyDown(key))
            {
                return true;
            }
            return false;
        }
    }
}