using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using FFPlayLib;
using System.IO;

namespace WinformPlay
{
    public partial class frmMain : Form
    {
        static public void OnRefresh(IntPtr sender)
        {
            Application.DoEvents();
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
            frmMain frm = (frmMain)mhWnd.Target;

            if (frm == null)
                return;

            if (status ==FFPlayLib.FFP_PLAY_STATUS.FFP_STOP)
            {
                frm.InvalidateScreen();
            }

            frm.m_Current_Status = status;
        }

        public enum PLAYINGMODE
        {
            STOP = 0,
            PLAY = 1,
            PAUSE = 2,
            RESUME = 3
        };

        private PLAYINGMODE m_CurrentMode;

        private GCHandle mForm;

        private Thread m_PlayingThread;

        public FFPlayLib.FFP_PLAY_STATUS m_Current_Status = FFPlayLib.FFP_PLAY_STATUS.FFP_STOP;

        private FFPLAY_POS m_PlayPos = new FFPLAY_POS();

        private void StartPlayingThread()
        {
            m_PlayingThread = new Thread(new ThreadStart(FFPlayLibWrapper.Start_FFPlayer));
            m_PlayingThread.Start();
        }

        public void SetPlayingMode(PLAYINGMODE playingmode)
        {
            switch(playingmode)
            {
                case PLAYINGMODE.STOP:
                    mButtonPlay.Enabled = true;
                    mButtonStop.Enabled = false;
                    mButtoonPause.Enabled = false;
                    mButtoonPause.Text = @"PAUSE";
                    mTimer.Enabled = false;
                    break;
                case PLAYINGMODE.PLAY:
                    mButtonPlay.Enabled = false;
                    mButtonStop.Enabled = true;
                    mButtoonPause.Enabled = true;
                    mButtoonPause.Text = @"PAUSE";
                    mTimer.Enabled = true;
                    break;
                case PLAYINGMODE.PAUSE:
                    mButtonPlay.Enabled = false;
                    mButtonStop.Enabled = true;
                    mButtoonPause.Enabled = true;
                    mButtoonPause.Text = @"RESUME";
                    mTimer.Enabled = false;
                    break;
                case PLAYINGMODE.RESUME:
                    mButtonPlay.Enabled = false;
                    mButtonStop.Enabled = true;
                    mButtoonPause.Enabled = true;
                    mButtoonPause.Text = @"PAUSE";
                    mTimer.Enabled = true;
                    break;
            }
            m_CurrentMode = playingmode;

            if(m_CurrentMode == PLAYINGMODE.STOP)
            {
                mButtonTestScreen.Enabled = true;
                mButtonCLI.Enabled = true;
            }
            else
            {
                mButtonTestScreen.Enabled = false;
                mButtonCLI.Enabled = false;
            }
        }
        
        public void InvalidateScreen()
        {

            if (mPanelYUV.InvokeRequired)
            {
                mPanelYUV.BeginInvoke(new Action(() =>
                {
                    mPanelYUV.Invalidate(); 
                }));
            }
            else
            {
                mPanelYUV.Invalidate();
                SetPlayingMode(PLAYINGMODE.STOP);
            }
        }
        
        public frmMain()
        {
            InitializeComponent();
            SetPlayingMode(PLAYINGMODE.STOP);
            openMediaFileDialog.Filter = @"Mov (*.mov)|*.mov|AVI (*.avi)|*.avi|MP4 (*.mp4)|*.mp4|All Files (*.*)|*.*";
            mForm = GCHandle.Alloc(this);
        }

