
// MFC_FFPlayDlg.h : header file
//

#pragma once

#include "FFPlayLib.h"

// CMFCFFPlayDlg dialog
class CMFCFFPlayDlg : public CDialogEx
{
// Construction
public:
	CMFCFFPlayDlg(CWnd* pParent = nullptr);	// standard constructor

	enum PLAYINGMODE {
		STOP = 0,
		PLAY = 1,
		PAUSE = 2,
		RESUME = 3
	};

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_MFC_FFPLAY_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
private:
	PLAYINGMODE m_currentMode;
	CStatic m_Pannel_yuv;
	CButton m_Button_play;
	CButton m_Button_pause;
	CButton m_Button_stop;

	FFP_EVENTS m_FFP_events;

public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);

	void StartPlaying();
	void StopPlaying();
	void setPlayingMode(enum PLAYINGMODE playmode);

	afx_msg void OnBnClickedButtonPlay();
	afx_msg void OnBnClickedButtonPause();
	afx_msg void OnBnClickedButtonStop();
};
