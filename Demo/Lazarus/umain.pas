unit umain;

{$mode objfpc}{$H+}

//{$DEFINE DEF_RGB}

interface

uses
  {$ifdef fpc}
  LCLType,
  {$endif}
  Classes, SysUtils, FileUtil, Forms, Controls, Graphics, Dialogs, ExtCtrls,
  StdCtrls, LCLIntf,  LMessages, LazLogger, FFPlay;

const WM_USER_RESIZE          = WM_USER + $01;
      WM_USER_REFRESH         = WM_USER + $02;
      WM_USER_STATUS          = WM_USER + $03;
      WM_USER_EVENT_REFRESH   = WM_USER + $04;
      WM_USER_EVENT_LOG       = WM_USER + $05;
type
  { TfrmMain }
  TfrmMain = class(TForm)
    ButtonPause : TButton;
    ButtonStop  : TButton;
    ButtonPlay  : TButton;
    ImageRGB    : TImage;
    Label1      : TLabel;
    Memo1       : TMemo;
    PanelYUV    : TGroupBox;
    OpenDialog  : TOpenDialog;
    ScrollBar1  : TScrollBar;
    Timer1      : TTimer;
    procedure ButtonPauseClick(Sender: TObject);
    procedure ButtonPlayClick(Sender: TObject);
    procedure ButtonStopClick(Sender: TObject);
    procedure FormCloseQuery(Sender: TObject; var CanClose: boolean);
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure FormResize(Sender: TObject);
    procedure ImageRGBClick(Sender: TObject);
    procedure ImageRGBDblClick(Sender: TObject);
    procedure ImageRGBResize(Sender: TObject);
    procedure PanelYUVDblClick(Sender: TObject);
    procedure PanelYUVMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure PanelYUVMouseMove(Sender: TObject; Shift: TShiftState; X,
      Y: Integer);
    procedure Timer1Timer(Sender: TObject);
  private
    { private declarations }
    FCurrentTime_Sec   : double;
    FDurationTime_mSec : Int64;
    FresImage          : TBitmap;
    sti_events         : TFFP_EVENTS;
    FInitPanelWidth    : Integer;
    FInitPanelHeight   : Integer;
  public
    { public declarations }
    procedure OnResizeScreen(var Msg : TLMessage); message WM_USER_RESIZE;
    procedure OnRefreshScreen(var Msg : TLMessage); message WM_USER_REFRESH;
    procedure OnStatus(var Msg : TLMessage); message WM_USER_STATUS;
    procedure OnRefreshEvent(var Msg : TLMessage); message WM_USER_EVENT_REFRESH;
    procedure OnUserLog(var Msg : TLMessage); message WM_USER_EVENT_LOG;
    procedure stopPlaying();
    procedure updateScreen( Buffer : pointer ; w,h,Bpp : Integer);
    procedure updateTime(Current : double);
  end;

var
  frmMain: TfrmMain;

implementation

{$R *.frm}

{ TfrmMain }

uses LCLProc
     {$IFDEF DEF_OUTPUT_WIN}
     ;
     {$ELSE}
     ,Gtk2, Gdk2, Gdk2x, Glib2, xlib, x, Gtk2Proc;
     {$ENDIF}

type TVideoData = record
     Buffer : Pointer;
     w      : Integer;
     h      : Integer;
     Bpp    : Integer;
end;
type PVideoData = ^TVideoData;

type TPlayData = record
  w            : Integer;
  h            : Integer;
  Bpp          : Integer;
  status       : TFFP_PLAY_STATUS;
end;
type PPlayData = ^TPlayData;

var RGBBuffer  : pointer  = nil;

procedure PrintDebugMessage( msg : String );
  Var TS   : TTimeStamp;
      DT   : TDateTime;
      dMsg : String;
      pMsg : PChar;
begin
  TS:=DateTimeToTimeStamp(Now);
  DT:=TimeStampToDateTime(TS);
  dMsg := DateTimeToStr(DT) + ': ' + msg;
  pMsg := StrNew(PChar(dMsg));
  PostMessage( frmMain.Handle, WM_USER_EVENT_LOG, PtrInt(pMsg), 0);
