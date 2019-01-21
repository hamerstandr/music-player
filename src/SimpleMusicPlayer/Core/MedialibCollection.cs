using System.Collections.Generic;
using MusicPlayer.Core.Interfaces;

namespace MusicPlayer.Core
{
    public class MedialibCollection : QuickFillObservableCollection<IMediaFile>
    {
        public MedialibCollection(IEnumerable<IMediaFile> files)
            : base(files)
        {
        }
    }
}