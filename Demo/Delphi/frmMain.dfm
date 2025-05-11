object Form1: TForm1
  Left = 0
  Top = 0
  Caption = 'FFPlay'
  ClientHeight = 470
  ClientWidth = 1004
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  DesignSize = (
    1004
    470)
  PixelsPerInch = 96
  TextHeight = 13
  object PanelBase: TPanel
    Left = 8
    Top = 8
    Width = 988
    Height = 404
    Anchors = [akLeft, akTop, akRight, akBottom]
    BevelOuter = bvLowered
    TabOrder = 0
    ExplicitHeight = 401
    DesignSize = (
      988
      404)
    object PanelYUV: TPanel
      Left = 8
      Top = 8
      Width = 969
      Height = 387
      Anchors = [akLeft, akTop, akRight, akBottom]
      Color = clGreen
      ParentBackground = False
      TabOrder = 0
      ExplicitHeight = 385
    end
  end
  object buttonPlay: TButton
    Left = 759
    Top = 441
    Width = 75
    Height = 25
    Anchors = [akTop, akRight, akBottom]
    Caption = 'PLAY'
    TabOrder = 1
  end
  object buttonPause: TButton
    Left = 840
    Top = 441
    Width = 75
    Height = 25
    Anchors = [akTop, akRight, akBottom]
    Caption = 'PAUSE'
    TabOrder = 2
  end
  object buttonStop: TButton
    Left = 921
    Top = 441
    Width = 75
    Height = 25
    Anchors = [akTop, akRight, akBottom]
    Caption = 'STOP'
    TabOrder = 3
  end
  object ScrollBar1: TScrollBar
    Left = 8
    Top = 418
    Width = 988
    Height = 15
    Anchors = [akLeft, akTop, akRight, akBottom]
    PageSize = 0
    TabOrder = 4
  end
  object OpenDialog1: TOpenDialog
    Left = 536
    Top = 440
  end
  object Timer1: TTimer
    Left = 616
    Top = 440
  end
end
