// FFPlayLibWrapper.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include <stdio.h>
#include <process.h>

#include "FFPlayLib.h"

static FFP_EVENTS			 FFP_events;

static FFP_EVENT_EXIT		 OnExit    = NULL;
static FFP_EVENT_INFO		 OnInfo    = NULL;
static FFP_EVENT_AUDIO		 OnAudio   = NULL;
static FFP_EVENT_VIDEO		 OnVideo   = NULL;
static FFP_EVENT_VIDEORESIZE OnResize  = NULL;
static FFP_EVENT_PLAYSTATUS	 OnStatus  = NULL;
static FFP_EVENT_REFRESH     OnRefresh = NULL;

static void OnEventExit(void *sender, int exitCode)
{
	if (OnExit != NULL)
		OnExit(sender, exitCode);
}

static void OnEventInfo(void *sender, int infoCode, char *Message)
{
	if (OnInfo != NULL)
		OnInfo(sender, infoCode, Message);
}

static void OnEventAudio(void *sender, unsigned char **AudBuffer, int BufferLengthInByte)
{
	if (OnAudio != NULL)
		OnAudio(sender, AudBuffer, BufferLengthInByte);
}

static void OnEventVideo(void *sender, void *YuvData, int isRGB)
{
	if (OnVideo != NULL)
		OnVideo(sender, YuvData, isRGB);
}

static void OnEventResize(void *sender, int width, int height, int isOriginalsize)
{
	if (OnResize != NULL)
		OnResize(sender, width, height, isOriginalsize);
}

static void OnEventStatus(void *sender, FFP_PLAY_STATUS status)
{
	if (OnStatus != NULL)
		OnStatus(sender, status);
}

static void OnEventRefresh(void *sender)
{
	if (OnRefresh != NULL)
		OnRefresh(sender);
}

static void ThreadStreaming(void *pParam)
{
	multimedia_stream_start();
}

struct FFPLAY_ELEMENTS{
	void *sender;
	unsigned int yuvHandle;
	void *eventInfo;
	void *eventExit;
	void *eventAudio;
	void *eventVideo;
	void *eventResize;
	void *eventStatus; 
	void *eventRefresh;
};

extern "C"
{
	__declspec(dllexport) int  Start_CLI_FFPlayer(int argc, char **argv, FFPLAY_ELEMENTS* playElements);
	__declspec(dllexport) int  Setup_GUI_FFPlayer(int argc, char **argv, FFPLAY_ELEMENTS* playElements);
	__declspec(dllexport) int  SetupAndStart_GUI_FFPlayer(int argc, char** argv, FFPLAY_ELEMENTS* playElements);
	__declspec(dllexport) void PauseResume_FFPlayer();
	__declspec(dllexport) void Start_FFPlayer();
	__declspec(dllexport) void Stop_FFPlayer();
	__declspec(dllexport) void Resize_GUI_Screen(int w, int h);
	__declspec(dllexport) void Test_GUI_Screen(int xWinID, int holdTime);
}

void Set_FFPlayer_FileName(const char *FileName)
{
	multimedia_set_filename(FileName);
}

const char* Get_FFPlayer_FileName()
{
	return multimedia_get_filename();
}

int Start_CLI_FFPlayer(int argc, char **argv, FFPLAY_ELEMENTS* playElements)
{
	FFP_events.sender = playElements->sender;
	FFP_events.current_in_s			= 0;
	FFP_events.duration_in_us		= 0;
	FFP_events.event_audio			= OnEventAudio;
	FFP_events.event_exit			= OnEventExit;
	FFP_events.event_play_status	= OnEventStatus;
	FFP_events.event_info			= OnEventInfo;
	FFP_events.event_video			= NULL;// OnEventVideo;
	FFP_events.event_video_resize	= OnEventResize;
	FFP_events.screenID				= 0;
	FFP_events.ui_type				= FFP_CLI;
	FFP_events.playstatus			= FFP_STOP;

	OnExit = (FFP_EVENT_EXIT)playElements->eventExit;
	OnInfo = (FFP_EVENT_INFO)playElements->eventInfo;
	OnAudio = (FFP_EVENT_AUDIO)playElements->eventAudio;
	OnVideo		= NULL;
	OnResize = (FFP_EVENT_VIDEORESIZE)playElements->eventResize;
	OnStatus = (FFP_EVENT_PLAYSTATUS)playElements->eventStatus;

	multimedia_parse_options(argc, argv);
	
	if (multimedia_init_device(&FFP_events) != 0)
	{
		multimedia_exit();
		return 1;
	}

	if (!multimedia_stream_open())
	{
		multimedia_exit();
		return 2;
	}

	multimedia_stream_start();
	return 0;
}

