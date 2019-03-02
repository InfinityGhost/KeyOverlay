using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace KeyOverlay.Controls
{
    /// <summary>
    /// Interaction logic for KeyCounter.xaml
    /// </summary>
    public partial class KeyCounter : UserControl, INotifyPropertyChanged
    {
        public KeyCounter(Panel panel, Configuration config)
        {
            InitializeComponent();
            SetPanel(panel);

            var timer = new System.Timers.Timer
            {
                AutoReset = true,
                Interval = 100,
            };
            timer.Elapsed += KPSTimer_Elapsed;

            var kpsUpdate = new System.Timers.Timer
            {
                AutoReset = true,
                Interval = 10,
            };
            kpsUpdate.Elapsed += (tmr, args) => KPS = KpsPerTick[CurrentTick];

            timer.Start();
            kpsUpdate.Start();

            Foreground = config.TextBrush;
        }

        private void Viewer_KeyChanged(object sender, bool e)
        {
            if (e)
            {
                KeyCount++;
                for (int i = 0; i < 10; i++)
                    KpsPerTick[i]++;
            }
        }

        #region Properties

        private double _kps, _keycount;

        public double KPS
        {
            private set
            {
                _kps = value;
                NotifyPropertyChanged();
            }
            get => _kps;
        }

        public double KeyCount
        {
            private set
            {
                _keycount = value;
                NotifyPropertyChanged();
            }
            get => _keycount;
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            KPS = 0;
            KeyCount = 0;
        }

        public void SetPanel(Panel panel)
        {
            foreach (var obj in panel.Children)
                if (obj is KeyStateViewer viewer)
                    viewer.KeyChanged += Viewer_KeyChanged;
        }

        public void AddKey(KeyStateViewer viewer)
        {
            viewer.KeyChanged += Viewer_KeyChanged;
        }

        #endregion

        #region KPS Counting

        private byte[] KpsPerTick = new byte[10];

        private byte _currenttick;
        private byte CurrentTick
        {
            set
            {
                if (value >= 10)
                    _currenttick = 0;
                else
                    _currenttick = value;
            }
            get => _currenttick;
        }

        private void KPSTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            KpsPerTick[CurrentTick] = 0;
            CurrentTick++;
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
