import wx
import sys
import ctypes
from ctypes import *
import FFPlayLibWrapper

def on_exit(sender, code):
    print('\n-->> ** Python player exited with code {} **'.format(code))

def on_info(sender, info_code, message):
    msg = message.decode('utf-8') if message else ''
    print(f'-->> [Player INFO] code={info_code}, message={msg}')

def on_refresh(sender):
    wx.GetApp().Yield()  # Application.DoEvent()

def on_resize(sender, width, height, is_original):
    print(f'-->> Video Resize: {width}x{height}, original={is_original}')

def on_status(sender, status):
    win = wx.FindWindowById(sender)
    if isinstance(win, frmMain):
       form = win
       if status == FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_STOP:
          form.invalidate_screen()
       form.currentStatus = status

class frmMain(wx.Frame):

    class PLAYINGMODE(c_int):
        STOP = 0
        PLAY = 1
        PAUSE = 2
        RESUME = 3

    formID = 0
    elements = FFPlayLibWrapper.FFPLAY_ELEMENTS()
    yuvID  = 0
    currentStatus = FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_STOP
    currentMode = PLAYINGMODE.STOP
    playPos = FFPlayLibWrapper.FFPLAY_POS()

    def set_playing_mode(self, playingmode):
        match playingmode:
            case self.PLAYINGMODE.STOP:
                self.mButtonPlay.Enable(True)
                self.mButtonStop.Enable(False)
                self.mButtoonPause.Enable(False)
                self.mButtoonPause.SetLabel("PAUSE")
                self.mTimer.Stop()

            case self.PLAYINGMODE.PLAY:
                self.mButtonPlay.Enable(False)
                self.mButtonStop.Enable(True)
                self.mButtoonPause.Enable(True)
                self.mButtoonPause.SetLabel("PAUSE")
                self.mTimer.Start(1000)

            case self.PLAYINGMODE.PAUSE:
                self.mButtonPlay.Enable(False)
                self.mButtonStop.Enable(True)
                self.mButtoonPause.Enable(True)
                self.mButtoonPause.SetLabel("RESUME")
                self.mTimer.Stop()

            case self.PLAYINGMODE.RESUME:
                self.mButtonPlay.Enable(False)
                self.mButtonStop.Enable(True)
                self.mButtoonPause.Enable(True)
                self.mButtoonPause.SetLabel("PAUSE")
                self.mTimer.Start(1000)

        self.currentMode = playingmode

    def invalidate_screen(self):
        if wx.IsMainThread():
            # Already on GUI thread, call directly
            self.mPanelYUV.Refresh()
            self.mPanelYUV.Update()
            self.set_playing_mode(self.PLAYINGMODE.STOP)
        else:
            # On worker thread, must marshal to GUI thread
            wx.CallAfter(self.mPanelYUV.Refresh)
            wx.CallAfter(self.mPanelYUV.Update)
            wx.CallAfter(self.set_playing_mode, self.PLAYINGMODE.STOP)

    def __init__(self, parent=None):
        super().__init__(
            parent,
            title="Python FFPlay (GUI)",
            size=(1073, 468),
            style=wx.DEFAULT_FRAME_STYLE,
        )
        self.Centre(wx.BOTH)
        self.formID = self.GetId()
        self._init_components()
        self._bind_events()
        self.set_playing_mode(self.PLAYINGMODE.STOP)

    def _init_components(self):
        self.panel = wx.Panel(self)

        # Video display panel (green background)
        self.mPanelYUV = wx.Panel(self.panel)
        self.mPanelYUV.SetBackgroundColour(wx.Colour(34, 139, 34))  # ForestGreen

        # Playback buttons (bottom-right)
        self.mButtonPlay = wx.Button(self.panel, label="PLAY")
        self.mButtoonPause = wx.Button(self.panel, label="PAUSE")
        self.mButtonStop = wx.Button(self.panel, label="STOP")

        # Seek scrollbar
        self.mScrollBar = wx.ScrollBar(self.panel, style=wx.SB_HORIZONTAL)
        self.mScrollBar.SetScrollbar(0, 1, 1000, 1)

        # Position label
        self.mLabelPos = wx.StaticText(self.panel, label="")

        # Timer (1 second interval, equivalent to mTimer)
        self.mTimer = wx.Timer(self)

        self._layout()

    def _layout(self):
        # Bottom row: [TEST Screen] [Run CLI] [scrollbar + label] [PLAY] [PAUSE] [STOP]
        bottom_sizer = wx.BoxSizer(wx.HORIZONTAL)

        scroll_sizer = wx.BoxSizer(wx.VERTICAL)
        scroll_sizer.Add(self.mScrollBar, 0, wx.EXPAND)
        scroll_sizer.Add(self.mLabelPos, 0, wx.TOP, 2)

        bottom_sizer.Add(scroll_sizer, 1, wx.ALIGN_CENTER_VERTICAL | wx.RIGHT, 12)
        bottom_sizer.Add(self.mButtonPlay, 0, wx.ALIGN_CENTER_VERTICAL | wx.RIGHT, 4)
        bottom_sizer.Add(self.mButtoonPause, 0, wx.ALIGN_CENTER_VERTICAL | wx.RIGHT, 4)
        bottom_sizer.Add(self.mButtonStop, 0, wx.ALIGN_CENTER_VERTICAL)

        # Main sizer: video panel fills space, controls at bottom
        main_sizer = wx.BoxSizer(wx.VERTICAL)
        main_sizer.Add(self.mPanelYUV, 1, wx.EXPAND | wx.ALL, 12)
        main_sizer.Add(bottom_sizer, 0, wx.EXPAND | wx.LEFT | wx.RIGHT | wx.BOTTOM, 12)

        self.yuvID = self.mPanelYUV.GetHandle()
        self.panel.SetSizer(main_sizer)

    def _bind_events(self):
        self.mButtonPlay.Bind(wx.EVT_BUTTON, self.mButtonPlay_Click)
        self.mButtoonPause.Bind(wx.EVT_BUTTON, self.mButtoonPause_Click)
        self.mButtonStop.Bind(wx.EVT_BUTTON, self.mButtonStop_Click)
        self.mScrollBar.Bind(wx.EVT_SCROLL, self.mScrollBar_Scroll)
        self.Bind(wx.EVT_TIMER, self.mTimer_Tick, self.mTimer)
        self.Bind(wx.EVT_CLOSE, self.frmMain_FormClosing)
        self.Bind(wx.EVT_SIZE, self.frmMain_ResizeEnd)

    # --- Event handlers (implement your logic here) ---
    def mButtonPlay_Click(self, event):
        wildcard = (
            "Media files (*.mp4;*.mkv;*.avi;*.mov)|*.mp4;*.mkv;*.avi;*.mov|"
            "All files (*.*)|*.*"
        )
        with wx.FileDialog(
                self,
                message="Open media file",
                wildcard=wildcard,
                style=wx.FD_OPEN | wx.FD_FILE_MUST_EXIST,
        ) as dlg:
            if dlg.ShowModal() == wx.ID_OK:
               path = dlg.GetPath()  # full path string
               if len(sys.argv) == 1:
                  sys.argv.append(path)

               # Build argv
               args = [arg.encode("utf-8") for arg in sys.argv]
               argc = len(args)
               argv = (ctypes.c_char_p * argc)(*args)

               self.elements.sender = self.formID
               self.elements.yuvHandle = self.yuvID
               self.elements.eventExit = FFPlayLibWrapper.FFP_EVENT_EXIT(on_exit)
               self.elements.eventInfo = FFPlayLibWrapper.FFP_EVENT_INFO(on_info)
               self.elements.eventStatus = FFPlayLibWrapper.FFP_EVENT_PLAYSTATUS(on_status)
               self.elements.eventResize = FFPlayLibWrapper.FFP_EVENT_VIDEORESIZE(on_resize)
               self.elements.eventAudio = FFPlayLibWrapper.FFP_EVENT_AUDIO(0)
               self.elements.eventVideo = FFPlayLibWrapper.FFP_EVENT_VIDEO(0)
               self.elements.eventRefresh = FFPlayLibWrapper.FFP_EVENT_REFRESH(on_refresh)

               rtn = FFPlayLibWrapper.WrapperDLL.Setup_GUI_FFPlayer(argc, argv, ctypes.byref(self.elements))
               if rtn == 0:
                  self.frmMain_ResizeEnd(None)
                  self.set_playing_mode(self.PLAYINGMODE.PLAY)
                  FFPlayLibWrapper.WrapperDLL.Start_FFPlayer()

    def mButtoonPause_Click(self, event):
        if self.currentMode == self.PLAYINGMODE.PAUSE:
           self.set_playing_mode(self.PLAYINGMODE.RESUME)
        else:
           self.set_playing_mode(self.PLAYINGMODE.PAUSE)

        FFPlayLibWrapper.WrapperDLL.PauseResume_FFPlayer()

    def mButtonStop_Click(self, event):
        FFPlayLibWrapper.WrapperDLL.Stop_FFPlayer()

        self.set_playing_mode(self.PLAYINGMODE.STOP)

    def mScrollBar_Scroll(self, event):
        if self.currentStatus != FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_STOP:
            if event.GetEventType() == wx.wxEVT_SCROLL_CHANGED:
               self.mTimer.Stop()
               pos = self.mScrollBar.GetThumbPosition()
               FFPlayLibWrapper.WrapperDLL.SeekPositionInSecond_FFPlayer(pos)
               self.mTimer.Start(1000)

        event.Skip()    # DO NOT REMOVE

    def mTimer_Tick(self, event):
        if self.currentStatus != FFPlayLibWrapper.FFP_PLAY_STATUS.FFP_PLAY:
           return
        else:
           FFPlayLibWrapper.WrapperDLL.GetPositionInSecond_FFPlayer(byref(self.playPos))
           max = self.playPos.max_duration
           curr = self.playPos.current_position
           self.mScrollBar.SetScrollbar(curr, 1, max, 1)
           self.mLabelPos.SetLabel(f' Playing Position:  {curr} / {max}')

    def frmMain_FormClosing(self, event):
        self.mTimer.Stop()
        event.Skip()    # DO NOT REMOVE

    def frmMain_ResizeEnd(self, event):
        size = self.mPanelYUV.GetSize()
        w = size.Width
        h = size.Height
        FFPlayLibWrapper.WrapperDLL.Resize_GUI_Screen(w,h)
        if event != None:
           event.Skip()    # DO NOT REMOVE

    def hello_world(self):
        print('**** Hello World! ****')

if __name__ == "__main__":
    app = wx.App(False)
    frame = frmMain()
    frame.Show()
    app.MainLoop()