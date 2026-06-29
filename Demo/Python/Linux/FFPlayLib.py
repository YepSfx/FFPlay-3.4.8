import ctypes
from ctypes import *

# =========================
# CONSTANTS (#define)
# =========================
FFP_AUDIO_U8      = 0x0008
FFP_AUDIO_S8      = 0x8008
FFP_AUDIO_U16LSB  = 0x0010
FFP_AUDIO_S16LSB  = 0x8010
FFP_AUDIO_U16MSB  = 0x1010
FFP_AUDIO_S16MSB  = 0x9010

FFP_AUDIO_U16 = FFP_AUDIO_U16LSB
FFP_AUDIO_S16 = FFP_AUDIO_S16LSB

# =========================
# ENUMS
# =========================
class FFP_UI_TYPE(c_int):
    FFP_CLI = 0
    FFP_GUI = 1

class FFP_BOOL(c_int):
    FFP_FALSE = 0
    FFP_TRUE = 1

class FFP_INFO(c_int):
    FFP_INFO_NONE = 0
    FFP_INFO_WARNING = 1
    FFP_INFO_ERROR = 2
    FFP_INFO_STREAM_ERROR = 3
    FFP_INFO_DEBUG = 4

class FFP_PLAY_STATUS(c_int):
    FFP_STOP = 0
    FFP_PLAY = 1
    FFP_PAUSED = 2
    FFP_RESUMED = 3


# =========================
# STRUCTS
# =========================
class FFP_AUD_PARAMS(Structure):
    _fields_ = [
        ("Freq", c_int),
        ("Channels", c_ubyte),
        ("Format", c_ushort),
        ("SamplesInBuffer", c_ushort),
        ("BufferSizeInBytes", c_ulong),
    ]

class FFP_VID_PARAMS(Structure):
    _fields_ = [
        ("width", c_int),
        ("height", c_int),
        ("Bpp", c_int),
    ]

class FFP_YUV420P_DATA(Structure):
    _fields_ = [
        ("w", c_int),
        ("h", c_int),
        ("pixels", POINTER(POINTER(c_ubyte))),
    ]

class FFP_RGB_DATA(Structure):
    _fields_ = [
        ("w", c_int),
        ("h", c_int),
        ("BPP", c_int),
        ("pixels", POINTER(c_ubyte)),
    ]


# =========================
# CALLBACK TYPES
# =========================
FFP_EVENT_EXIT = CFUNCTYPE(None, c_void_p, c_int)
FFP_EVENT_INFO = CFUNCTYPE(None, c_void_p, c_int, c_char_p)
FFP_EVENT_AUDIO = CFUNCTYPE(None, c_void_p, POINTER(POINTER(c_ubyte)), c_int)
FFP_EVENT_VIDEO = CFUNCTYPE(None, c_void_p, c_void_p, c_int)
FFP_EVENT_VIDEORESIZE = CFUNCTYPE(None, c_void_p, c_int, c_int, c_int)
FFP_EVENT_PLAYSTATUS = CFUNCTYPE(None, c_void_p, c_int)
FFP_EVENT_VIDEOSIZE = CFUNCTYPE(None, c_void_p, c_int, c_int)
FFP_EVENT_REFRESH = CFUNCTYPE(None, c_void_p)


# =========================
# EVENTS STRUCT
# =========================
class FFP_EVENTS(Structure):
    _fields_ = [
        ("sender", c_void_p),
        ("screenID", c_uint),
        ("bRendererRGB", c_int),
        ("duration_in_us", c_int64),
        ("current_in_s", c_double),
        ("ui_type", c_int),

        ("event_info", FFP_EVENT_INFO),
        ("event_exit", FFP_EVENT_EXIT),
        ("event_audio", FFP_EVENT_AUDIO),
        ("event_video", FFP_EVENT_VIDEO),
        ("event_video_resize", FFP_EVENT_VIDEORESIZE),
        ("event_play_status", FFP_EVENT_PLAYSTATUS),
        ("event_refresh", FFP_EVENT_REFRESH),

        ("playstatus", c_int),
    ]


# Load your DLL
libFFPlay = ctypes.CDLL("libFFPlay.so")  # adjust path if needed

# =========================
# FUNCTION SIGNATURES
# =========================

libFFPlay.multimedia_get_filename.restype = c_char_p

libFFPlay.multimedia_init_device.argtypes = [POINTER(FFP_EVENTS)]
libFFPlay.multimedia_init_device.restype = c_int

libFFPlay.multimedia_parse_options.argtypes = [c_int, POINTER(c_char_p)]

libFFPlay.multimedia_set_filename.argtypes = [c_char_p]

libFFPlay.multimedia_get_audioformat.restype = POINTER(FFP_AUD_PARAMS)
libFFPlay.multimedia_get_videoformat.restype = POINTER(FFP_VID_PARAMS)

libFFPlay.multimedia_get_duration_in_mSec.restype = c_int64
libFFPlay.multimedia_event_loop_alive.restype = c_int

libFFPlay.multimedia_resize_screen.argtypes = [c_int, c_int]
libFFPlay.multimedia_reset_pointer.argtypes = []

libFFPlay.multimedia_stream_open.restype = c_int  # maps FFP_BOOL

libFFPlay.multimedia_yuv420p_to_rgb24.argtypes = [POINTER(FFP_YUV420P_DATA), POINTER(c_ubyte)]
libFFPlay.multimedia_yuv420p_to_rgb32.argtypes = [POINTER(FFP_YUV420P_DATA), POINTER(c_ubyte)]

libFFPlay.multimedia_rgb_swap.argtypes = [c_void_p, c_int, c_int, c_int, c_int, c_int, c_int]

libFFPlay.multimedia_start_cli_player.argtypes = [c_int, POINTER(c_char_p), POINTER(FFP_EVENTS)]
libFFPlay.multimedia_start_gui_player.argtypes = [c_char_p, POINTER(FFP_EVENTS)]

libFFPlay.multimedia_seek_time.argtypes = [c_int]
libFFPlay.multimedia_pause_resume.argtypes = []

libFFPlay.multimedia_stream_stop.argtypes = []
libFFPlay.multimedia_stream_start.argtypes = []
libFFPlay.multimedia_exit.argtypes = []

libFFPlay.multimedia_test_screen.argtypes = [c_int, c_int]

libFFPlay.SaveFramebufferAsPPM.argtypes = [c_void_p, c_int, c_int, c_int]
