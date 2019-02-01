using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using MusicPlayer.ViewModels;
using ReactiveUI;
using System;
using System.Windows;

namespace MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView /*: MetroWindow, IViewFor<SettingsViewModel>*/
    {
        public SettingsView()
        {
            InitializeComponent();
            //this.FocusedElement = this.CloseButton;

            this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext);

            //this.WhenActivated(d => this.WhenAnyValue(x => x.ViewModel).Subscribe(vm => vm.CloseEqualizerCommand.Subscribe(_ => Close())));
        }
        public SettingsViewModel ViewModel
        {
            get { return (SettingsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SettingsViewModel), typeof(SettingsView), new PropertyMetadata(null));

        //object IViewFor.ViewModel
        //{
        //    get { return ViewModel; }
        //    set { ViewModel = (SettingsViewModel)value; }
        //}
    }
}
