import sys
import ctypes
import FFPlayLibWrapper

def on_exit(sender, code):
    print('\n ** Python player exited with code {} **'.format(code))

def on_info(sender, info_code, message):
    msg = message.decode('utf-8') if message else ''
    print(f'  [Player INFO] code={info_code}, message={msg}')

def on_resize(sender, width, height, is_original):
    print(f'  Video Resize: {width}x{height}, original={is_original}')

def on_status(sender, status):
    if status == FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_STOP:
        print('\n-- The player has stopped --')
    elif status == FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_PLAY:
        print('\n-- The player is in playing --')
    elif status == FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_PAUSED:
        print('\n-- The player has paused --')
    elif status == FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_RESUMED:
        print('\n-- The player has resumed --')
    else:
        print('\n-- The player is in unknown status --')

if __name__ == "__main__":
    if len(sys.argv) == 1:
        sys.argv.append('../test.avi')

    # Build argv
    args = [arg.encode("utf-8") for arg in sys.argv]
    argc = len(args)
    argv = (ctypes.c_char_p * argc)(*args)

    elements = FFPlayLibWrapper.FFPLAY_ELEMENTS()
    try:
        elements.sender       = None
        elements.yuvHandle    = 0
        elements.eventExit    = FFPlayLibWrapper.FFP_EVENT_EXIT(on_exit)
        elements.eventInfo    = FFPlayLibWrapper.FFP_EVENT_INFO(on_info)
        elements.eventStatus  = FFPlayLibWrapper.FFP_EVENT_PLAYSTATUS(on_status)
        elements.eventResize  = FFPlayLibWrapper.FFP_EVENT_VIDEORESIZE(on_resize)
        elements.eventAudio   = FFPlayLibWrapper.FFP_EVENT_AUDIO(0)
        elements.eventVideo   = FFPlayLibWrapper.FFP_EVENT_VIDEO(0)
        elements.eventRefresh = FFPlayLibWrapper.FFP_EVENT_REFRESH(0)

        FFPlayLibWrapper.WrapperDLL.Start_CLI_FFPlayer(argc, argv, ctypes.byref(elements))
    except:
        print('** Python player has failed to load the DLL or play the file **')

    input("\nPress Enter to quit...")