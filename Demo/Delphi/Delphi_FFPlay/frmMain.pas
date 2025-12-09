unit frmMain;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  TForm1 = class(TForm)
    PanelBase: TPanel;
    buttonPlay: TButton;
    buttonPause: TButton;
    buttonStop: TButton;
    PanelYUV: TPanel;
    OpenDialog1: TOpenDialog;
    Timer1: TTimer;
    ScrollBar1: TScrollBar;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

end.
