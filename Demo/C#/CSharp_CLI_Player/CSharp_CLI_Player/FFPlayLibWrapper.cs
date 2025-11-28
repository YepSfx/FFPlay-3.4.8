using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FFPlayLib
{
    class FFPlayLibWrapper
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

        [StructLayout(LayoutKind.Sequential)]
        public struct FFP_EVENTS
        {
            public IntPtr sender;               // void*
            public uint screenID;               // unsigned int
            public int bRendererRGB;            // int (boolean-ish)
            public long duration_in_us;         // int64_t -> C# long (64-bit)
            public double current_in_s;         // double
            public FFP_UI_TYPE ui_type;         // enum (int)

            // Callback function pointers (delegates)
            public FFP_EVENT_INFO event_info;
            public FFP_EVENT_EXIT event_exit;
            public FFP_EVENT_AUDIO event_audio;
            public FFP_EVENT_VIDEO event_video;
            public FFP_EVENT_VIDEORESIZE event_video_resize;
            public FFP_EVENT_PLAYSTATUS event_play_status;
            public FFP_EVENT_REFRESH event_refresh;

            public FFP_PLAY_STATUS playstatus; // enum
        }

        [DllImport(@".\FFPlayLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void multimedia_start_cli_player(int argc, IntPtr argv, ref FFP_EVENTS events);
    }
}
