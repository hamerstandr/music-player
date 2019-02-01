using MusicPlayer.Core;
using MusicPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for SettingsControlView.xaml
    /// </summary>
    public partial class SettingsControlView : UserControl
    {
        public SettingsControlView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = sender as ComboBox;
            var i=c.SelectedItem as AccentColor;
            SettingsViewModel M = this.DataContext as SettingsViewModel;
            M.SelectedAccentColor = i;
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var c = sender as ComboBox;
            var i = c.SelectedItem as string;
            SettingsViewModel M = this.DataContext as SettingsViewModel;
            M.SelectedAppTheme = i;
        }
    }
}