int  Setup_GUI_FFPlayer(int argc, char **argv, FFPLAY_ELEMENTS* playElements)
{
	FFP_events.sender = playElements->sender;
	FFP_events.current_in_s = 0;
	FFP_events.duration_in_us = 0;
	FFP_events.event_exit = OnEventExit;
	FFP_events.event_play_status = OnEventStatus;
	FFP_events.event_info = OnEventInfo;
	FFP_events.event_video_resize = OnEventResize;
	FFP_events.event_refresh = OnEventRefresh;
	FFP_events.screenID = playElements->yuvHandle;
	FFP_events.ui_type = FFP_GUI;
	FFP_events.playstatus = FFP_STOP;

	OnExit = (FFP_EVENT_EXIT)playElements->eventExit;
	OnInfo = (FFP_EVENT_INFO)playElements->eventInfo;
	OnAudio = (FFP_EVENT_AUDIO)playElements->eventAudio;
	OnVideo = (FFP_EVENT_VIDEO)playElements->eventVideo;
	OnResize = (FFP_EVENT_VIDEORESIZE)playElements->eventResize;
	OnStatus = (FFP_EVENT_PLAYSTATUS)playElements->eventStatus;
	OnRefresh = (FFP_EVENT_REFRESH)playElements->eventRefresh;

	if (playElements->eventVideo == NULL)
	{
		FFP_events.event_video = NULL;
	}
	
	if (playElements->eventAudio == NULL)
	{
		FFP_events.event_audio = NULL;
	}

	int rtn = multimedia_setup_gui_player_with_arguments(argc, argv, &FFP_events);

	return rtn;
}

int  SetupAndStart_GUI_FFPlayer(int argc, char** argv, FFPLAY_ELEMENTS* playElements)
{
	FFP_events.sender = playElements->sender;
	FFP_events.current_in_s = 0;
	FFP_events.duration_in_us = 0;
	FFP_events.event_exit = OnEventExit;
	FFP_events.event_play_status = OnEventStatus;
	FFP_events.event_info = OnEventInfo;
	FFP_events.event_video_resize = OnEventResize;
	FFP_events.event_refresh = OnEventRefresh;
	FFP_events.screenID = playElements->yuvHandle;
	FFP_events.ui_type = FFP_GUI;
	FFP_events.playstatus = FFP_STOP;

	OnExit = (FFP_EVENT_EXIT)playElements->eventExit;
	OnInfo = (FFP_EVENT_INFO)playElements->eventInfo;
	OnAudio = (FFP_EVENT_AUDIO)playElements->eventAudio;
	OnVideo = (FFP_EVENT_VIDEO)playElements->eventVideo;
	OnResize = (FFP_EVENT_VIDEORESIZE)playElements->eventResize;
	OnStatus = (FFP_EVENT_PLAYSTATUS)playElements->eventStatus;
	OnRefresh = (FFP_EVENT_REFRESH)playElements->eventRefresh;

	if (playElements->eventVideo == NULL)
	{
		FFP_events.event_video = NULL;
	}

	if (playElements->eventAudio == NULL)
	{
		FFP_events.event_audio = NULL;
	}

	return multimedia_start_gui_player_with_arguments(argc, argv, &FFP_events);
}

void PauseResume_FFPlayer()
{
	multimedia_pause_resume();
}

void Stop_FFPlayer()
{
	multimedia_stream_stop();
}

void Start_FFPlayer()
{
	multimedia_stream_start();
}

void Resize_GUI_Screen(int w, int h)
{
	multimedia_resize_screen(w, h);
}

void Test_GUI_Screen(int xWinID, int holdTime)
{
	multimedia_test_screen(xWinID, holdTime);
}