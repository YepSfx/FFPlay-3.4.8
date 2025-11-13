
// MFC_FFPlayDlg.h : header file
//

#pragma once


// CMFCFFPlayDlg dialog
class CMFCFFPlayDlg : public CDialogEx
{
// Construction
public:
	CMFCFFPlayDlg(CWnd* pParent = nullptr);	// standard constructor

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
	enum PLAYINGMODE {
		STOP = 0, 
		PLAY = 1,
		PAUSE = 2,
		RESUME=3
	};
	PLAYINGMODE m_currentMode;
	CStatic m_Pannel_yuv;
	CButton m_Button_play;
	CButton m_Button_pause;
	CButton m_Button_stop;

	void setPlayingMode(enum PLAYINGMODE playmode);
	
public:
	afx_msg void OnBnClickedButtonPlay();
	afx_msg void OnBnClickedButtonPause();
	afx_msg void OnBnClickedButtonStop();
};
