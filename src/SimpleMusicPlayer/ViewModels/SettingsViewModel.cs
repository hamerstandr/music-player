using MahApps.Metro;
using MusicPlayer.Core;
using MusicPlayer.Core.Player;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Windows;

namespace MusicPlayer.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private ObservableCollection<string> _appThemes;

        public SettingsViewModel(PlayerSettings Settings)
        {

            SetToDefaultCommand = ReactiveCommand.Create(
                () => Settings.Save(),
                this.WhenAnyValue(x => x.Settings.IsEnabled));

            CloseEqualizerCommand = ReactiveCommand.Create(
                () => Settings.Save());
            Refresh();
        }

        public PlayerSettings Settings { get; private set; }

        public ReactiveCommand<Unit, Unit> SetToDefaultCommand { get; protected set; }

        public ReactiveCommand<Unit, Unit> CloseEqualizerCommand { get; protected set; }
        ObservableCollection<AccentColor> AccentColors { get; set; }
        public AccentColor SelectedAccentColor { get => Settings.AccentColor;
            set
            {
                Settings.AccentColor = value;
                ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(value.Name),
                                        ThemeManager.GetAppTheme("BaseLight")); // or appStyle.Item1
            }
        }
        public ObservableCollection<string> AppThemes
        {
            get
            {
                return _appThemes;
            }
        }
        public string SelectedAppTheme
        {
            get => Settings.AppThemes;
            set => Settings.AppThemes = value;
        }
        public void Refresh()
        {
            AccentColors = new ObservableCollection<AccentColor>();
            _appThemes = new ObservableCollection<string>();
            foreach (var t in ThemeManager.Accents.Select(a => new AccentColor { Name = a.Name }).OrderBy(x => x.TranslatedName))
            {
                AccentColors.Add(t);
            }
            _appThemes.Add("BaseDark");
            _appThemes.Add("BaseLight");
        }
    }
}
