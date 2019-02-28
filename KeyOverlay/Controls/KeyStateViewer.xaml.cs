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
    /// Interaction logic for KeyStateViewer.xaml
    /// </summary>
    public partial class KeyStateViewer : UserControl, INotifyPropertyChanged
    {
        public KeyStateViewer(InputHook input)
        {
            InitializeComponent();
            if (input != null)
                InputHook = input;
            else
                throw new ArgumentNullException();

            this.Loaded += (sender, e) => InputHook.Hook();
            InputHook.StateChanged += UpdateKeyState;

            // Stops hook when application closes, avoids null reference exceptions.
            Application.Current.MainWindow.Closing += (sender, args) => Unhook();
            TextBlock.Text = $"{InputHook.Key}";
        }

        public KeyStateViewer(InputHook input, Configuration config) : this(input)
        {
            KeyDownBrush = config.KeyDownBrush;
            KeyUpBrush = config.KeyUpBrush;
        }

        #region Properties & Events

        public event EventHandler RemoveKeyRequested;

        public InputHook InputHook { private set; get; }

        public Brush KeyDownBrush { set; get; } = Brushes.LightBlue;
        public Brush KeyUpBrush { set; get; } = Brushes.White;

        private Brush _brush;
        public Brush CurrentBrush
        {
            private set
            {
                _brush = value;
                NotifyPropertyChanged();
            }
            get => _brush;
        }

        #endregion

        #region Public Methods

        public void UpdateKeyState(object inputhook, bool isDown)
        {
            if (isDown)
                CurrentBrush = KeyDownBrush;
            else
                CurrentBrush = KeyUpBrush;
        }

        public void Unhook()
        {
            InputHook.Unhook();
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

        private void RemoveKey_Click(object sender, RoutedEventArgs e)
        {
            RemoveKeyRequested?.Invoke(this, e);
        }
    }
}
