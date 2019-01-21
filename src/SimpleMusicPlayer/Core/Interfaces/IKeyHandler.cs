using System.Windows.Input;

namespace MusicPlayer.Core.Interfaces
{
    public interface IKeyHandler
    {
        bool HandleKeyDown(Key key);
    }
}