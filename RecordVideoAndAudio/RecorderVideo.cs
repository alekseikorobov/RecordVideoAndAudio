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
using System.Collections.Generic;
using ScreenshotCaptureWithMouse.ScreenCapture;
using System.IO;

namespace RecordVideoAndAudio
{
    public class RecorderVideo
    {
        private readonly int screenWidth;
        private readonly int screenHeight;
        private AviWriter writer;
        private IAviVideoStream videoStream;
        private Thread screenThread;
        private ManualResetEvent stopThread;
        private AutoResetEvent videoFrameWritten;

        public RecorderVideo()
        {
            screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;

            //var bounds = SystemInformation.VirtualScreen;

            //screenWidth = bounds.Width;
            //screenHeight = bounds.Height;

            ////PointTransform = P => new Point(P.X - bounds.Left, P.Y - bounds.Top);



        }

        public void StartRecord(string fileName, FourCC codec, int quality)
        {
            stopThread = new ManualResetEvent(false);
            videoFrameWritten = new AutoResetEvent(false);

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
            stopThread.Set();
            screenThread.Join();

            writer.Close();

            stopThread.Close();
        }
        public HashSet<Exception> Exceptions = new HashSet<Exception>(new ExceptionComparer());
        public int CountErrors = 0;
        private void RecordScreen()
        {
            try
            {
                var stopwatch = new Stopwatch();
                var buffer = new byte[screenWidth * screenHeight * 4];

                Task videoWriteTask = null;

                var isFirstFrame = true;
                var shotsTaken = 0;
                var timeTillNextFrame = TimeSpan.Zero;
                stopwatch.Start();

                while (!stopThread.WaitOne(timeTillNextFrame))
                {
                    try
                    {
                        //buffer = GetScreenshotNew();
                        GetScreenshot(buffer);
                    }
                    catch (Exception ex)
                    {
                        Exceptions.Add(ex);
                        Interlocked.Increment(ref CountErrors);
                        GetScreenshotEmpty(buffer);
                    }
                    shotsTaken++;

                    // Wait for the previous frame is written
                    if (!isFirstFrame)
                    {
                        videoWriteTask.Wait();
                        videoFrameWritten.Set();
                    }
                    videoWriteTask = videoStream.WriteFrameAsync(true, buffer, 0, buffer.Length);

                    timeTillNextFrame = TimeSpan.FromSeconds(shotsTaken / (double)writer.FramesPerSecond - stopwatch.Elapsed.TotalSeconds);
                    if (timeTillNextFrame < TimeSpan.Zero)
                        timeTillNextFrame = TimeSpan.Zero;

                    isFirstFrame = false;
                }

                stopwatch.Stop();

                // Wait for the last frame is written
                if (!isFirstFrame)
                {
                    videoWriteTask.Wait();
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref CountErrors);
                Exceptions.Add(ex);
            }
        }

        private void GetScreenshot(byte[] buffer)
        {
            using (var bitmap = new Bitmap(screenWidth, screenHeight))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    //IntPtr hdc = graphics.GetHdc();

                    graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));

                    //если пытаться записывать положение курсора, то начинает расти количество GDI объектов                    
                    //try
                    //{
                    //    var icon = ScreenshotCaptureWithMouse.ScreenCapture.CaptureScreen.CaptureCursorIcon(out int cursorX, out int cursorY);
                    //    if (icon == null)
                    //        throw new Exception($"icon is empty");

                    //    graphics.DrawIcon(icon, cursorX, cursorY);
                        
                    //}
                    //catch (Exception ex)
                    //{
                    //    graphics.DrawEllipse(new Pen(Color.White), 1, 1, 10, 10);
                    //}
                    graphics.Flush();

                    var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                    bitmap.UnlockBits(bits);
                    //graphics.Flush();
                    //graphics.ReleaseHdc(hdc);

                }
            }
        }
        public struct SIZE
        {
            public int cx;
            public int cy;
        }
        static void GetScreenshotNew1(byte[] buffer)
        {
            SIZE size;
            IntPtr hBitmap;
            IntPtr hDC = Win32Stuff.GetDC(Win32Stuff.GetDesktopWindow());
            IntPtr hMemDC = GDIStuff.CreateCompatibleDC(hDC);

            size.cx = Win32Stuff.GetSystemMetrics(Win32Stuff.SM_CXSCREEN);
            size.cy = Win32Stuff.GetSystemMetrics(Win32Stuff.SM_CYSCREEN);

            hBitmap = GDIStuff.CreateCompatibleBitmap(hDC, size.cx, size.cy);

            if (hBitmap != IntPtr.Zero)
            {
                IntPtr hOld = (IntPtr)GDIStuff.SelectObject(hMemDC, hBitmap);

                GDIStuff.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, GDIStuff.SRCCOPY);
                
                Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                var bits = bmp.LockBits(new Rectangle(0, 0, size.cx, size.cy), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bmp.UnlockBits(bits);

                GDIStuff.SelectObject(hMemDC, hOld);
                GDIStuff.DeleteDC(hMemDC);
                Win32Stuff.ReleaseDC(Win32Stuff.GetDesktopWindow(), hDC);

                GDIStuff.DeleteObject(hBitmap);
                GC.Collect();
            }

        }

        private void GetScreenshotNew(byte[] buffer)
        {
            var bitmap = CaptureScreen.CaptureDesktopWithCursor();

            //return BitmapToByteArray(bitmap);

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                //return stream.ToArray();
            }
        }



        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            //BitmapData bmpdata = null;
            //byte[] bytes = new byte[bmpData.Width * bmpData.Height * 3];
            //for (int y = 0; y < bmpData.Height; ++y)
            //{
            //    //bmpData.Scan0 + y * bmpData.Stride
            //    IntPtr mem = (IntPtr)((long)bmpData.Scan0 + y * bmpData.Stride);
            //    Marshal.Copy(mem, bytes, y * bmpData.Width * 3, bmpData.Width); //This is where the exception is pointed.
            //}


            BitmapData bmpData = null;

            try
            {
                bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                byte[] bytes = new byte[bmpData.Width * bmpData.Height * 3];
                for (int y = 0; y < bmpData.Height; ++y)
                {
                    //bmpData.Scan0 + y * bmpData.Stride
                    IntPtr mem = (IntPtr)((long)bmpData.Scan0 + y * bmpData.Stride);
                    Marshal.Copy(mem, bytes, y * bmpData.Width * 3, bmpData.Width); //This is where the exception is pointed.
                }

                return bytes;
            }
            finally
            {
                if (bmpData != null)
                    bitmap.UnlockBits(bmpData);
            }

        }

        private void GetScreenshotEmpty(byte[] buffer)
        {
            using (var bitmap = new Bitmap(screenWidth, screenHeight))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawRectangle(new Pen(Color.White), 0, 0, screenWidth, screenHeight);

                    var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                    bitmap.UnlockBits(bits);
                }
            }
        }
    }
}