using System.Windows;
using ControlzEx.Standard;

namespace MusicPlayer.Core.Interfaces
{
    public interface IWindowSetting
    {
        WINDOWPLACEMENT Placement { get; set; }
        DpiScale? DpiScale { get; set; }
    }
}