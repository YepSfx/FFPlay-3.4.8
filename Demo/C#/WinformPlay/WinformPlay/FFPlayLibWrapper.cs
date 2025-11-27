using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FFPlayLib
{
    public enum FFP_UI_TYPE : int
    {
        FFP_CLI = 0,
        FFP_GUI = 1
    }

    public enum FFP_BOOL : int
    {
        FFP_FALSE = 0,
        FFP_TRUE = 1
    }

    public enum FFP_INFO : int
    {
        FFP_INFO_NONE = 0,
        FFP_INFO_WARNING = 1,
        FFP_INFO_ERROR = 2,
        FFP_INFO_STREAM_ERROR = 3,
        FFP_INFO_DEBUG = 4
    }

    public enum FFP_PLAY_STATUS : int
    {
        FFP_STOP = 0,
        FFP_PLAY = 1,
        FFP_PAUSED = 2,
        FFP_RESUMED = 3
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FFP_AUD_PARAMS
    {
        public int Freq;               // int
        public byte Channels;           // unsigned char
        public ushort Format;             // unsigned short
        public ushort SamplesInBuffer;    // unsigned short
        public uint BufferSizeInBytes;  // unsigned long (32-bit on Windows)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FFP_VID_PARAMS
    {
        public int width;   // int
        public int height;  // int
        public int Bpp;     // int
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FFPLAY_ELEMENTS
    {
        public IntPtr sender;
        
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 yuvHandle;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_INFO eventInfo;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_EXIT eventExit;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_AUDIO eventAudio;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_VIDEO eventVideo;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_VIDEORESIZE eventResize;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_PLAYSTATUS eventStatus;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFP_EVENT_REFRESH eventRefresh;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_EXIT(IntPtr sender, int exitCode);

    // void (*FFP_EVENT_INFO)(void *sender, int infoCode, char *Message);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_INFO(IntPtr sender, int infoCode, IntPtr Message);

    // void (*FFP_EVENT_AUDIO)(void *sender, unsigned char **AudBuffer, int BufferLengthInByte);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_AUDIO(IntPtr sender, IntPtr AudBuffer, int BufferLengthInByte);

    // void (*FFP_EVENT_VIDEO)(void *sender, void *rgbData, int isRGB);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_VIDEO(IntPtr sender, IntPtr rgbData, int isRGB);
    // isRGB: 0 -> IYUV ; 1 -> RGB for Video Image ; 2 -> RGB for Audio Image

    // void (*FFP_EVENT_VIDEORESIZE)(void *sender, int width, int height, int isOriginalsize);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_VIDEORESIZE(IntPtr sender, int width, int height, int isOriginalsize);

    // void (*FFP_EVENT_PLAYSTATUS)(void *sender, FFP_PLAY_STATUS status);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_PLAYSTATUS(IntPtr sender, FFP_PLAY_STATUS status);

    // void (*FFP_EVENT_VIDEOSIZE)(void *sender, int width, int height);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_VIDEOSIZE(IntPtr sender, int width, int height);

    // void (*FFP_EVENT_REFRESH)(void *sender);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFP_EVENT_REFRESH(IntPtr sender);

    class FFPlayLibWrapper
    {
        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Start_CLI_FFPlayer(int argv, IntPtr args, ref FFPLAY_ELEMENTS playelements);

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Setup_GUI_FFPlayer(int argv, IntPtr args, ref FFPLAY_ELEMENTS playelements);

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetupAndStart_GUI_FFPlayer(int argv, IntPtr args, ref FFPLAY_ELEMENTS playelements);

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Start_FFPlayer();

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PauseResume_FFPlayer();

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Stop_FFPlayer();

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Resize_GUI_Screen(int w, int h);

        [DllImport(@".\FFPlayLibWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Test_GUI_Screen(int xWinID, int holdTime);
    }
}
