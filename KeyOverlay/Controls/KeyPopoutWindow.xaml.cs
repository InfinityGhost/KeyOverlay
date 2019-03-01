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
using System.Windows.Shapes;

namespace KeyOverlay.Controls
{
    /// <summary>
    /// Interaction logic for KeyPopoutWindow.xaml
    /// </summary>
    public partial class KeyPopoutWindow : Window
    {
        public KeyPopoutWindow(FrameworkElement element)
        {
            InitializeComponent();
            this.Content = element;
        }

        public KeyPopoutWindow(FrameworkElement element, Configuration config) : this(element)
        {
            this.Background = config.BackgroundBrush;
            config.PropertyChanged += (s, e) => this.Background = config.BackgroundBrush;
        }

        public event RoutedEventHandler AddKeyRequested;

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddKey_Click(object sender, RoutedEventArgs e)
        {
            AddKeyRequested?.Invoke(this, e);
        }
    }
}