end;

procedure SendResize(w,h : Integer ; src : String);
  var dMsg : String;
begin
  dMsg := Format('[SendResize() set new size %d %d (%s)]',[w,h,src]);
  PrintDebugMessage(dMsg);
  multimedia_resize_screen(w,h);
end;

procedure EventRefreshHandler( sender : pointer ) ; cdecl;
  var handle : HWND;
begin
  handle := TfrmMain(sender).Handle;
  PostMessage( handle, WM_USER_EVENT_REFRESH, 0, 0 );
end;

procedure EventExit( sender : pointer ; exitCode : Integer ) ; cdecl;
   var msg : String;
       handle : HWND;
       pPlay  : PPlayData;
begin
   msg := Format('[Exit signal : %d]',[exitCode]);
   PrintDebugMessage( msg );
   GetMem( pPlay , sizeof(TPlayData) );
   FillChar( pPlay^,Sizeof(TPlayData), 0);
   pPlay^.status := FFP_STOP;
   PostMessage( handle, WM_USER_STATUS, PtrInt(pPlay), 0 );
end;

procedure EventInfo( sender : pointer; infoCode : Integer ; msg : PFFP_CHAR ) ; cdecl;
  var dbgMsg : String;
begin
  if (infocode <> Integer(FFP_INFO_DEBUG)) then
  begin
    dbgMsg := String(msg);
    PrintDebugMessage(dbgMsg);
  end;
end;

procedure EventAudio( sender : pointer ; buff : PByte ; BuffLenInByte : Integer ) ; cdecl;
begin
end;

procedure EventVideo( sender : pointer ; videoData : Pointer ; isRGB : Integer ) ; cdecl;
  var handle : HWND;
      msg    : String;
      pPlay  : PPlayData;
      pRGB   : PFFP_RGB_DATA;
      pYUV   : PFFP_YUV_DATA;
      pSrc, pDst : PByte;
      size,w,h   : Integer;
begin
     handle := TfrmMain(sender).Handle;
     pRGB := PFFP_RGB_DATA(videoData);

     pSrc := pRGB^.pixels;
     pDst := RGBBuffer;
     size := pRGB^.width * pRGB^.height * pRGB^.bpp;
     Move( pSrc^, pDst^, size );
     GetMem( pPlay , sizeof(TPlayData) );
     pPlay^.w   := pRGB^.width;
     pPlay^.h   := pRGB^.height;
     pPlay^.Bpp := pRGB^.bpp;
     PostMessage( handle, WM_USER_REFRESH, PtrInt(pPlay), isRGB );
end;

procedure EventResize(sender : pointer ; w, h, isOriginal : Integer) ; cdecl;
  var msg    : String;
      handle : HWND;
      pPlay  : PPlayData;
begin
     if isOriginal = 1 then
     begin
       msg := Format('[Original Video Size: %d %d]',[w,h]);
       PrintDebugMessage(msg);
       handle := TfrmMain(sender).Handle;
       GetMem( pPlay , sizeof(TPlayData) );
       FillChar( pPlay^,Sizeof(TPlayData), 0);
       pPlay^.w := w;
       pPlay^.h := h;
       PostMessage( handle, WM_USER_RESIZE, PtrInt(pPlay), 1 );
     end else
     begin
       msg := Format('[Resize Event] %d %d',[w,h]);
       PrintDebugMessage(msg);
       handle := TfrmMain(sender).Handle;
       GetMem( pPlay , sizeof(TPlayData) );
       FillChar( pPlay^,Sizeof(TPlayData), 0);
       pPlay^.w := w;
       pPlay^.h := h;
       PostMessage( handle, WM_USER_RESIZE, PtrInt(pPlay), 0 );
     end;
end;

procedure TfrmMain.OnUserLog(var Msg : TLMessage);
  var pMsg : PChar;
