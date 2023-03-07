object frmMain: TfrmMain
  Left = 258
  Height = 569
  Top = 0
  Width = 1048
  Caption = 'lazPlayer'
  ClientHeight = 569
  ClientWidth = 1048
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  Position = poScreenCenter
  LCLVersion = '7.8'
  object ScrollBar1: TScrollBar
    Left = 8
    Height = 13
    Top = 498
    Width = 1040
    Anchors = [akLeft, akRight, akBottom]
    PageSize = 0
    TabOrder = 0
  end
  object ButtonPlay: TButton
    Left = 616
    Height = 35
    Top = 520
    Width = 131
    Anchors = [akRight, akBottom]
    Caption = 'Play'
    OnClick = ButtonPlayClick
    TabOrder = 1
  end
  object ButtonStop: TButton
    Left = 904
    Height = 35
    Top = 520
    Width = 134
    Anchors = [akRight, akBottom]
    Caption = 'Stop'
    OnClick = ButtonStopClick
    TabOrder = 2
  end
  object Label1: TLabel
    Left = 9
    Height = 1
    Top = 547
    Width = 1
    Anchors = [akLeft, akBottom]
    Color = clDefault
    ParentColor = False
  end
  object PanelYUV: TGroupBox
    Left = 8
    Height = 493
    Top = 8
    Width = 1024
    Anchors = [akTop, akLeft, akRight, akBottom]
    Color = clInfoBk
    DockSite = True
    DoubleBuffered = False
    ParentBackground = False
    ParentColor = False
    ParentDoubleBuffered = False
    TabOrder = 3
    OnDblClick = PanelYUVDblClick
    OnMouseDown = PanelYUVMouseDown
    OnMouseMove = PanelYUVMouseMove
    OnResize = PanelYUVResize
  end
  object ImageRGB: TImage
    Left = 9
    Height = 493
    Top = 8
    Width = 1029
    AntialiasingMode = amOn
    Anchors = [akTop, akLeft, akRight, akBottom]
    Center = True
    OnDblClick = ImageRGBDblClick
    OnResize = ImageRGBResize
    Proportional = True
    Stretch = True
    StretchInEnabled = False
  end
  object ButtonPause: TButton
    Left = 760
    Height = 35
    Top = 520
    Width = 129
    Anchors = [akRight, akBottom]
    Caption = 'Pause'
    OnClick = ButtonPauseClick
    TabOrder = 4
  end
  object Memo1: TMemo
    Left = 8
    Height = 498
    Top = 3
    Width = 367
    Anchors = [akTop, akLeft, akBottom]
    Font.CharSet = ANSI_CHARSET
    Font.Height = -11
    Font.Pitch = fpVariable
    Font.Quality = fqDraft
    Lines.Strings = (
      'Memo1'
    )
    ParentFont = False
    ScrollBars = ssBoth
    TabOrder = 5
    Visible = False
  end
  object Timer1: TTimer
    Enabled = False
    OnTimer = Timer1Timer
    Left = 56
    Top = 424
  end
  object OpenDialog: TOpenDialog
    Left = 96
    Top = 424
  end
end
