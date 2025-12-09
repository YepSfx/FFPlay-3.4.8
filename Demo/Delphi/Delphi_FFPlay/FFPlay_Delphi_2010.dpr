program FFPlay_Delphi_2010;

uses
  Math,
  Forms,
  uMain in 'uMain.pas' {frmMain},
  FFPlay in 'FFPlay.pas';

{$R *.res}

begin
  SetExceptionMask([exInvalidOp, exDenormalized, exZeroDivide,
                                        exOverflow, exUnderflow, exPrecision]);
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
