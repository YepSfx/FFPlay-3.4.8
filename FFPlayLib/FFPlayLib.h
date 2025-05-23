#ifdef __MINGW32__
  #undef main /* We don't want SDL to override our main() */
  typedef long long int64_t;  
#endif

#ifdef DEF_BUILD_DLL
  #define EXPORTDLL __declspec(dllexport)
  #define IMPORTDLL __declspec(dllimport)
#else
  #define EXPORTDLL 
  #define IMPORTDLL
#endif  

#define FFP_AUDIO_U8	    0x0008	                  /**< Unsigned 8-bit samples */
#define FFP_AUDIO_S8	    0x8008	                  /**< Signed 8-bit samples */
#define FFP_AUDIO_U16LSB	0x0010	                  /**< Unsigned 16-bit samples */
#define FFP_AUDIO_S16LSB	0x8010	                  /**< Signed 16-bit samples */
#define FFP_AUDIO_U16MSB	0x1010	                  /**< As above, but big-endian byte order */
#define FFP_AUDIO_S16MSB	0x9010	                  /**< As above, but big-endian byte order */
#define FFP_AUDIO_U16	    FFP_AUDIO_U16LSB
#define FFP_AUDIO_S16	    FFP_AUDIO_S16LSB
  
typedef enum {
    FFP_CLI = 0,
    FFP_GUI
}FFP_UI_TYPE;

typedef enum {
    FFP_FALSE = 0,
    FFP_TRUE  = 1
}FFP_BOOL;

typedef enum {
    FFP_INFO_NONE         = 0,
    FFP_INFO_WARNING      = 1,
    FFP_INFO_ERROR        = 2, 
    FFP_INFO_STREAM_ERROR = 3,
    FFP_INFO_DEBUG        = 4
}FFP_INFO;

typedef struct {
    int               Freq;
    unsigned char     Channels;
    unsigned short    Format;
    unsigned short    SamplesInBuffer;
    unsigned long     BufferSizeInBytes;
} FFP_AUD_PARAMS;

typedef struct{
  int   width;
  int   height;
  int   Bpp;
} FFP_VID_PARAMS;

typedef struct {
  int           w;
  int           h;
  unsigned char **pixels;
}FFP_YUV420P_DATA;

typedef struct {
  int           w;
  int           h;
  int           BPP;
  unsigned char *pixels;
}FFP_RGB_DATA;


typedef enum {
  FFP_STOP    = 0, 
  FFP_PLAY    = 1, 
  FFP_PAUSED  = 2,
  FFP_RESUMED = 3, 
}FFP_PLAY_STATUS;

typedef void (*FFP_EVENT_EXIT)(void *sender, int exitCode);
typedef void (*FFP_EVENT_INFO)(void *sender, int infoCode, char *Message);
typedef void (*FFP_EVENT_AUDIO)(void *sender, unsigned char **AudBuffer, int BufferLengthInByte);
typedef void (*FFP_EVENT_VIDEO)(void *sender, void *rgbData, int isRGB); //isRGB: 0 -> IYUV ; 1 -> RGB for Video Image ; 2 -> RGB for Audio Image
typedef void (*FFP_EVENT_VIDEORESIZE)(void *sender, int width, int height, int isOriginalsize);
typedef void (*FFP_EVENT_PLAYSTATUS)(void *sender, FFP_PLAY_STATUS status);
typedef void (*FFP_EVENT_VIDEOSIZE)(void *sender, int width, int height);
typedef void (*FFP_EVENT_REFRESH)(void *sender);

typedef struct {
   void*                  sender;
   unsigned int           screenID;
   int                    bRendererRGB;
   int64_t                duration_in_us;
   double                 current_in_s; 
   FFP_UI_TYPE            ui_type;
   FFP_EVENT_INFO         event_info; 
   FFP_EVENT_EXIT         event_exit;
   FFP_EVENT_AUDIO        event_audio;
   FFP_EVENT_VIDEO        event_video;
   FFP_EVENT_VIDEORESIZE  event_video_resize;
   FFP_EVENT_PLAYSTATUS   event_play_status;
   FFP_EVENT_REFRESH      event_refresh;
   FFP_PLAY_STATUS        playstatus;   
}FFP_EVENTS;

const char*      EXPORTDLL multimedia_get_filename();
void             EXPORTDLL multimedia_parse_options(int argc, char **argv);
void             EXPORTDLL multimedia_set_filename( const char *filename);
FFP_AUD_PARAMS*  EXPORTDLL multimedia_get_audioformat();
FFP_VID_PARAMS*  EXPORTDLL multimedia_get_videoformat();
int64_t          EXPORTDLL multimedia_get_duration_in_mSec();
int              EXPORTDLL multimedia_event_loop_alive();
void             EXPORTDLL multimedia_resize_screen(int w, int h); 
void             EXPORTDLL multimedia_reset_pointer(); 

void             EXPORTDLL multimedia_yuv420p_to_rgb24(FFP_YUV420P_DATA *yuvData,unsigned char* rgbbuffer);
void             EXPORTDLL multimedia_yuv420p_to_rgb32(FFP_YUV420P_DATA *yuvData,unsigned char* rgbbuffer);
void 		         EXPORTDLL multimedia_rgb_swap(void *pRGB, int width, int height, int bpp, int shiftR, int shiftG, int shitB);

void             EXPORTDLL multimedia_start_cli_player(int argc, char **argv, FFP_EVENTS *events);
int              EXPORTDLL multimedia_start_gui_player(const char* mediaName, FFP_EVENTS *events);
int              EXPORTDLL multimedia_start_gui_player_with_arguments(int argc, char **argv, FFP_EVENTS *events);

int              EXPORTDLL multimedia_setup_gui_player(FFP_EVENTS *events);
int              EXPORTDLL multimedia_setup_gui_player_with_arguments(int argc, char **argv,FFP_EVENTS *events);

void             EXPORTDLL multimedia_pause_resume();
void             EXPORTDLL multimedia_stream_stop();  
void             EXPORTDLL multimedia_stream_start();
void             EXPORTDLL multimedia_exit();
void 		         EXPORTDLL multimedia_test_screen( int xWinID, int latency ); 
void             EXPORTDLL SaveFramebufferAsPPM(void* Buff, int w, int h, int Bpp); 

