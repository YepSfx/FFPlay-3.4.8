import ctypes
from ctypes import *

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
# STRUCT: FFPLAY_ELEMENTS
# =========================
class FFPLAY_ELEMENTS(Structure):
    _fields_ = [
        ("sender", c_void_p),
        ("yuvHandle", c_uint),
        ("eventInfo", FFP_EVENT_INFO),
        ("eventExit", FFP_EVENT_EXIT),
        ("eventAudio", FFP_EVENT_AUDIO),
        ("eventVideo", FFP_EVENT_VIDEO),
        ("eventResize", FFP_EVENT_VIDEORESIZE),
        ("eventStatus", FFP_EVENT_PLAYSTATUS),
        ("eventRefresh", FFP_EVENT_REFRESH),
    ]

# =========================
# Enum: FFP_PLAY_STATUS
# =========================
class FFP_PLAY_STATUS(c_int):
    FFP_STOP = 0
    FFP_PLAY = 1
    FFP_PAUSED = 2
    FFP_RESUMED = 3

# =========================
# STRUCT: FFPLAY_POS
# =========================
class FFPLAY_POS(Structure):
    _fields_ = [
        ("current_position", c_uint),
        ("max_duration", c_uint),
    ]

# =========================
# Load DLL
# =========================
WrapperDLL = ctypes.CDLL("libFFPlayWrapper.so")

# int Start_CLI_FFPlayer(int argc, char **argv, FFPLAY_ELEMENTS* playElements);
WrapperDLL.Start_CLI_FFPlayer.argtypes = [c_int, POINTER(c_char_p), POINTER(FFPLAY_ELEMENTS)]
WrapperDLL.Start_CLI_FFPlayer.restype = c_int

# int Setup_GUI_FFPlayer(int argc, char **argv, FFPLAY_ELEMENTS* playElements);
WrapperDLL.Setup_GUI_FFPlayer.argtypes = [c_int, POINTER(c_char_p), POINTER(FFPLAY_ELEMENTS)]
WrapperDLL.Setup_GUI_FFPlayer.restype = c_int

# int SetupAndStart_GUI_FFPlayer(int argc, char** argv, FFPLAY_ELEMENTS* playElements);
WrapperDLL.SetupAndStart_GUI_FFPlayer.argtypes = [c_int, POINTER(c_char_p), POINTER(FFPLAY_ELEMENTS)]
WrapperDLL.SetupAndStart_GUI_FFPlayer.restype = c_int

# void PauseResume_FFPlayer();
WrapperDLL.PauseResume_FFPlayer.argtypes = []
WrapperDLL.PauseResume_FFPlayer.restype = None

# void Start_FFPlayer();
WrapperDLL.Start_FFPlayer.argtypes = []
WrapperDLL.Start_FFPlayer.restype = None

# void Stop_FFPlayer();
WrapperDLL.Stop_FFPlayer.argtypes = []
WrapperDLL.Stop_FFPlayer.restype = None

# void Resize_GUI_Screen(int w, int h);
WrapperDLL.Resize_GUI_Screen.argtypes = [c_int, c_int]

# void Test_GUI_Screen(int xWinID, int holdTime);
WrapperDLL.Test_GUI_Screen.argtypes = [c_int, c_int]

# void SeekPositionInSecond_FFPlayer(int posInSec);
WrapperDLL.SeekPositionInSecond_FFPlayer.argtypes = [c_int]

# void GetPositionInSecond_FFPlayer(FFPLAY_POS* posInfo);
WrapperDLL.GetPositionInSecond_FFPlayer.argtypes = [POINTER(FFPLAY_POS)]
WrapperDLL.GetPositionInSecond_FFPlayer.restype = None



