using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace KeyOverlay
{
    public class InputHook
    {
        public InputHook(Key key, int pollrate = 10)
        {
            Key = key;
            PollRate = pollrate;
        }

        #region Properties & Events

        public event EventHandler<bool> StateChanged;

        public Key Key { protected set; get; }
        public KeyStates CurrentState { private set; get; }

        private Timer PollTimer { set; get; }
        public int PollRate { protected set; get; }

        private Dispatcher AppDispatcher = Application.Current.Dispatcher;

        #endregion

        #region Public Methods

        public void Hook()
        {
            PollTimer = new Timer
            {
                AutoReset = true,
                Interval = PollRate,
            };
            PollTimer.Elapsed += PollKeyState;
            PollTimer.Start();
        }

        public void Unhook()
        {
            if (PollTimer != null)
            {
                PollTimer.Stop();
                PollTimer.Dispose();
            }
        }

        #endregion

        #region Polling

        private async void PollKeyState(object sender, ElapsedEventArgs e)
        {
            await AppDispatcher?.InvokeAsync(() =>
            {
                var newState = Keyboard.GetKeyStates(Key);
                if (newState != CurrentState)
                {
                    CurrentState = newState;
                    StateChanged?.Invoke(this, CurrentState.HasFlag(KeyStates.Down));
                }
            });
        }

        #endregion
    }
}
