object frmMain: TfrmMain
  Left = 324
  Height = 441
  Top = 217
  Width = 1030
  Caption = 'lazPlayer'
  ClientHeight = 441
  ClientWidth = 1030
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnResize = FormResize
  Position = poScreenCenter
  LCLVersion = '6.5'
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
    Top = 8
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
    Height = 17
    Top = 424
    Width = 45
    Anchors = [akLeft, akBottom]
    Caption = 'Label1'
    ParentColor = False
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
      'Memo1'
    )
    ParentFont = False
    ReadOnly = True
    ScrollBars = ssAutoBoth
    TabOrder = 4
  end
  object Label2: TLabel
    Left = 808
    Height = 17
    Top = 134
    Width = 105
    Anchors = [akTop, akRight]
    Caption = 'Debug Message'
    ParentColor = False
  end
  object PanelYUV: TGroupBox
    Left = 8
    Height = 384
    Top = 8
    Width = 792
    Anchors = [akTop, akLeft, akRight, akBottom]
    Color = clInfoBk
    ParentColor = False
    TabOrder = 5
    OnMouseMove = PanelYUVMouseMove
    OnResize = PanelYUVResize
  end
  object ImageRGB: TImage
    Left = 8
    Height = 385
    Top = 6
    Width = 791
    Anchors = [akTop, akLeft, akRight, akBottom]
    Center = True
    OnClick = ImageRGBClick
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
