program FFPlay_Delphi_2010;

uses
  Forms,
  uMain in 'uMain.pas' {frmMain},
  FFPlay in 'FFPlay.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
