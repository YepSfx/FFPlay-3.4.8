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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WpfPlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum PLAYINGMODE
        {
            STOP = 0,
            PLAY = 1,
            PAUSE = 2,
            RESUME = 3
        };

        private PLAYINGMODE m_CurrentMode;
        private System.Windows.Forms.Panel mYuvPanel;
        private Microsoft.Win32.OpenFileDialog mOpenFileDialog;
        private GCHandle mForm;

        public delegate void InvokeMethod(string msg);

        public MainWindow()
        {
            InitializeComponent();
            SetPlayingMode(PLAYINGMODE.STOP);
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

            if (m_CurrentMode == PLAYINGMODE.STOP)
            {
            }
            else
            {
            }
        }

        private void mButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            IntPtr self = (IntPtr)mForm;
            UInt32 hYuv = (UInt32)mYuvPanel.Handle;

            SetPlayingMode(PLAYINGMODE.PLAY);
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

        }

        private void mButtonStop_Click(object sender, RoutedEventArgs e)
        {
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

        }
    }
}
