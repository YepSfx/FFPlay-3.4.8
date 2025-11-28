using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using FFPlayLib;

namespace CSharp_CLI_Player
{
    class CSharp_CLI_Player
    {
        static public void OnExit(IntPtr sender, int exitCode)
        {
            Console.WriteLine(@"->> Player Exit: " + exitCode.ToString());
        }

        static public void OnInfo(IntPtr sender, int infoCode, IntPtr Message)
        {
            string msg = Marshal.PtrToStringAnsi(Message);
            Console.WriteLine(@"->> Player Info: " + infoCode.ToString() + @" " + msg);
        }

        static public void OnAudio(IntPtr sender, IntPtr AudioBuffer, int BufferLengthInByte)
        {
            //Trace.WriteLine(@"->> Audio event");
        }

        static public void OnVideo(IntPtr sender, IntPtr yuvData)
        {
            //Trace.WriteLine(@"->> Video event");
        }

        static public void OnVideoResize(IntPtr sender, int width, int height, int isOriginalsize)
        {
            Console.WriteLine(@"->> Player resizes the screen: " + "w-" + width.ToString() 
                                                               + " h-" + height.ToString() 
                                                               + " Original-" + isOriginalsize.ToString());
        }

        static public void OnPlayStatus(IntPtr sender, FFPlayLibWrapper.FFP_PLAY_STATUS status)
        {
            Console.WriteLine(@"->> Player status: " + status.ToString());
        }

        static void Main(string[] args)
        {
            
            string[] argv = { @"CLI_Player"};
            Array.Resize(ref argv, argv.Length + args.Length);
            for (int i = 0 ; i < args.Length ; i++)
            {
                argv[i + 1] = args[i];
            }
            IntPtr argvPtr = Marshal.AllocHGlobal(IntPtr.Size * argv.Length);
            IntPtr[] utf8Ptrs = new IntPtr[argv.Length];
            for (int i = 0; i < argv.Length; i++)
            {
                byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(argv[i] + "\0");
                utf8Ptrs[i] = Marshal.AllocHGlobal(utf8Bytes.Length);
                Marshal.Copy(utf8Bytes, 0, utf8Ptrs[i], utf8Bytes.Length);
                Marshal.WriteIntPtr(argvPtr, i * IntPtr.Size, utf8Ptrs[i]);
            }

            FFPlayLibWrapper.FFP_EVENTS events = new FFPlayLibWrapper.FFP_EVENTS();
            events.sender = IntPtr.Zero;
            events.bRendererRGB = 0;
            events.screenID     = 0;
            events.ui_type = FFPlayLibWrapper.FFP_UI_TYPE.FFP_CLI;
            events.playstatus = FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_STOP;

            events.event_audio   = null;
            events.event_video   = null;
            events.event_refresh = null;
            events.event_exit    = OnExit;
            events.event_info    = OnInfo;
            events.event_play_status  = OnPlayStatus;
            events.event_video_resize = OnVideoResize;

            FFPlayLibWrapper.multimedia_start_cli_player(argv.Length, argvPtr, ref events);

            foreach (var ptr in utf8Ptrs) 
                Marshal.FreeHGlobal(ptr);
            Marshal.FreeHGlobal(argvPtr);
            
            Console.WriteLine(@"Player Terminated");
        }
    }
}