begin
  pMsg := PChar(Msg.WParam);
  Memo1.Lines.Add(String(pMsg));
  StrDispose(pMsg);
end;

procedure TfrmMain.OnRefreshEvent(var Msg: TLMessage);
begin
  //Application.ProcessMessages();
end;

procedure EventPlayStatus( sender : pointer ; status : TFFP_PLAY_STATUS ) ; cdecl;
  var handle : HWND;
      msg    : String;
      pPlay  : PPlayData;
begin
     handle := TfrmMain(sender).Handle;

     msg := Format('[Status event: %d]',[Integer(status)]);
     PrintDebugMessage((msg));

     GetMem( pPlay , sizeof(TPlayData) );
     FillChar( pPlay^,Sizeof(TPlayData), 0);
     pPlay^.status := status;
     PostMessage( handle, WM_USER_STATUS, PtrInt(pPlay), 0 );
end;

procedure TfrmMain.OnStatus(var Msg: TLMessage);
  var pPlay : PPlayData;
      pVid  : PFFP_VID_PARAMS;
      dMsg  : String;
      x,y   : Integer;
begin
  pPlay := PPlayData(Msg.WParam);

  case pPlay^.status of
  FFP_PAUSED: begin
                   ButtonPause.Caption := 'Resume';
                   dMsg := '[Paused Status]';
                   PrintDebugMessage( dMsg );
              end;
  FFP_RESUMED:begin
                   ButtonPause.Caption := 'Pause';
                   dMsg := '[Resumed Status]';
                   PrintDebugMessage( dMsg );
              end;
  FFP_STOP:   begin
                   PanelYUV.Visible:= True;
                   ImageRGB.Visible:= False;
                   StopPlaying();
                   dMsg := '[Stop Status]';
                   PrintDebugMessage( dMsg );
                   PanelYUV.Invalidate();
              end;
  FFP_PLAY:   begin
                   Timer1.Enabled     := True;
                   ButtonPlay.Enabled := False;
                   ButtonStop.Enabled := True;
                   ButtonPause.Enabled:= True;
{$IFNDEF DEF_RGB}
                   PanelYUV.Width := 0;
                   PanelYUV.Height:= 0;
                   PanelYUV.Width := FInitPanelWidth;
                   PanelYUV.Height:= FInitPanelHeight;
{$ELSE}
{$ENDIF}

                   dMsg := '[Play Status]';
                   PrintDebugMessage( dMsg );
            end
  else begin
       end;
  end;

  FreeMem(pPlay);
end;

procedure TfrmMain.OnRefreshScreen(var Msg : TLMessage);
  var pPlay : PPlayData;
      dMsg   : String;
      w, h, bpp, isRGB : Integer;
begin
{$IFNDEF DEF_OUTPUT_WIN}
  isRGB := Msg.LParam;
  FresImage.PixelFormat := pf24Bit;
{$ENDIF}

  pPlay := PPlayData(Msg.WParam);

  updateScreen(RGBBuffer, pPlay^.w, pPlay^.h, pPlay^.Bpp);

  FreeMem(pPlay);
end;

procedure TfrmMain.UpdateScreen(Buffer : pointer ; w, h, BPP : Integer);
  var
    pSrc, pDst : PByte;
    size : Integer;
    dMsg : String;
begin
  {$IFDEF  DEF_OUTPUT_WIN}
  multimedia_rgb_swap(Buffer, w, h, Bpp, 16, 8, 0);
  {$ENDIF}

  FresImage.BeginUpdate(False);
  FresImage.SetSize(w,h);
  size := w * h * BPP;
  pSrc := Buffer;
  pDst := FresImage.RawImage.Data;
  Move( pSrc^, pDst^, size );
  FresImage.EndUpdate(False);

  ImageRGB.Picture.Assign(FresImage);

  Application.ProcessMessages();
end;

procedure TfrmMain.OnResizeScreen(var Msg: TLMessage);
  var pPlay : PPlayData;
      dMsg   : String;
