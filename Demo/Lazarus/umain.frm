object frmMain: TfrmMain
  Left = 330
  Height = 524
  Top = 215
  Width = 1105
  Caption = 'lazPlayer'
  ClientHeight = 524
  ClientWidth = 1105
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnResize = FormResize
  OnWindowStateChange = FormWindowStateChange
  Position = poScreenCenter
  LCLVersion = '7.8'
  object ScrollBar1: TScrollBar
    Left = 8
    Height = 13
    Top = 453
    Width = 1097
    Anchors = [akLeft, akRight, akBottom]
    PageSize = 0
    TabOrder = 0
  end
  object ButtonPlay: TButton
    Left = 456
    Height = 35
    Top = 477
    Width = 211
    Anchors = [akRight, akBottom]
    Caption = 'Play'
    OnClick = ButtonPlayClick
    TabOrder = 1
  end
  object ButtonStop: TButton
    Left = 888
    Height = 35
    Top = 477
    Width = 211
    Anchors = [akRight, akBottom]
    Caption = 'Stop'
    OnClick = ButtonStopClick
    TabOrder = 2
  end
  object Label1: TLabel
    Left = 9
    Height = 1
    Top = 502
    Width = 1
    Anchors = [akLeft, akBottom]
    Color = clDefault
    ParentColor = False
  end
  object PanelYUV: TGroupBox
    Left = 8
    Height = 448
    Top = 8
    Width = 1091
    Anchors = [akTop, akLeft, akRight, akBottom]
    Color = clInfoBk
    ParentBackground = False
    ParentColor = False
    TabOrder = 3
    OnDblClick = PanelYUVDblClick
    OnMouseDown = PanelYUVMouseDown
    OnMouseMove = PanelYUVMouseMove
    OnResize = PanelYUVResize
  end
  object ImageRGB: TImage
    Left = 9
    Height = 448
    Top = 8
    Width = 1090
    AntialiasingMode = amOn
    Anchors = [akTop, akLeft, akRight, akBottom]
    Center = True
    OnClick = ImageRGBClick
    OnDblClick = ImageRGBDblClick
    OnMouseDown = ImageRGBMouseDown
    OnResize = ImageRGBResize
    Proportional = True
    Stretch = True
    StretchInEnabled = False
  end
  object ButtonPause: TButton
    Left = 672
    Height = 35
    Top = 477
    Width = 209
    Anchors = [akRight, akBottom]
    Caption = 'Pause'
    OnClick = ButtonPauseClick
    TabOrder = 4
  end
  object Memo1: TMemo
    Left = 8
    Height = 453
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
