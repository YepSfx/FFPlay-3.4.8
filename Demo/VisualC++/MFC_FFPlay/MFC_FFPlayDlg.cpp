
// MFC_FFPlayDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "MFC_FFPlay.h"
#include "MFC_FFPlayDlg.h"
#include "afxdialogex.h"
#include <string>
#include <atlconv.h> 

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

static void DoProc()
{
	MSG		msg;

	while (PeekMessage(&msg, 0, 0, 0, PM_REMOVE))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}
}

void __cdecl EventExit(void* sender, int exitCode)
{
	OutputDebugString(_T("Exit Event\n"));
}

void __cdecl Eventinfo(void* sender, int infoCode, char* pMsg)
{
	CString msg = CString(pMsg);
	CString code;
	code.Format(_T(" infoCode: %d"), infoCode);
	msg = CString(_T(">Eventinfo: ")) + msg + code + CString(_T("\n"));
	OutputDebugString(msg);
}

void __cdecl EventAudio(void* sender, BYTE** pBuffer, int BufferLenInByte)
{

}

void __cdecl EventPlayStatus(void* sender, FFP_PLAY_STATUS status)
{
	CString msg;
	msg.Format(_T(">>Play Status Info: %d\n"), (int)status);
	OutputDebugString(msg);

	CMFCFFPlayDlg *pPlayer = (CMFCFFPlayDlg*)sender;

	switch (status)
	{
		case FFP_STOP:
			pPlayer->setPlayingMode(CMFCFFPlayDlg::PLAYINGMODE::STOP);
			break;
		case FFP_PLAY:
			pPlayer->setPlayingMode(CMFCFFPlayDlg::PLAYINGMODE::PLAY);
			break;
		case FFP_PAUSED:
			pPlayer->setPlayingMode(CMFCFFPlayDlg::PLAYINGMODE::PAUSE);
			break;
		case FFP_RESUMED:
			pPlayer->setPlayingMode(CMFCFFPlayDlg::PLAYINGMODE::RESUME);
			break;
	}
}

void __cdecl EventVideo(void* sender, FFP_YUV420P_DATA* pYUVData)
{
}

void __cdecl EventResize(void* sender, int width, int height, int isOriginalsize)
{
	OutputDebugString(_T("Resize Event\n"));
}

void __cdecl EventRefresh(void* sender)
{
	DoProc();
}

static void ThreadStreaming(void* pParam)
{
	multimedia_stream_start();
}

// CMFCFFPlayDlg dialog

std::string CStringToUTF8(const CString& str)
{
	CW2A utf8(str.GetString(), CP_UTF8);
	return std::string(utf8);
}

std::string utf8_encode(const std::wstring &wstr)
{
	if (wstr.empty())
		return std::string();
	int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);

	std::string strTo(size_needed, 0);
	WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);

	return strTo;
}

CMFCFFPlayDlg::CMFCFFPlayDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_MFC_FFPLAY_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CMFCFFPlayDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_PLAY, m_Button_play);
	DDX_Control(pDX, IDC_BUTTON_PAUSE, m_Button_pause);
	DDX_Control(pDX, IDC_BUTTON_STOP, m_Button_stop);
	DDX_Control(pDX, IDC_STATIC_YUV, m_Pannel_yuv);
}

BEGIN_MESSAGE_MAP(CMFCFFPlayDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_PLAY,  &CMFCFFPlayDlg::OnBnClickedButtonPlay)
	ON_BN_CLICKED(IDC_BUTTON_PAUSE, &CMFCFFPlayDlg::OnBnClickedButtonPause)
	ON_BN_CLICKED(IDC_BUTTON_STOP,  &CMFCFFPlayDlg::OnBnClickedButtonStop)
END_MESSAGE_MAP()


// CMFCFFPlayDlg message handlers

BOOL CMFCFFPlayDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	setPlayingMode(STOP);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMFCFFPlayDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CMFCFFPlayDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

BOOL CMFCFFPlayDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->wParam == VK_RETURN || pMsg->wParam == VK_F4 || pMsg->wParam == VK_ESCAPE)
		return TRUE;

	return CDialogEx::PreTranslateMessage(pMsg);
}

