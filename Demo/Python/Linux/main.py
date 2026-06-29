import sys
import ctypes
import FFPlayLib

def on_exit(sender, code):
    print('\n ** Python player exited with code {} **'.format(code))

def on_info(sender, info_code, message):
    msg = message.decode('utf-8') if message else ''
    print(f'  [Player INFO] code={info_code}, message={msg}')

def on_resize(sender, width, height, is_original):
    print(f'  Video Resize: {width}x{height}, original={is_original}')

def on_status(sender, status):
    if status == FFPlayLib.FFP_PLAY_STATUS.FFP_STOP:
        print('\n-- The player has stopped --')
    elif status == FFPlayLib.FFP_PLAY_STATUS.FFP_PLAY:
        print('\n-- The player is in playing --')
    elif status == FFPlayLib.FFP_PLAY_STATUS.FFP_PAUSED:
        print('\n-- The player has paused --')
    elif status == FFPlayLib.FFP_PLAY_STATUS.FFP_RESUMED:
        print('\n-- The player has resumed --')
    else:
        print('\n-- The player is in unknown status --')


if __name__ == '__main__':
    if len(sys.argv) == 1:
        sys.argv.append('../test.avi')

    # Build argv
    args = [arg.encode("utf-8") for arg in sys.argv]
    argc = len(args)
    argv = (ctypes.c_char_p * argc)(*args)

    events = FFPlayLib.FFP_EVENTS()
    events.sender = None
    events.screenID = 0
    events.bRendererRGB = 0 # Yuv Image
    events.duration_in_us = 0
    events.current_in_s = 0
    events.ui_type = FFPlayLib.FFP_UI_TYPE.FFP_CLI
    events.event_exit = FFPlayLib.FFP_EVENT_EXIT(on_exit)
    events.event_info = FFPlayLib.FFP_EVENT_INFO(on_info)
    events.event_play_status = FFPlayLib.FFP_EVENT_PLAYSTATUS(on_status)
    events.event_video_resize = FFPlayLib.FFP_EVENT_VIDEORESIZE(on_resize)
    events.event_audio = FFPlayLib.FFP_EVENT_AUDIO(0)
    events.event_video = FFPlayLib.FFP_EVENT_VIDEO(0)
    events.event_refresh = FFPlayLib.FFP_EVENT_REFRESH(0)

    FFPlayLib.libFFPlay.multimedia_start_cli_player(argc, argv, ctypes.byref(events))