        private void mButtonPlay_Click(object sender, EventArgs e)
        {
            if (openMediaFileDialog.ShowDialog() == DialogResult.OK)
            {
                string utf16FileName = openMediaFileDialog.FileName;
                string utf16SmiPath = Path.ChangeExtension(utf16FileName, ".smi");
                string utf16SubtitleArg = utf16SmiPath.Replace("\\", "/");
                utf16SubtitleArg = utf16SubtitleArg.Replace(":", @"\:");
                utf16SubtitleArg = string.Format("subtitles='{0}':charenc=cp949", utf16SubtitleArg);
                
                IntPtr self = (IntPtr)mForm;
                UInt32 hYuv = (UInt32)mPanelYUV.Handle;
                
                IntPtr argvPtr = IntPtr.Zero;
                IntPtr[] utf8Ptrs = { IntPtr.Zero };
                try 
                {
                    string[] argv;
                    string[] args;

                    if (File.Exists(utf16SmiPath))
                    {
                        argv = new string[]{ @"GUI_Player" };
                        args = new string[]{ utf16FileName, @"-vf", utf16SubtitleArg, @"-vf", @"yadif=1" };
                    }
                    else
                    {
                        argv = new string[] { @"GUI_Player" };
                        args = new string[] { utf16FileName, @"-vf", @"yadif=1" };
                    }

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
                        frmMain_ResizeEnd(null, null);
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

        private void mButtoonPause_Click(object sender, EventArgs e)
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

        private void mButtonStop_Click(object sender, EventArgs e)
        {
            FFPlayLibWrapper.Stop_FFPlayer();

            SetPlayingMode(PLAYINGMODE.STOP);
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            int w, h;
            w = mPanelYUV.Width;
            h = mPanelYUV.Height;
            FFPlayLibWrapper.Resize_GUI_Screen(w, h);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mForm.Free();
        }

        private void mButtonCLI_Click(object sender, EventArgs e)
        {
            if (openMediaFileDialog.ShowDialog() == DialogResult.OK)
            {
                mButtonCLI.Enabled = false;

                IntPtr self = (IntPtr)mForm;

                string utf16FileName = openMediaFileDialog.FileName;
                IntPtr argvPtr = IntPtr.Zero;
                IntPtr[] utf8Ptrs = { IntPtr.Zero };

                try
                {
                    string[] argv = { @"CLI_Player" };
                    string[] args = { utf16FileName};

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
                    playElements.yuvHandle = 0;
                    playElements.eventInfo = OnInfo;
                    playElements.eventExit = OnExit;
                    playElements.eventAudio = OnAudio;
                    playElements.eventVideo = null;
                    playElements.eventResize = OnVideoResize;
                    playElements.eventStatus = OnPlayStatus;
                    playElements.eventRefresh = null;

                    FFPlayLibWrapper.Start_CLI_FFPlayer(argv.Length, argvPtr, ref playElements);
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

                    mButtonCLI.Enabled = true;
                }
            }
        }

        private void mButtonTestScreen_Click(object sender, EventArgs e)
        {
            int hYuv = (int)mPanelYUV.Handle;
            FFPlayLibWrapper.Test_GUI_Screen(hYuv, 5000);
            InvalidateScreen();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mButtonStop_Click(null, null);
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            if (m_Current_Status != FFPlayLib.FFP_PLAY_STATUS.FFP_PLAY)
                return;

            FFPlayLibWrapper.GetPositionInSecond_FFPlayer(ref m_PlayPos);
            UInt32 max, curr;
            max = m_PlayPos.max_duration;
            curr = m_PlayPos.current_position;
            mScrollBar.Minimum = 0;
            mScrollBar.Maximum = (int)max;
            mScrollBar.Value = (int)curr;
            mLabelPos.Text = curr.ToString() + " / " + max.ToString();
        }

        private void mScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.EndScroll)
            {
                if (m_Current_Status == FFPlayLib.FFP_PLAY_STATUS.FFP_STOP)
                    return;

                mTimer.Enabled = false;
                int pos = mScrollBar.Value;
                FFPlayLibWrapper.SeekPositionInSecond_FFPlayer(pos);
                mTimer.Enabled = true;
            }
        }
    }
}
