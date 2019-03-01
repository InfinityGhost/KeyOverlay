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

namespace KeyOverlay.Controls.Internal
{
    /// <summary>
    /// Interaction logic for AddKeyItem.xaml
    /// </summary>
    public partial class AddKeyItem : UserControl
    {
        public AddKeyItem()
        {
            InitializeComponent();
        }

        public static AddKeyItem Item = new AddKeyItem();
    }
}