void CMFCFFPlayDlg::setPlayingMode(enum PLAYINGMODE playmode)
{
	switch (playmode)
	{
		case STOP:
			m_Button_play.EnableWindow(TRUE);
			m_Button_stop.EnableWindow(FALSE);
			m_Button_pause.EnableWindow(FALSE);
			m_Button_pause.SetWindowText(CString("PAUSE"));
			break;
		case PLAY:
			m_Button_play.EnableWindow(FALSE);
			m_Button_stop.EnableWindow(TRUE);
			m_Button_pause.EnableWindow(TRUE);
			m_Button_pause.SetWindowText(CString("PAUSE"));
			break;
		case PAUSE:
			m_Button_play.EnableWindow(FALSE);
			m_Button_stop.EnableWindow(TRUE);
			m_Button_pause.EnableWindow(TRUE);
			m_Button_pause.SetWindowText(CString("RESUME"));
			break;
		case RESUME:
			m_Button_play.EnableWindow(FALSE);
			m_Button_stop.EnableWindow(TRUE);
			m_Button_pause.EnableWindow(TRUE);
			m_Button_pause.SetWindowText(CString("PAUSE"));
			break;
	}
	m_currentMode = playmode;
}

static char argv[4][256] = { NULL, };

void CMFCFFPlayDlg::OnBnClickedButtonPlay()
{
	// TODO: Add your control notification handler code here

	CFileDialog openFileDialog(TRUE, _T("mov"), NULL, OFN_FILEMUSTEXIST | OFN_HIDEREADONLY,
		_T("Mov (*.mov)|*.mov|AVI (*.avi)|*.avi|MP4 (*.mp4)|*.mp4|All Files (*.*)|*.*||", this));

	if (openFileDialog.DoModal() == IDOK)
	{
		int rtn;

		CString fileName = openFileDialog.GetPathName();

		std::wstring wsFileName(fileName);
		std::string utf8filename = utf8_encode(wsFileName);

		const char* pFileName = utf8filename.c_str();

		m_FFP_events.sender = this;
		m_FFP_events.event_info = Eventinfo;
		m_FFP_events.screenID = (unsigned long)m_Pannel_yuv.m_hWnd;
		m_FFP_events.ui_type = FFP_GUI;
		m_FFP_events.event_exit = EventExit;
		m_FFP_events.event_audio = EventAudio;
		m_FFP_events.event_video_resize = EventResize;
		m_FFP_events.event_play_status = EventPlayStatus;
		m_FFP_events.playstatus = FFP_STOP;
		m_FFP_events.event_video = NULL;
		m_FFP_events.event_refresh = EventRefresh;

		strcpy_s(argv[0], "FFPlay");
		strcpy_s(argv[1], pFileName);
		strcpy_s(argv[2], "-vf");
		strcpy_s(argv[3], "yadif=1");

		char* argv_ptrs[4] = { NULL, };
		for (int i = 0; i < 4; i++)
			argv_ptrs[i] = argv[i];
		
		char** args = argv_ptrs;

		try
		{
#if 0			
			multimedia_set_filename(pFileName);

			if (multimedia_init_device(&m_FFP_events) != 0)
			{
				multimedia_exit();
				MessageBox(_T("Fail to Init!"));
				return;
			}

			if (multimedia_stream_open() == FFP_FALSE)
			{
				multimedia_exit();
				MessageBox(_T("Fail to open file!"));
				return;
			}
#else
			//multimedia_set_filename(pFileName);
			//multimedia_setup_gui_player(&m_FFP_events);
			multimedia_setup_gui_player_with_arguments(4, args, &m_FFP_events);
#endif
			StartPlaying();

			CRect rect;
			m_Pannel_yuv.GetWindowRect(&rect);
			
			int w = rect.Width();
			int h = rect.Height();

			multimedia_resize_screen(w, h);

		}
		catch (const std::exception& e) 
		{
			AfxMessageBox(CString(_T("Exception: ")) + CString(e.what()));
		}
		catch(...)
		{
			AfxMessageBox(_T("Unknown exception occurred"));
		}
	}
}

void CMFCFFPlayDlg::OnBnClickedButtonPause()
{
	// TODO: Add your control notification handler code here
	multimedia_pause_resume();
}

void CMFCFFPlayDlg::OnBnClickedButtonStop()
{
	// TODO: Add your control notification handler code here
	StopPlaying();
}

void CMFCFFPlayDlg::StartPlaying()
{
	_beginthread(ThreadStreaming, 0, NULL);
}

void CMFCFFPlayDlg::StopPlaying()
{
	OutputDebugString(_T("Stop playing...\n"));
	multimedia_stream_stop();
	OutputDebugString(_T("Playing stopped.\n"));
}
