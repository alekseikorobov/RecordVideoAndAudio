using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Diagnostics;
using System.Windows.Forms;
using SharpAvi;
using Captura.Audio;
using Captura.Video;
using Captura.Windows;
using Captura;
using System.Collections.Generic;
using System.Linq;
using Captura.FFmpeg;

namespace RecordVideoAndAudio
{
    public class RecorderVideo
    {
        private readonly int screenWidth;
        private readonly int screenHeight;
        private Recorder _recorder;

        public RecorderAudio RecorderAudio { get; }
        internal X264VideoCodec VideoWriter { get; }
        public string FileName { get; private set; }

        public RecorderVideo(RecorderAudio recorderAudio)
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            RecorderAudio = recorderAudio;
            VideoWriter = new X264VideoCodec();

        }
        IImageProvider GetImageProvider(/*RecordingModelParams RecordingParams*/)
        {
            var _platformServices = new WindowsPlatformServices();
            return _platformServices.GetAllScreensProvider(true,
                false /*_videoSettings.RecorderMode == RecorderMode.Steps*/);
        }
        IImageProvider GetImageProviderWithOverlays()
        {
            var imageProvider = GetImageProvider();

            return imageProvider == null
                ? null
                : new OverlayedImageProvider(imageProvider, GetOverlays().ToArray());
        }
        IEnumerable<IOverlay> GetOverlays()
        {
            MouseOverlaySettings MousePointerOverlay = new MouseOverlaySettings
            {
                Color = Color.FromArgb(200, 239, 108, 0)
            };

            yield return new MousePointerOverlay(MousePointerOverlay);
        }

        IVideoFileWriter GetVideoFileWriter(IImageProvider ImgProvider, IAudioProvider AudioProvider, 
            //RecordingModelParams RecordingParams, 
            string FileName = null)
        {

            var args = new VideoWriterArgs
            {
                FileName = FileName, //?? CurrentFileName,
                FrameRate = 20,//Settings.Video.FrameRate,
                VideoQuality = 70,//Settings.Video.Quality,
                ImageProvider = ImgProvider,
                AudioQuality = 80,//Settings.Audio.Quality,
                AudioProvider = AudioProvider
            };

            return VideoWriter.GetVideoFileWriter(args);
        }

        bool SetupVideoRecorder(IAudioProvider AudioProvider)
        {
            var imgProvider = GetImageProviderWithOverlays();

            IVideoFileWriter videoEncoder;

            try
            {
                Captura.Windows.MediaFoundation.MfManager.Startup();

                videoEncoder = GetVideoFileWriter(imgProvider, AudioProvider);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                imgProvider?.Dispose();

                return false;
            }

            _recorder = new Recorder(videoEncoder, imgProvider, 20, AudioProvider);

            return true;
        }
        public void StartRecord(string fileName,
            FourCC codec, int quality)
        {
            FileName = fileName;
            RecorderAudio.SetupAudioProvider(fileName);
            if (!SetupVideoRecorder(RecorderAudio.AudioProvider))
            {
                return;
            }
            _recorder.Start();
        }


        private IAviVideoStream CreateVideoStream(FourCC codec, int quality)
        {
            // Select encoder type based on FOURCC of codec
            if (codec == KnownFourCCs.Codecs.Uncompressed)
            {
                return writer.AddUncompressedVideoStream(screenWidth, screenHeight);
            }
            else if (codec == KnownFourCCs.Codecs.MotionJpeg)
            {
                return writer.AddMotionJpegVideoStream(screenWidth, screenHeight, quality);
            }
            else
            {
                return writer.AddMpeg4VideoStream(screenWidth, screenHeight, (double)writer.FramesPerSecond,
                    // It seems that all tested MPEG-4 VfW codecs ignore the quality affecting parameters passed through VfW API
                    // They only respect the settings from their own configuration dialogs, and Mpeg4VideoEncoder currently has no support for this
                    quality: quality,
                    codec: codec,
                    // Most of VfW codecs expect single-threaded use, so we wrap this encoder to special wrapper
                    // Thus all calls to the encoder (including its instantiation) will be invoked on a single thread although encoding (and writing) is performed asynchronously
                    forceSingleThreadedAccess: true);
            }
        }


        public void StopRecord()
        {
            //Captura.Windows.MediaFoundation.MfManager.Shutdown();
            _recorder.Dispose();

        }      
    }
}