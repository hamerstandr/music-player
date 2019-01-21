using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using MusicPlayer.FMODStudio;
using MusicPlayer.Core.Player;

namespace MusicPlayer.ViewModels
{
    public class Settings : ReactiveObject
    {
        private FMOD.System fmodSystem;
        private PlayerSettings playerSettings;

        private Settings(FMOD.System system, PlayerSettings settings)
        {
            this.playerSettings = settings;
            this.Name = "DefaultSettings";
            this.fmodSystem = system;
            this.Initializied = false;
            this.IsEnabled = settings.PlayerEngine.EqualizerSettings == null || settings.PlayerEngine.EqualizerSettings.IsEnabled;

            this.WhenAnyValue(x => x.IsEnabled)
                .Subscribe(enabled =>
                {
                    if (!Initializied || enabled)
                    {
                        Init(fmodSystem);
                    }
                    else if (Initializied)
                    {
                        SaveSettingsSettings();
                        DeInit(fmodSystem);
                    }
                });
        }
        public void CleanUp()
        {
            this.DeInit(this.fmodSystem);
            this.fmodSystem = null;
            this.playerSettings = null;
        }

        public void SaveSettingsSettings()
        {
            if (this.playerSettings != null)
            {
                playerSettings.Save();
            }
        }
        public static Settings GetEqualizer(FMOD.System system, PlayerSettings settings)
        {
            var eq = new Settings(system, settings);
            eq.Initializied = true;
            return eq;
        }
        private void Init(FMOD.System system, bool setToDefaultValues = false)
        {
            system.lockDSP().ERRCHECK();

            var gainValues = !setToDefaultValues && this.playerSettings.PlayerEngine.EqualizerSettings != null
                ? this.playerSettings.PlayerEngine.EqualizerSettings.GainValues
                : null;

            system.unlockDSP().ERRCHECK();
            system.update().ERRCHECK();
        }
        private void DeInit(FMOD.System system)
        {
            system.lockDSP().ERRCHECK();


            system.unlockDSP().ERRCHECK();
            system.update().ERRCHECK();
        }
        private bool initializied;

        public bool Initializied
        {
            get { return this.initializied; }
            set { this.RaiseAndSetIfChanged(ref initializied, value); }
        }
        private bool isEnabled = true;

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.RaiseAndSetIfChanged(ref isEnabled, value); }
        }

        public string Name { get; private set; }

        public void SetToDefault()
        {
            this.DeInit(this.fmodSystem);
            this.Init(this.fmodSystem, true);
        }
    }
}