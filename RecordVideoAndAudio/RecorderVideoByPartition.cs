﻿using System;
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

namespace RecordVideoAndAudio
{
    public class RecorderVideoByPartition
    {
        private readonly int screenWidth;
        private readonly int screenHeight;
        private AviWriter writer;
        private IAviVideoStream videoStream;
        private Thread screenThread;
        private ManualResetEvent stopThread;
        private Stopwatch stopwatchGlobal;

        public RecorderVideoByPartition()
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;


        }

        public void StartRecord(string fileName, FourCC codec, int quality)
        {
            stopThread = new ManualResetEvent(false);

            // Create AVI writer and specify FPS
            writer = new AviWriter(fileName)
            {
                FramesPerSecond = 10,
                EmitIndex1 = true,
            };

            // Create video stream
            videoStream = CreateVideoStream(codec, quality);
            // Set only name. Other properties were when creating stream, 
            // either explicitly by arguments or implicitly by the encoder used
            videoStream.Name = "Screencast";

            screenThread = new Thread(RecordScreen)
            {
                Name = typeof(RecorderVideo).Name + ".RecordScreen",
                IsBackground = true
            };
            screenThread.Start();
            stopwatchGlobal = Stopwatch.StartNew();

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


        public void StopRecord(bool isBreak)
        {
            stopThread.Set();
            if (!isBreak)
            {
                screenThread.Join();
            }
            else
            {
                stopThread.Close();
            }

            writer.Close();

            stopThread.Close();
        }

        private void RecordScreen()
        {
            var stopwatch = new Stopwatch();
            var buffer = new byte[screenWidth * screenHeight * 4];

            Task videoWriteTask = null;

            var isFirstFrame = true;
            var shotsTaken = 0;
            var timeTillNextFrame = TimeSpan.Zero;
            stopwatch.Start();

            while (!stopThread.SafeWaitHandle.IsClosed && !stopThread.WaitOne(timeTillNextFrame))
            {
                ScreenshotCaptureWithMouse.ScreenCapture.CaptureScreen.GetScreenshotAll(buffer, screenWidth, screenHeight);

                shotsTaken++;

                // Wait for the previous frame is written
                if (!isFirstFrame)
                {
                    videoWriteTask.Wait();
                }
                videoWriteTask = videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);

                timeTillNextFrame = TimeSpan.FromSeconds(shotsTaken / (double)writer.FramesPerSecond - stopwatch.Elapsed.TotalSeconds);
                if (timeTillNextFrame < TimeSpan.Zero)
                    timeTillNextFrame = TimeSpan.Zero;

                isFirstFrame = false;
                
                if(stopwatchGlobal.Elapsed.TotalSeconds > 10)
                {
                    ///если превышаем размер, то запускаем процесс создания видео
                    ///запускаем новый процес записи аудио
                    Break?.Invoke();
                }
            }

            stopwatch.Stop();

            // Wait for the last frame is written
            if (!isFirstFrame)
            {
                videoWriteTask.Wait();
            }
        }

        public event Action Break;  
    }
}