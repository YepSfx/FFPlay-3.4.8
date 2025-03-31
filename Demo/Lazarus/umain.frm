object frmMain: TfrmMain
  Left = 258
  Height = 569
  Top = 0
  Width = 1048
  Caption = 'lazPlayer'
  ClientHeight = 569
  ClientWidth = 1048
  Position = poScreenCenter
  LCLVersion = '8.6'
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnResize = FormResize
  object ScrollBar1: TScrollBar
    Left = 9
    Height = 13
    Top = 501
    Width = 1029
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
    TabOrder = 1
    OnClick = ButtonPlayClick
  end
  object ButtonStop: TButton
    Left = 904
    Height = 35
    Top = 520
    Width = 134
    Anchors = [akRight, akBottom]
    Caption = 'Stop'
    TabOrder = 2
    OnClick = ButtonStopClick
  end
  object Label1: TLabel
    Left = 9
    Height = 1
    Top = 547
    Width = 1
    Anchors = [akLeft, akBottom]
    ParentColor = False
  end
  object PanelYUV: TGroupBox
    Left = 9
    Height = 493
    Top = 8
    Width = 1029
    Anchors = [akTop, akLeft, akRight, akBottom]
    DoubleBuffered = False
    ParentBackground = False
    ParentBidiMode = False
    ParentColor = False
    ParentDoubleBuffered = False
    ParentFont = False
    ParentShowHint = False
    TabOrder = 3
    OnDblClick = PanelYUVDblClick
    OnMouseDown = PanelYUVMouseDown
    OnMouseMove = PanelYUVMouseMove
  end
  object ImageRGB: TImage
    Left = 9
    Height = 493
    Top = 8
    Width = 1029
    AntialiasingMode = amOn
    Anchors = [akTop, akLeft, akRight, akBottom]
    AutoSize = True
    Center = True
    Proportional = True
    Stretch = True
    Visible = False
    OnDblClick = ImageRGBDblClick
    OnResize = ImageRGBResize
  end
  object ButtonPause: TButton
    Left = 760
    Height = 35
    Top = 520
    Width = 129
    Anchors = [akRight, akBottom]
    Caption = 'Pause'
    TabOrder = 4
    OnClick = ButtonPauseClick
  end
  object Memo1: TMemo
    Left = 16
    Height = 429
    Top = 40
    Width = 552
    Anchors = [akTop, akLeft, akRight, akBottom]
    Font.CharSet = ANSI_CHARSET
    Font.Height = -11
    Font.Pitch = fpVariable
    Font.Quality = fqDraft
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
