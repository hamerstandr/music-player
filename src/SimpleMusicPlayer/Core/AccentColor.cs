using Newtonsoft.Json;
using System.Windows.Media;

namespace MusicPlayer.Core
{
    public class AccentColor
    {
        public string Name { get; set; }
        public string TranslatedName { get;  set; }
        [JsonIgnore]
        public Brush ColorBrush {
            get
            {
                // in WPF you can use a BrushConverter
                SolidColorBrush Brush = (SolidColorBrush)new BrushConverter().ConvertFromString(Name);
                return Brush; }
        }
    }
}