﻿using System;
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
        public KeyCounter(Panel panel)
        {
            InitializeComponent();
            SetPanel(panel);
        }

        private void Viewer_KeyChanged(object sender, bool e)
        {
            if (e)
            {
                KeyCount++;
                // Add KPS math
            }
        }

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