begin
  pPlay := PPlayData(Msg.WParam);

  if Msg.lParam = 1 then
  begin
    //Original Video Image size (Width x Height) from libFFPlay.DLL
    //This occurs when a playing starts.
  end else
  begin
    //Video Image size (Width x Height) during the playing
  end;
  FreeMem(pPlay);
end;

procedure TfrmMain.Timer1Timer(Sender: TObject);
  var msg : String;
begin
  FCurrentTime_Sec := sti_events.current_in_s;
  FDurationTime_mSec := sti_events.duration_in_us div 1000000;

  msg := Format('%f / %d',[FCurrentTime_Sec, FDurationTime_mSec]);
  Label1.Caption := msg;
  ScrollBar1.Max := FDurationTime_mSec ;
  ScrollBar1.Position := Round(FCurrentTime_Sec);
end;

procedure TfrmMain.FormCreate(Sender: TObject);
begin
  GetMem(RGBBuffer, (1920*1080*4*4));
  FCurrentTime_Sec := 0;
  FDurationTime_mSec:= 0;
  Timer1.Enabled := False;
  FInitPanelWidth := PanelYUV.Width;
  FInitPanelHeight:= PanelYUV.Height;;
  ButtonPlay.Enabled := True;
  ButtonStop.Enabled := False;
  ButtonPause.Enabled:= False;

  FresImage := TBitmap.Create();
  {$IFDEF DEF_OUTPUT_WIN}
  Self.Caption := 'Win32 LazFFPlayer';
  ImageRGB.Picture.Bitmap.PixelFormat := pf32Bit;
  FresImage.PixelFormat := pf32Bit;
  Self.DoubleBuffered := True;
  {$ELSE}
  Self.Caption := 'Linux lazPlayer';
  ImageRGB.Picture.Bitmap.PixelFormat := pf24Bit;
  FresImage.PixelFormat := pf24Bit;
  {$ENDIF}
  PanelYUV.Visible := True;
  ImageRGB.Visible := False;

  ImageRGB.Width := PanelYUV.Width;
  ImageRGB.Height:= PanelYUV.Height;

  Memo1.Clear();
end;

procedure TfrmMain.FormDestroy(Sender: TObject);
begin
  FresImage.Free();
  FreeMem(RGBBuffer);
  PrintDebugMessage('Application Terminated safely!');
end;

procedure TfrmMain.FormResize(Sender: TObject);
  var dMsg : String;
begin
{$IFNDEF DEF_OUTPUT_WIN}
  dMsg := Format('[Form size: %d, %d]',[Width, Height]);
  PrintDebugMessage(dMsg);
  SendResize(PanelYUV.Width,PanelYUV.Height,'PanelYUVResize');
{$ENDIF}
  FInitPanelWidth := PanelYUV.Width;
  FInitPanelHeight:= PanelYUV.Height;
end;

procedure TfrmMain.ImageRGBClick(Sender: TObject);
begin

end;

procedure TfrmMain.PanelYUVMouseMove(Sender: TObject; Shift: TShiftState; X,
  Y: Integer);
begin
  multimedia_reset_pointer();
end;

procedure TfrmMain.ImageRGBDblClick(Sender: TObject);
begin
    if Memo1.Visible = False then
     Memo1.Visible := True
  else
     Memo1.Visible := False;
end;

procedure TfrmMain.ImageRGBResize(Sender: TObject);
   var dMsg : String;
begin
   dMsg := Format('#ImageRGB Resize event  %d  %d',[ImageRGB.Width, ImageRGB.Height]);
   PrintDebugMessage(dMsg);
   dMsg := Format('#PanelYUV %d  %d',[PanelYUV.Width, PanelYUV.Height]);
   PrintDebugMessage(dMsg);
end;

procedure TfrmMain.PanelYUVDblClick(Sender: TObject);
begin
  if Memo1.Visible = False then
     Memo1.Visible := True
  else
     Memo1.Visible := False;
end;

procedure TfrmMain.PanelYUVMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
begin
  PrintDebugMessage('# Mouse Down YUV');
end;

