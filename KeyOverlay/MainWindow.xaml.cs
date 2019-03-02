using KeyOverlay.Controls;
using KeyOverlay.Controls.FileManagement;
using KeyOverlay.Controls.Internal;
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

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
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

        private bool _addingkey = false;
        public bool AddingKey
        {
            private set
            {
                if (value != _addingkey)
                {
                    if (value)
                    {
                        Keyboard.AddKeyDownHandler(this, KeyHookReq);
                        if (PopoutWindow != null)
                            Keyboard.AddKeyDownHandler(PopoutWindow, KeyHookReq);
                        ButtonsPanel.Children.Add(AddKeyItem.Item);
                        _addingkey = value;
                    }
                    else
                    {
                        Keyboard.RemoveKeyDownHandler(this, KeyHookReq);
                        if (PopoutWindow != null)
                            Keyboard.RemoveKeyDownHandler(PopoutWindow, KeyHookReq);
                        ButtonsPanel.Children.Remove(AddKeyItem.Item);
                        _addingkey = value;
                    }
                }
            }
            get => _addingkey;
        }

        private KeyPopoutWindow PopoutWindow;
        private KeyCounter KeyCounter;

        private bool _poppedout = false;
        public bool IsPoppedOut
        {
            private set
            {
                if (value != _poppedout)
                {
                    if (value)
                    {
                        KeysPageGrid.Children.Remove(ButtonsPanel);

                        PopoutWindow = new KeyPopoutWindow(ButtonsPanel, Config);
                        PopoutWindow.AddKeyRequested += AddKeyRequested;
                        PopoutWindow.Closed += (win, args) =>
                        {
                            (win as KeyPopoutWindow).Content = null;
                            KeysPageGrid.Children.Add(ButtonsPanel);

                            if (Window.Visibility == Visibility.Hidden)
                                Window.Show();
                            _poppedout = false;
                        };

                        PopoutWindow.Show();
                        if (Config.HideOnPopout)
                            this.Hide();
                        _poppedout = true;
                    }
                    else
                        PopoutWindow.Close();
                }
            }
            get => _poppedout;
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
                RefreshKeys();
            }
        }

        public void KeyHookReq(object sender, KeyEventArgs e)
        {
            AddKey(e.Key);
            if (AddingKey)
                AddingKey = false;
        }

        public void RemoveKey(Key key)
        {
            if (Config.Keys.Contains(key))
            {
                Config.Keys.Remove(key);
                RefreshKeys();
            }
        }

        public void DisplayKey(Key key)
        {
            var hook = new InputHook(key, Config.PollRate);
            var viewer = new KeyStateViewer(hook, Config);
            viewer.RemoveKeyRequested += RemoveKeyRequested;
            ButtonsPanel.Children.Add(viewer);

            var counters = ButtonsPanel.Children.Cast<KeyCounter>();
            if (counters != null)
                ButtonsPanel.Children.Remove(KeyCounter);
            if (Config.ShowKPS)
            {
                KeyCounter = new KeyCounter(ButtonsPanel, Config);
                ButtonsPanel.Children.Add(KeyCounter);
            }
        }

        private void AddKeyRequested(object sender, RoutedEventArgs e)
        {
            AddingKey = true;
        }

        private void RemoveKeyRequested(object sender, EventArgs e)
        {
            var key = sender as KeyStateViewer;
            RemoveKey(key.InputHook.Key);
        }

        #endregion

        #region Buttons & Configuration Settings

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

        #region Window Options

        private void WindowDrag_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState.HasFlag(MouseButtonState.Pressed) && e.ChangedButton == MouseButton.Left)
                this.DragMove();
            System.Diagnostics.Debug.WriteLine(e.ChangedButton);
        }

        private void PopoutKeys(object sender, RoutedEventArgs e)
        {
            IsPoppedOut = !IsPoppedOut;
        }

        #endregion

        private void RefreshKeys_Click(object sender, RoutedEventArgs e)
        {
            RefreshKeys();
        }
    }
}
