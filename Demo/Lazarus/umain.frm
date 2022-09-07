object frmMain: TfrmMain
  Left = 459
  Height = 441
  Top = 201
  Width = 1030
  Caption = 'lazPlayer'
  ClientHeight = 441
  ClientWidth = 1030
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
    Top = 394
    Width = 793
    Anchors = [akLeft, akRight, akBottom]
    PageSize = 0
    TabOrder = 0
  end
  object ButtonPlay: TButton
    Left = 808
    Height = 33
    Top = 7
    Width = 211
    Anchors = [akTop, akRight]
    Caption = 'Play'
    OnClick = ButtonPlayClick
    TabOrder = 1
  end
  object ButtonStop: TButton
    Left = 808
    Height = 34
    Top = 48
    Width = 211
    Anchors = [akTop, akRight]
    Caption = 'Stop'
    OnClick = ButtonStopClick
    TabOrder = 2
  end
  object ButtonPause: TButton
    Left = 808
    Height = 35
    Top = 88
    Width = 210
    Anchors = [akTop, akRight]
    Caption = 'Pause'
    OnClick = ButtonPauseClick
    TabOrder = 3
  end
  object Label1: TLabel
    Left = 8
    Height = 15
    Top = 426
    Width = 34
    Anchors = [akLeft, akBottom]
    Caption = 'Label1'
    Color = clDefault
    ParentColor = False
    Transparent = False
  end
  object Memo1: TMemo
    Left = 808
    Height = 287
    Top = 152
    Width = 214
    Anchors = [akTop, akRight, akBottom]
    Font.Height = -8
    Font.Name = 'Sans'
    Lines.Strings = (
      'Clean up and Build ...'
    )
    ParentFont = False
    ReadOnly = True
    ScrollBars = ssAutoBoth
    TabOrder = 4
  end
  object Label2: TLabel
    Left = 829
    Height = 15
    Top = 134
    Width = 84
    Anchors = [akTop, akRight]
    Caption = 'Debug Message'
    Color = clDefault
    ParentColor = False
    Transparent = False
  end
  object PanelYUV: TGroupBox
    Left = 9
    Height = 376
    Top = 7
    Width = 791
    Anchors = [akTop, akLeft, akRight, akBottom]
    ParentColor = False
    TabOrder = 5
    OnMouseDown = PanelYUVMouseDown
    OnMouseMove = PanelYUVMouseMove
    OnResize = PanelYUVResize
  end
  object ImageRGB: TImage
    Left = 8
    Height = 375
    Top = 8
    Width = 791
    AntialiasingMode = amOn
    Anchors = [akTop, akLeft, akRight, akBottom]
    Center = True
    OnClick = ImageRGBClick
    OnMouseDown = ImageRGBMouseDown
    Proportional = True
    Stretch = True
  end
  object Timer1: TTimer
    Enabled = False
    OnTimer = Timer1Timer
    Left = 600
    Top = 408
  end
  object OpenDialog: TOpenDialog
    Left = 704
    Top = 408
  end
end
