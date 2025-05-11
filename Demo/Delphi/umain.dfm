object frmMain: TfrmMain
  Left = 0
  Top = 0
  Caption = 'FFPlay'
  ClientHeight = 451
  ClientWidth = 1004
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  Position = poScreenCenter
  OnCloseQuery = FormCloseQuery
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnResize = FormResize
  DesignSize = (
    1004
    451)
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 430
    Width = 3
    Height = 13
    Anchors = [akLeft, akBottom]
  end
  object PanelBase: TPanel
    Left = 8
    Top = 8
    Width = 988
    Height = 385
    Anchors = [akLeft, akTop, akRight, akBottom]
    BevelOuter = bvLowered
    TabOrder = 0
    DesignSize = (
      988
      385)
    object PanelYUV: TPanel
      Left = 8
      Top = 8
      Width = 969
      Height = 368
      Anchors = [akLeft, akTop, akRight, akBottom]
      Color = clGreen
      ParentBackground = False
      TabOrder = 0
      OnDblClick = PanelYUVDblClick
      DesignSize = (
        969
        368)
      object Memo1: TMemo
        Left = 8
        Top = 8
        Width = 449
        Height = 350
        Anchors = [akLeft, akTop, akRight, akBottom]
        TabOrder = 0
        Visible = False
      end
    end
  end
  object ButtonPlay: TButton
    Left = 759
    Top = 422
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'PLAY'
    TabOrder = 1
    OnClick = ButtonPlayClick
  end
  object ButtonPause: TButton
    Left = 840
    Top = 422
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'PAUSE'
    TabOrder = 2
    OnClick = ButtonPauseClick
  end
  object ButtonStop: TButton
    Left = 921
    Top = 422
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = 'STOP'
    TabOrder = 3
    OnClick = ButtonStopClick
  end
  object ScrollBar1: TScrollBar
    Left = 8
    Top = 399
    Width = 988
    Height = 17
    Anchors = [akLeft, akRight, akBottom]
    PageSize = 0
    TabOrder = 4
  end
  object OpenDialog: TOpenDialog
    Left = 544
    Top = 352
  end
  object Timer1: TTimer
    OnTimer = Timer1Timer
    Left = 656
    Top = 352
  end
end
