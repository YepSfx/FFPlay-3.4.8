#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <time.h>

#include <signal.h>

#include "FFPlayLib.h"

void ExitEvent(void *sender, int exitCode)
{
   printf("\n\n Exit Event!!!\n\n");
   exit(exitCode);
}  

void sigterm_handler(int sig)
{
   ExitEvent(NULL, 123);
}

static char FileName[256] = {0,};
static unsigned char RGBBuff[1920*1080*4] = {0,};

/* Called from the main */

void PlayProgress(void *sender)
{
    FFP_VID_PARAMS  *vidParams = multimedia_get_videoformat();
    FFP_AUD_PARAMS  *audParams = multimedia_get_audioformat();
}

#if 1
void CallbackVideo(void *sender, void *videoData, int isRGB)
{
      if (isRGB != 1)
      {
         FFP_YUV420P_DATA *yuvData = (FFP_YUV420P_DATA*)videoData;
         multimedia_yuv420p_to_rgb32( yuvData,  RGBBuff);      
         SaveFramebufferAsPPM(RGBBuff, yuvData->w, yuvData->h, 4 );  
      }
      else
      {
         FFP_RGB_DATA *rgbData = (FFP_RGB_DATA*)videoData;
         SaveFramebufferAsPPM(rgbData->pixels, rgbData->w, rgbData->h, rgbData->BPP );
      }   
} 
#else
void CallbackVideo(void *sender, void *videoData)
{
     FFP_RGB_DATA *rgbData = (FFP_RGB_DATA*)videoData;
     SaveFramebufferAsPPM(rgbData->pixels, rgbData->w, rgbData->h, rgbData->BPP );
} 
#endif
void CallbackAudio(void *sender, unsigned char **buffer, int BufLenInByte)
{
    FFP_AUD_PARAMS *params = multimedia_get_audioformat();
    int i, ch, chs = params->Channels, fmt = params->Format, samps = params->SamplesInBuffer;
    short *AudData[chs], aSample;

    for (i = 0 ; i < chs ; i++)
        AudData[i] = (short*)*(buffer++);
    
    for (i = 0 ; i < samps ; i++)
    {
        for (ch = 0 ; ch < chs ; ch++)
        {
            AudData[ch][i] = AudData[ch][i];
        }
    }    
}

void CallbackResize(void *sender, int w , int h, int isOriginalSize)
{
    if (isOriginalSize)
    {
        printf( "--->> Report Original Size  %d x %d\n\n", w, h );
    }
}

void MessageInfo(void *sender, int infoCode, char *Message)
{
        printf( "--->> %d: %s\n", infoCode, Message );
}

int main(int argc, char **argv)
{
    int theID = 123;
    FFP_EVENTS FFP_events;
    
    printf("\n");
    
    memset( (void*)&FFP_events, 0, sizeof(FFP_EVENTS));
    
    FFP_events.sender        = &theID; 
    FFP_events.ui_type       = FFP_CLI;
    FFP_events.bRendererRGB  = 0;
    FFP_events.event_exit    = ExitEvent; 
    FFP_events.event_info    = MessageInfo;
    FFP_events.event_audio   = NULL;//CallbackAudio;
    FFP_events.event_video   = NULL;//CallbackVideo;
    FFP_events.event_video_resize = CallbackResize;
    FFP_events.playstatus    = FFP_STOP;
    FFP_events.event_refresh = NULL;

    signal(SIGINT , sigterm_handler); /* Interrupt (ANSI).    */
    signal(SIGTERM, sigterm_handler); /* Termination (ANSI).  */

    /* never returns */
    multimedia_start_cli_player( argc, argv, &FFP_events );

    return 0;
}
