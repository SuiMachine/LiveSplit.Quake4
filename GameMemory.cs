using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.ComponentUtil;

namespace LiveSplit.Quake4
{
    class GameMemory
    {
        public enum SplitArea : int
        {
            None,
            l01,
            l02,
            l03,
            l04,
            l05,
            l06,
            l07,
            l08,
            l09,
            l10,
            l11,
            l12,
            l13,
            l14,
            l15,
            l16,
            l17,
            l18,
            l19,
            l20,
            l21,
            l22,
            l23,
            l24,
            l25,
            l26,
            l27,
            l28,
            l29,
            l30,
            l31
        }

        public event EventHandler OnPlayerGainedControl;
        public event EventHandler OnLoadStarted;
        public event EventHandler OnFirstLevelLoading;
        public event EventHandler OnLoadFinished;
        public delegate void SplitCompletedEventHandler(object sender, SplitArea type, uint frame);
        public event SplitCompletedEventHandler OnSplitCompleted;

        private Task _thread;
        private CancellationTokenSource _cancelSource;
        private SynchronizationContext _uiThread;
        private List<int> _ignorePIDs;
        private Quake4Settings _settings;

        private DeepPointer _isLoadingPtr;
        private DeepPointer _levelNamePtr;
        private DeepPointer _isCutscenePtr;

        private static class LevelName
        {
            public const string l01_airDefence1 = "game/airdefense1";
            public const string l02_airDefence2 = "game/airdefense2";
            public const string l03_hangar1 = "game/hangar1";
            public const string l04_hangar2 = "game/hangar2";
            public const string l05_mcclanding = "game/mcc_landing";
            public const string l06_mcc1 = "game/mcc_1";
            public const string l07_convoy1 = "game/convoy1";
            public const string l08_buildingb = "game/building_b";
            public const string l09_convoy2 = "game/convoy2";
            public const string l10_convoy2b = "game/convoy2b";
            public const string l11_hub1 = "game/hub1";
            public const string l12_hub2 = "game/hub2";
            public const string l13_medlabs = "game/medlabs";
            public const string l14_walker = "game/walker";
            public const string l15_dispersal = "game/dispersal";
            public const string l16_recomp = "game/recomp";
            public const string l17_purification = "game/putra";
            public const string l18_wasteDisposal = "game/waste";
            public const string l19_mcc2 = "game/mcc_2";
            public const string l20_storage1 = "game/storage1";
            public const string l21_storage2 = "game/storage2";
            public const string l22_storage1 = "game/storage1";          //hub-like level
            public const string l23_tram1 = "game/tram1";
            public const string l24_tram1b = "game/tram1b";
            public const string l25_process1 = "game/process1";
            public const string l26_process2 = "game/process2";
            public const string l27_process1 = "game/process1";         //2nd hub-like level
            public const string l28_network1 = "game/network1";
            public const string l29_network2 = "game/network2";
            public const string l30_core1 = "game/core1";
            public const string l31_core2 = "game/core2";
        }

        private enum ExpectedDllSizes
        {
            PureFaction30d = 29945856
        }

        public bool[] splitStates { get; set; }

        public void resetSplitStates()
        {
            for (int i = 0; i <= (int)SplitArea.l31; i++)
            {
                splitStates[i] = false;
            }

        }

        public GameMemory(Quake4Settings componentSettings)
        {
            _settings = componentSettings;
            splitStates = new bool[(int)SplitArea.l31 + 1];

            _isLoadingPtr = new DeepPointer(0x507A35); // == 1 if a loadscreen is happening
            _levelNamePtr = new DeepPointer(0x507910);
            _isCutscenePtr = new DeepPointer("gamex86.dll", 0x86EE74);

            resetSplitStates();

            _ignorePIDs = new List<int>();
        }

        public void StartMonitoring()
        {
            if (_thread != null && _thread.Status == TaskStatus.Running)
            {
                throw new InvalidOperationException();
            }
            if (!(SynchronizationContext.Current is WindowsFormsSynchronizationContext))
            {
                throw new InvalidOperationException("SynchronizationContext.Current is not a UI thread.");
            }

            _uiThread = SynchronizationContext.Current;
            _cancelSource = new CancellationTokenSource();
            _thread = Task.Factory.StartNew(MemoryReadThread);
        }

