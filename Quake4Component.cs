using LiveSplit.Model;
using LiveSplit.TimeFormatters;
using LiveSplit.UI.Components;
using LiveSplit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

namespace LiveSplit.Quake4
{
    class Quake4Component : LogicComponent
    {
        public override string ComponentName
        {
            get { return "Quake 4"; }
        }

        public Quake4Settings Settings { get; set; }

        public bool Disposed { get; private set; }
        public bool IsLayoutComponent { get; private set; }

        private TimerModel _timer;
        private GameMemory _gameMemory;
        private LiveSplitState _state;

        public Quake4Component(LiveSplitState state, bool isLayoutComponent)
        {
            _state = state;
            this.IsLayoutComponent = isLayoutComponent;

            this.Settings = new Quake4Settings();

           _timer = new TimerModel { CurrentState = state };
           _timer.CurrentState.OnPause += timer_OnStart;

            _gameMemory = new GameMemory(this.Settings);
            _gameMemory.OnFirstLevelLoading += gameMemory_OnFirstLevelLoading;
            _gameMemory.OnPlayerGainedControl += gameMemory_OnPlayerGainedControl;
            _gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
            _gameMemory.OnLoadFinished += gameMemory_OnLoadFinished;
            _gameMemory.OnSplitCompleted += gameMemory_OnSplitCompleted;
            state.OnStart += State_OnStart;
            _gameMemory.StartMonitoring();
        }

        public override void Dispose()
        {
            this.Disposed = true;

            _state.OnStart -= State_OnStart;
            _timer.CurrentState.OnStart -= timer_OnStart;

            if (_gameMemory != null)
            {
                _gameMemory.Stop();
            }

        }

        private void timer_OnStart(object sender, EventArgs e)
        {
            _timer.InitializeGameTime();
        }

        void State_OnStart(object sender, EventArgs e)
        {
            _gameMemory.resetSplitStates();
        }

        void gameMemory_OnFirstLevelLoading(object sender, EventArgs e)
        {
            if (this.Settings.AutoReset)
            {
                _timer.Reset();
            }
        }

        void gameMemory_OnPlayerGainedControl(object sender, EventArgs e)
        {
            if (this.Settings.AutoStart)
            {
                _timer.Start();
            }
        }

        void gameMemory_OnLoadStarted(object sender, EventArgs e)
        {
            _state.IsGameTimePaused = true;
        }

        void gameMemory_OnLoadFinished(object sender, EventArgs e)
        {
            _state.IsGameTimePaused = false;
        }

        void gameMemory_OnSplitCompleted(object sender, GameMemory.SplitArea split, uint frame)
        {
            Debug.WriteLineIf(split != GameMemory.SplitArea.None, String.Format("[NoLoads] Trying to split {0}, State: {1} - {2}", split, _gameMemory.splitStates[(int)split], frame));
            if (_state.CurrentPhase == TimerPhase.Running && !_gameMemory.splitStates[(int)split] &&
                ((split == GameMemory.SplitArea.l01 && this.Settings.sC01) ||
                (split == GameMemory.SplitArea.l02 && this.Settings.sC02) ||
                (split == GameMemory.SplitArea.l03 && this.Settings.sC03) ||
                (split == GameMemory.SplitArea.l04 && this.Settings.sC04) ||
                (split == GameMemory.SplitArea.l05 && this.Settings.sC05) ||
                (split == GameMemory.SplitArea.l06 && this.Settings.sC06) ||
                (split == GameMemory.SplitArea.l07 && this.Settings.sC07) ||
                (split == GameMemory.SplitArea.l08 && this.Settings.sC08) ||
                (split == GameMemory.SplitArea.l09 && this.Settings.sC09) ||
                (split == GameMemory.SplitArea.l10 && this.Settings.sC10) ||
                (split == GameMemory.SplitArea.l11 && this.Settings.sC11) ||
                (split == GameMemory.SplitArea.l12 && this.Settings.sC12) ||
                (split == GameMemory.SplitArea.l13 && this.Settings.sC13) ||
                (split == GameMemory.SplitArea.l14 && this.Settings.sC14) ||
                (split == GameMemory.SplitArea.l15 && this.Settings.sC15) ||
                (split == GameMemory.SplitArea.l16 && this.Settings.sC16) ||
                (split == GameMemory.SplitArea.l17 && this.Settings.sC17) ||
                (split == GameMemory.SplitArea.l18 && this.Settings.sC18) ||
                (split == GameMemory.SplitArea.l29 && this.Settings.sC19) ||
                (split == GameMemory.SplitArea.l20 && this.Settings.sC20) ||
                (split == GameMemory.SplitArea.l21 && this.Settings.sC21) ||
                (split == GameMemory.SplitArea.l22 && this.Settings.sC22) ||
                (split == GameMemory.SplitArea.l23 && this.Settings.sC23) ||
                (split == GameMemory.SplitArea.l24 && this.Settings.sC24) ||
                (split == GameMemory.SplitArea.l25 && this.Settings.sC25) ||
                (split == GameMemory.SplitArea.l26 && this.Settings.sC26) ||
                (split == GameMemory.SplitArea.l27 && this.Settings.sC27) ||
                (split == GameMemory.SplitArea.l28 && this.Settings.sC28) ||
                (split == GameMemory.SplitArea.l29 && this.Settings.sC29) ||
                (split == GameMemory.SplitArea.l30 && this.Settings.sC30) ||
                (split == GameMemory.SplitArea.l31 && this.Settings.sC31)
                ))
            {
                Trace.WriteLine(String.Format("[NoLoads] {0} Split - {1}", split, frame));
                _timer.Split();
                _gameMemory.splitStates[(int)split] = true;
            }
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return this.Settings.GetSettings(document);
        }

        public override Control GetSettingsControl(LayoutMode mode)
        {
            return this.Settings;
        }

        public override void SetSettings(XmlNode settings)
        {
            this.Settings.SetSettings(settings);
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
        //public override void RenameComparison(string oldName, string newName) { }
    }
}
