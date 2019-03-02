using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace KeyOverlay
{
    [XmlInclude(typeof(SolidColorBrush)), XmlInclude(typeof(MatrixTransform))]
    public class Configuration : INotifyPropertyChanged
    {
        public Configuration()
        {
            KeyDownBrush = Brushes.LightBlue;
            KeyUpBrush = Brushes.White;
            BackgroundBrush = Brushes.Green;
            PollRate = 10;
            Keys = new List<Key>();
        }

        #region Colors

        private Brush _downBrush, _upBrush, _backgroundBrush, _textBrush;

        public Brush KeyDownBrush
        {
            set
            {
                _downBrush = value;
                NotifyPropertyChanged();
            }
            get => _downBrush;
        }

        public Brush KeyUpBrush
        {
            set
            {
                _upBrush = value;
                NotifyPropertyChanged();
            }
            get => _upBrush;
        }

        public Brush BackgroundBrush
        {
            set
            {
                _backgroundBrush = value;
                NotifyPropertyChanged();
            }
            get => _backgroundBrush;
        }

        public Brush TextBrush
        {
            set
            {
                _textBrush = value;
                NotifyPropertyChanged();
            }
            get => _textBrush;
        }

        #endregion

        private List<Key> _keys;
        public List<Key> Keys
        {
            set
            {
                _keys = value;
                NotifyPropertyChanged();
            }
            get => _keys;
        }

        private int _poll;
        public int PollRate
        {
            set
            {
                _poll = value;
                NotifyPropertyChanged();
            }
            get => _poll;
        }

        private bool _hideOnPopout;
        public bool HideOnPopout
        {
            set
            {
                _hideOnPopout = value;
                NotifyPropertyChanged();
            }
            get => _hideOnPopout;
        }

        private bool _showKps;
        public bool ShowKPS
        {
            set
            {
                _showKps = value;
                NotifyPropertyChanged();
            }
            get => _showKps;
        }

        #region Serialization

        public static string DefaultPath = Directory.GetCurrentDirectory().Trim('/', '\\') + @"\default.xml";
        private static XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));

        /// <summary>
        /// Reads a KeyOverlay configuration and returns the configuration.
        /// </summary>
        /// <param name="path">The read path.</param>
        /// <returns>Configuration</returns>
        public static Configuration Read(string path)
        {
            using (var sr = new StreamReader(path))
                return (Configuration)Serializer.Deserialize(sr);
        }

        /// <summary>
        /// Writes the configuration to the specified path.
        /// </summary>
        /// <param name="path">The write path.</param>
        public void Write(string path)
        {
            using (TextWriter tw = new StreamWriter(path))
                Serializer.Serialize(tw, this);
        }

        public static bool TryGetDefaultConfiguration(out Configuration config)
        {
            if (File.Exists(DefaultPath))
            {
                try
                {
                    config = Read(DefaultPath);
                    return true;
                }
                catch
                {
                    config = null;
                    return false;
                }
            }
            else
            {
                config = null;
                return false;
            }

        }


        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string PropertyName = "")
        {
            if (PropertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
