using KeyOverlay.Controls;
using KeyOverlay.Controls.FileManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace KeyOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Configuration.TryGetDefaultConfiguration(out var result))
                Config = result;
            else
                Config = new Configuration();
            RefreshKeys();
        }

        #region Properties

        private Configuration _cfg;
        public Configuration Config
        {
            set
            {
                _cfg = value;
                NotifyPropertyChanged();
            }
            get => _cfg;
        }

        #endregion

        #region Methods

        public void RefreshKeys()
        {
            ButtonsPanel.Children.Clear();
            Config.Keys.ForEach(key => DisplayKey(key));
        }

        public void AddKey(Key key)
        {
            if (!Config.Keys.Contains(key))
            {
                Config.Keys.Add(key);
                DisplayKey(key);
            }
        }

        public void RemoveKey(Key key)
        {
            if (Config.Keys.Contains(key))
            {
                var panelChildren = ButtonsPanel.Children.Cast<KeyStateViewer>();
                var keyViewer = panelChildren.Where(e => e.InputHook.Key == key).First();
                if (keyViewer != null)
                    ButtonsPanel.Children.Remove(keyViewer);
                Config.Keys.Remove(key);
            }
        }

        public void DisplayKey(Key key)
        {
            var hook = new InputHook(key, Config.PollRate);
            var viewer = new KeyStateViewer(hook, Config);
            viewer.RemoveKeyRequested += RemoveKeyRequested;
            ButtonsPanel.Children.Add(viewer);
        }

        private void RemoveKeyRequested(object sender, EventArgs e)
        {
            var key = sender as KeyStateViewer;
            RemoveKey(key.InputHook.Key);
        }

        #endregion

        #region Buttons & Configuration Settings

        private void AddKey_Click(object sender, RoutedEventArgs e)
        {
            if (KeyReader.Key != null && KeyReader.Key != 0)
                AddKey((Key)KeyReader.Key);
        }

        private void SettingTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            RefreshKeys();
        }

        #endregion

        #region Menu

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveDefaultConfig_Click(object sender, RoutedEventArgs e)
        {
            Config.Write(Configuration.DefaultPath);
        }

        private void SaveConfigAs_Click(object sender, RoutedEventArgs e)
        {
            if (FileDialog.Save(out string path))
                Config.Write(path);
        }

        private void LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            if (FileDialog.Open(out string path))
            {
                Config = Configuration.Read(path);
                RefreshKeys();
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
