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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyOverlay.Controls
{
    /// <summary>
    /// Interaction logic for KeyReader.xaml
    /// </summary>
    public partial class KeyReader : UserControl
    {
        public KeyReader()
        {
            InitializeComponent();
        }

        public Key? Key { private set; get; }

        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Key = e.Key;
            await Task.Delay(1);
            (sender as TextBox).Text = $"{Key}";
        }
    }
}
