
// MFC_FFPlayDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "MFC_FFPlay.h"
#include "MFC_FFPlayDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMFCFFPlayDlg dialog



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
	ON_BN_CLICKED(IDC_BUTTON_PLAY, &CMFCFFPlayDlg::OnBnClickedButtonPlay)
	ON_BN_CLICKED(IDC_BUTTON_PAUSE, &CMFCFFPlayDlg::OnBnClickedButtonPause)
	ON_BN_CLICKED(IDC_BUTTON_STOP, &CMFCFFPlayDlg::OnBnClickedButtonStop)
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
	this->setPlayingMode(STOP);

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


void CMFCFFPlayDlg::OnBnClickedButtonPlay()
{
	// TODO: Add your control notification handler code here
	this->setPlayingMode(PLAY);
}

void CMFCFFPlayDlg::OnBnClickedButtonPause()
{
	// TODO: Add your control notification handler code here
	if (this->m_currentMode == PAUSE)
	{
		this->setPlayingMode(RESUME);
	}
	else 
	{
		this->setPlayingMode(PAUSE);
	}
}

void CMFCFFPlayDlg::OnBnClickedButtonStop()
{
	// TODO: Add your control notification handler code here
	this->setPlayingMode(STOP);
}
