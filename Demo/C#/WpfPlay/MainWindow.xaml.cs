using FFPlayLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace WpfPlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public void PlayDoEvents()
        {
            Application.Current.Dispatcher.Invoke(() => { }, DispatcherPriority.Background);
        }
        static public void OnRefresh(IntPtr sender)
        {
            PlayDoEvents();
        }
        static public void OnExit(IntPtr sender, int exitCode)
        {
            Trace.WriteLine(@"->> Player Exit: " + exitCode.ToString());
        }

        static public void OnInfo(IntPtr sender, int infoCode, IntPtr Message)
        {
            string msg = Marshal.PtrToStringAnsi(Message);
            Trace.WriteLine(@"->> Player Info: " + infoCode.ToString() + @" " + msg);
        }

        static public void OnAudio(IntPtr sender, IntPtr AudioBuffer, int BufferLengthInByte)
        {
            Trace.WriteLine(@"->> Audio event");
        }
        static public void OnVideo(IntPtr sender, IntPtr rgbData, int isRGB)
        {
            Trace.WriteLine(@"->> Video event");
        }

        static public void OnVideoResize(IntPtr sender, int width, int height, int isOriginalsize)
        {
            Trace.WriteLine(@"->> Player resizes the screen: " + "w-" + width.ToString()
                                                               + " h-" + height.ToString()
                                                               + " Original-" + isOriginalsize.ToString());
        }

        static public void OnPlayStatus(IntPtr sender, FFPlayLib.FFP_PLAY_STATUS status)
        {
            Trace.WriteLine(@"->> Player status: " + status.ToString());
            GCHandle mhWnd = GCHandle.FromIntPtr(sender);
            MainWindow win = (MainWindow)mhWnd.Target;

            if (status == FFPlayLib.FFP_PLAY_STATUS.FFP_STOP)
            {
                win.InvalidateScreen();
            }

            win.m_Current_Status = status;
        }

        public enum PLAYINGMODE
        {
            STOP = 0,
            PLAY = 1,
            PAUSE = 2,
            RESUME = 3
        };

        private PLAYINGMODE m_CurrentMode;
        private System.Windows.Forms.Panel mYuvPanel = null;
        private Microsoft.Win32.OpenFileDialog mOpenFileDialog = new Microsoft.Win32.OpenFileDialog();
        private GCHandle mForm;

        public delegate void InvokeMethod(string msg);

        private Thread m_PlayingThread;

        public FFPlayLib.FFP_PLAY_STATUS m_Current_Status = FFPlayLib.FFP_PLAY_STATUS.FFP_STOP;

        public MainWindow()
        {
            InitializeComponent();
            SetPlayingMode(PLAYINGMODE.STOP);
            mOpenFileDialog.Filter = @"Mov (*.mov)|*.mov|AVI (*.avi)|*.avi|MP4 (*.mp4)|*.mp4|All Files (*.*)|*.*";
        }

        public void InvalidateScreen()
        {

            if (mYuvPanel.InvokeRequired)
            {
                mYuvPanel.BeginInvoke(new Action(() =>
                {
                    mYuvPanel.Invalidate();
                }));
            }
            else
            {
                mYuvPanel.Invalidate();
                SetPlayingMode(PLAYINGMODE.STOP);
            }
        }

        public void SetPlayingMode(PLAYINGMODE playingmode)
        {
            switch (playingmode)
            {
                case PLAYINGMODE.STOP:
                    mButtonPlay.IsEnabled = true;
                    mButtonStop.IsEnabled = false;
                    mButtonPause.IsEnabled = false;
                    mButtonPause.Content = @"PAUSE";
                    break;
                case PLAYINGMODE.PLAY:
                    mButtonPlay.IsEnabled = false;
                    mButtonStop.IsEnabled = true;
                    mButtonPause.IsEnabled = true;
                    mButtonPause.Content = @"PAUSE";
                    break;
                case PLAYINGMODE.PAUSE:
                    mButtonPlay.IsEnabled = false;
                    mButtonStop.IsEnabled = true;
                    mButtonPause.IsEnabled = true;
                    mButtonPause.Content = @"RESUME";
                    break;
                case PLAYINGMODE.RESUME:
                    mButtonPlay.IsEnabled = false;
                    mButtonStop.IsEnabled = true;
                    mButtonPause.IsEnabled = true;
                    mButtonPause.Content = @"PAUSE";
                    break;
            }
            m_CurrentMode = playingmode;
        }

        private void mButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            IntPtr self = (IntPtr)mForm;
            UInt32 hYuv = (UInt32)mYuvPanel.Handle;

            SetPlayingMode(PLAYINGMODE.PLAY);

            if (mOpenFileDialog.ShowDialog() == true)
            {
                string utf16FileName = mOpenFileDialog.FileName;

                IntPtr argvPtr = IntPtr.Zero;
                IntPtr[] utf8Ptrs = { IntPtr.Zero };
                try
                {
                    string[] argv = { @"WPF_GUI_Player" };
                    string[] args = { utf16FileName };

                    Array.Resize(ref argv, argv.Length + args.Length);
                    for (int i = 0; i < args.Length; i++)
                    {
                        argv[i + 1] = args[i];
                    }

                    argvPtr = Marshal.AllocHGlobal(IntPtr.Size * argv.Length);
                    utf8Ptrs = new IntPtr[argv.Length];
                    for (int i = 0; i < argv.Length; i++)
                    {
                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(argv[i] + "\0");
                        utf8Ptrs[i] = Marshal.AllocHGlobal(utf8Bytes.Length);
                        Marshal.Copy(utf8Bytes, 0, utf8Ptrs[i], utf8Bytes.Length);
                        Marshal.WriteIntPtr(argvPtr, i * IntPtr.Size, utf8Ptrs[i]);
                    }

                    FFPLAY_ELEMENTS playElements = new FFPLAY_ELEMENTS();
                    playElements.sender = self;
                    playElements.yuvHandle = hYuv;
                    playElements.eventInfo = OnInfo;
                    playElements.eventExit = OnExit;
                    playElements.eventAudio = OnAudio;
                    playElements.eventVideo = OnVideo;
                    playElements.eventResize = OnVideoResize;
                    playElements.eventStatus = OnPlayStatus;
                    playElements.eventRefresh = OnRefresh;

                    int rtn = FFPlayLibWrapper.Setup_GUI_FFPlayer(argv.Length, argvPtr, ref playElements);

                    if (rtn == 0)
                    {
                        SetPlayingMode(PLAYINGMODE.PLAY);
                        winMainForm_SizeChanged(null, null);
                        FFPlayLibWrapper.Start_FFPlayer();
                    }
                }
                catch
                {
                    Trace.WriteLine("Have a problem to play");
                }
                finally
                {
                    foreach (var ptr in utf8Ptrs)
                        Marshal.FreeHGlobal(ptr);
                    Marshal.FreeHGlobal(argvPtr);
                }
            }
        }

        private void mButtonPause_Click(object sender, RoutedEventArgs e)
        {
            if (m_CurrentMode == PLAYINGMODE.PAUSE)
            {
                SetPlayingMode(PLAYINGMODE.RESUME);
            }
            else
            {
                SetPlayingMode(PLAYINGMODE.PAUSE);
            }
            FFPlayLibWrapper.PauseResume_FFPlayer();
        }

        private void mButtonStop_Click(object sender, RoutedEventArgs e)
        {
            FFPlayLibWrapper.Stop_FFPlayer();
            SetPlayingMode(PLAYINGMODE.STOP);
        }

        private void winMainForm_Loaded(object sender, RoutedEventArgs e)
        {
            mForm = GCHandle.Alloc(this);
            mYuvPanel = new System.Windows.Forms.Panel();
            mHost.Child = mYuvPanel;
        }

        private void winMainForm_Closed(object sender, EventArgs e)
        {
            mForm.Free();
            mYuvPanel.Dispose();
        }

        private void winMainForm_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (mYuvPanel != null)
            {
                int w, h;
                w = mYuvPanel.Width;
                h = mYuvPanel.Height;
                FFPlayLibWrapper.Resize_GUI_Screen(w, h);
            }
        }
    }
}