procedure TfrmMain.FormCloseQuery(Sender: TObject; var CanClose: boolean);
begin
    {$IFDEF  DEF_OUTPUT_WIN}
    if multimedia_event_loop_alive() = 1 then
    begin
      multimedia_stream_stop();
      Sleep(500);
      CanClose := True;
    end;
    {$ELSE}
    if multimedia_event_loop_alive() = 1 then
    begin
      multimedia_stream_stop();
      Sleep(500);
      CanClose := True;
    end;
    {$ENDIF}
end;

procedure TfrmMain.ButtonPlayClick(Sender: TObject);
  var mediaFile : AnsiString;
      rtn       : Integer;
      msg       : String;
{$IFNDEF DEF_OUTPUT_WIN}
      XWinID    : TXID;
{$ENDIF}
begin
  sti_events.sender           := self;
{$IFDEF DEF_OUTPUT_WIN}
  sti_events.screenID         := PanelYUV.Handle;
  sti_events.uiType           := FFP_GUI;
{$ELSE}
  XWinID                      := 0;
  XWinID := GDK_WINDOW_XWINDOW(PGtkWidget(PtrUInt(PanelYUV.Handle))^.window);
  sti_events.screenID         := XWinID;
  sti_events.uiType           := FFP_GUI;
{$ENDIF}
  sti_events.eventInfo        := @EventInfo;
  sti_events.eventExit        := @EventExit;
  sti_events.eventAudio       := @EventAudio;
  sti_events.eventResize      := @EventResize;
  sti_events.eventStatus      := @EventPlayStatus;
  sti_events.eventRefresh     := @EventRefreshHandler;
  sti_events.playStatus       := FFP_STOP;
{$IFDEF DEF_RGB}
  sti_events.bRendererRGB     := 1;
  sti_events.eventVideo       := @EventVideo;
  PanelYUV.Enabled            := False;
  PanelYUV.Visible            := False;
  {$IFDEF  DEF_OUTPUT_WIN}
  PanelYUV.Top :=             -8172*4;//Height +100;
  PanelYUV.Left:=             -8172*4;//Width +100;
  {$ENDIF}
  ImageRGB.Visible            := True;
{$ELSE}
  sti_events.bRendererRGB     := 0;
  sti_events.eventVideo       := nil;
  PanelYUV.Visible            := True;
  ImageRGB.Visible            := False;
{$ENDIF}

{$IFDEF  DEF_OUTPUT_WIN}
       if OpenDialog.Execute() then
       begin
          mediaFile := AnsiString(OpenDialog.FileName);
          try
            multimedia_start_gui_player( PFFP_CHAR(mediaFile), @sti_events);
          except
            ShowMessage('Have a problem to play!');
          end;
       end;
{$ELSE}
       if OpenDialog.Execute() then
       begin
         Application.ProcessMessages();
         mediaFile := AnsiString(OpenDialog.FileName);
         try
           multimedia_start_gui_player( PFFP_CHAR(mediaFile), @sti_events);
         except
           ShowMessage('Have a problem to play!');
         end;
       end;
{$ENDIF}
end;

procedure TfrmMain.ButtonPauseClick(Sender: TObject);
begin
  multimedia_pause_resume();
end;

procedure TfrmMain.ButtonStopClick(Sender: TObject);
begin
  ButtonStop.Enabled  := False;
  ButtonPause.Enabled := False;
  multimedia_stream_stop();
end;

procedure TfrmMain.stopPlaying();
  var i : Integer;
begin
  for i := 0 to 39 do
  begin
    Application.ProcessMessages();
    Sleep(10);
  end;

  FCurrentTime_Sec    := 0;
  FDurationTime_mSec  := 0;
  ScrollBar1.Position := 0;
  Timer1.Enabled      := False;
  ButtonPlay.Enabled  := True;
  ButtonStop.Enabled  := False;
  ButtonPause.Enabled := False;
  ButtonPause.Caption := 'Pause';
end;

procedure TfrmMain.updateTime(Current : double);
begin
  FCurrentTime_Sec := Current;
end;

end.