        public void Stop()
        {
            if (_cancelSource == null || _thread == null || _thread.Status != TaskStatus.Running)
            {
                return;
            }

            _cancelSource.Cancel();
            _thread.Wait();
        }

        void MemoryReadThread()
        {
            Debug.WriteLine("[NoLoads] MemoryReadThread");

            while (!_cancelSource.IsCancellationRequested)
            {
                try
                {
                    Debug.WriteLine("[NoLoads] Waiting for quake4.exe...");

                    Process game;
                    while ((game = GetGameProcess()) == null)
                    {
                        Thread.Sleep(250);
                        if (_cancelSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    Debug.WriteLine("[NoLoads] Got games process!");

                    uint frameCounter = 0;

                    bool prevIsLoading = false;
                    bool prevIsCutscene = false;
                    string prevStreamGroupId = String.Empty;


                    bool loadingStarted = false;

                    while (!game.HasExited)
                    {
                        bool isLoading;
                        bool isCutscene;
                        string streamGroupId = String.Empty;
                        _levelNamePtr.DerefString(game, 30, out streamGroupId);
                        streamGroupId = streamGroupId.ToLower();
                        _isLoadingPtr.Deref(game, out isLoading);
                        _isCutscenePtr.Deref(game, out isCutscene);

                        if (streamGroupId != prevStreamGroupId && streamGroupId != null || prevIsCutscene != isCutscene)
                        {
                            if (prevStreamGroupId == LevelName.l01_airDefence1 && streamGroupId == LevelName.l02_airDefence2)
                            {
                                Split(SplitArea.l01, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l02_airDefence2 && streamGroupId == LevelName.l03_hangar1)
                            {
                                Split(SplitArea.l02, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l03_hangar1 && streamGroupId == LevelName.l04_hangar2)
                            {
                                Split(SplitArea.l03, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l04_hangar2 && streamGroupId == LevelName.l05_mcclanding)
                            {
                                Split(SplitArea.l04, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l05_mcclanding && streamGroupId == LevelName.l06_mcc1)
                            {
                                Split(SplitArea.l05, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l06_mcc1 && streamGroupId == LevelName.l07_convoy1)
                            {
                                Split(SplitArea.l06, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l07_convoy1 && streamGroupId == LevelName.l08_buildingb)
                            {
                                Split(SplitArea.l07, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l08_buildingb && streamGroupId == LevelName.l09_convoy2)
                            {
                                Split(SplitArea.l08, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l09_convoy2 && streamGroupId == LevelName.l10_convoy2b)
                            {
                                Split(SplitArea.l09, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l10_convoy2b && streamGroupId == LevelName.l11_hub1)
                            {
                                Split(SplitArea.l10, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l11_hub1 && streamGroupId == LevelName.l12_hub2)
                            {
                                Split(SplitArea.l11, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l12_hub2 && streamGroupId == LevelName.l13_medlabs)
                            {
                                Split(SplitArea.l12, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l13_medlabs && streamGroupId == LevelName.l14_walker)
                            {
                                Split(SplitArea.l13, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l14_walker && streamGroupId == LevelName.l15_dispersal)
                            {
                                Split(SplitArea.l14, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l15_dispersal && streamGroupId == LevelName.l16_recomp)
                            {
                                Split(SplitArea.l15, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l16_recomp && streamGroupId == LevelName.l17_purification)
                            {
                                Split(SplitArea.l16, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l17_purification && streamGroupId == LevelName.l18_wasteDisposal)
                            {
                                Split(SplitArea.l17, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l18_wasteDisposal && streamGroupId == LevelName.l19_mcc2)
                            {
                                Split(SplitArea.l18, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l19_mcc2 && streamGroupId == LevelName.l20_storage1)
                            {
                                Split(SplitArea.l19, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l20_storage1 && streamGroupId == LevelName.l21_storage2)
                            {
                                Split(SplitArea.l20, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l21_storage2 && streamGroupId == LevelName.l22_storage1)
                            {
                                Split(SplitArea.l21, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l22_storage1 && streamGroupId == LevelName.l23_tram1)
                            {
                                Split(SplitArea.l22, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l23_tram1 && streamGroupId == LevelName.l24_tram1b)
                            {
                                Split(SplitArea.l23, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l24_tram1b && streamGroupId == LevelName.l25_process1)
                            {
                                Split(SplitArea.l24, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l25_process1 && streamGroupId == LevelName.l26_process2)
                            {
                                Split(SplitArea.l25, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l26_process2 && streamGroupId == LevelName.l27_process1)
                            {
                                Split(SplitArea.l26, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l27_process1 && streamGroupId == LevelName.l28_network1)
                            {
                                Split(SplitArea.l27, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l28_network1 && streamGroupId == LevelName.l29_network2)
                            {
                                Split(SplitArea.l28, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l29_network2 && streamGroupId == LevelName.l30_core1)
                            {
                                Split(SplitArea.l29, frameCounter);
                            }
                            else if (prevStreamGroupId == LevelName.l30_core1 && streamGroupId == LevelName.l31_core2)
                            {
                                Split(SplitArea.l30, frameCounter);
                            }
                            else if (streamGroupId == LevelName.l31_core2 && isCutscene)
                            {
                                Split(SplitArea.l31, frameCounter);
                            }
                            else if (streamGroupId == LevelName.l01_airDefence1 && isCutscene == true && prevIsCutscene == false)
                            {
                                //Reset in game timer
                                _uiThread.Post(d =>
                                {
                                    if (this.OnFirstLevelLoading != null)
                                    {
                                        this.OnFirstLevelLoading(this, EventArgs.Empty);
                                    }
                                }, null);

                                // And instantly start it
                                _uiThread.Post(d =>
                                {
                                    if (this.OnPlayerGainedControl != null)
                                    {
                                        this.OnPlayerGainedControl(this, EventArgs.Empty);
                                    }
                                }, null);
                            }
                        }

                        _isLoadingPtr.Deref(game, out isLoading);

                        if (isLoading != prevIsLoading)
                        {
                            if (isLoading)
                            {
                                Debug.WriteLine(String.Format("[NoLoads] Load Start - {0}", frameCounter));

                                loadingStarted = true;

                                // pause game timer
                                _uiThread.Post(d =>
                                {
                                    if (this.OnLoadStarted != null)
                                    {
                                        this.OnLoadStarted(this, EventArgs.Empty);
                                    }
                                }, null);
                            }
                            else
                            {
                                Debug.WriteLine(String.Format("[NoLoads] Load End - {0}", frameCounter));
                                if (loadingStarted)
                                {
                                    loadingStarted = false;

                                    // unpause game timer
                                    _uiThread.Post(d =>
                                    {
                                        if (this.OnLoadFinished != null)
                                        {
                                            this.OnLoadFinished(this, EventArgs.Empty);
                                        }
                                    }, null);
                                }
                            }
                        }


                        Debug.WriteLineIf(streamGroupId != prevStreamGroupId, String.Format("[NoLoads] streamGroupId changed from {0} to {1} - {2}", prevStreamGroupId, streamGroupId, frameCounter));
                        prevStreamGroupId = streamGroupId;
                        prevIsLoading = isLoading;
                        prevIsCutscene = isCutscene;

                        frameCounter++;

                        Thread.Sleep(15);

                        if (_cancelSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Thread.Sleep(1000);
                }
            }
        }

        private void Split(SplitArea split, uint frame)
        {
            Debug.WriteLine(String.Format("[NoLoads] split {0} - {1}", split, frame));
            _uiThread.Post(d =>
            {
                if (this.OnSplitCompleted != null)
                {
                    this.OnSplitCompleted(this, split, frame);
                }
            }, null);
        }

        Process GetGameProcess()
        {
            Process game = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.ToLower() == "quake4" && !p.HasExited && !_ignorePIDs.Contains(p.Id));
            if (game == null)
            {
                return null;
            }
            /*
            if (game.MainModuleWow64Safe().ModuleMemorySize == (int)ExpectedDllSizes.PureFaction30d)
            {

            }
            else
            {
                _ignorePIDs.Add(game.Id);
                _uiThread.Send(d => MessageBox.Show("Unexpected game version. Red Faction (Pure Faction 3.0d) is required", "LiveSplit.RedFaction",
                    MessageBoxButtons.OK, MessageBoxIcon.Error), null);
                return null;
            }*/

            return game;
        }
    }
}
