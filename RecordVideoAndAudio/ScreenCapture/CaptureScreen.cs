using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace ScreenshotCaptureWithMouse.ScreenCapture
{
    /// <summary>
    /// https://www.codeproject.com/Articles/12850/Capturing-the-Desktop-Screen-with-the-Mouse-Cursor
    /// </summary>
    class CaptureScreen
    {
        //This structure shall be used to keep the size of the screen.
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

        static Bitmap CaptureDesktop()
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

                GDIStuff.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC,0, 0, GDIStuff.SRCCOPY);

                GDIStuff.SelectObject(hMemDC, hOld);
                GDIStuff.DeleteDC(hMemDC);
                Win32Stuff.ReleaseDC(Win32Stuff.GetDesktopWindow(), hDC);
                Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                GDIStuff.DeleteObject(hBitmap);
                GC.Collect();
                return bmp;
            }
            return null;

        }

        public static Icon CaptureCursorIcon(out int x, out int y)
        {
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                hicon = Win32Stuff.CopyIcon(ci.hCursor);
                if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                {
                    x = ci.ptScreenPos.x - icInfo.xHotspot;
                    y = ci.ptScreenPos.y - icInfo.yHotspot;
                    return Icon.FromHandle(hicon);

                }
                else
                {
                    x = ci.ptScreenPos.x;
                    y = ci.ptScreenPos.y;
                }
            }
            else
            {
                x = 0;
                y = 0;
            }
            return null;
        }
        public static Bitmap CaptureCursor(out int x, out int y)
        {
            Bitmap bmp;
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        x = ci.ptScreenPos.x - icInfo.xHotspot;
                        y = ci.ptScreenPos.y - icInfo.yHotspot;

                        Icon ic = Icon.FromHandle(hicon);
                        bmp = ic.ToBitmap();
                        return bmp;
                    }
                }
            }
            x = 0;
            y = 0;
            return null;
        }

        public static Bitmap CaptureDesktopWithCursor()
        {
            Bitmap desktopBMP;
            Bitmap cursorBMP = null;
            Graphics g;
            Rectangle r;
            int cursorX = 0;
            int cursorY = 0;
            desktopBMP = CaptureDesktop();
            try
            {
                cursorBMP = CaptureCursor(out cursorX, out cursorY);
            }
            catch (Exception)
            {
            }
            if (desktopBMP != null)
            {
                if (cursorBMP != null)
                {
                    r = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                    g = Graphics.FromImage(desktopBMP);
                    g.DrawImage(cursorBMP, r);
                    g.Flush();

                    return desktopBMP;
                }
                else
                    return desktopBMP;
            }

            return null;

        }


        public static void GetScreenshotAll(byte[] buffer, int screenWidth, int screenHeight)
        {
            try
            {
                GetScreenshot(buffer, screenWidth, screenHeight);
            }
            catch (Exception)
            {
                GetScreenshotEmpty(buffer, screenWidth, screenHeight);
            }
        }
        private static void GetScreenshot(byte[] buffer, int screenWidth, int screenHeight)
        {
            using (var bitmap = new Bitmap(screenWidth, screenHeight))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(screenWidth, screenHeight));

                    var icon = ScreenshotCaptureWithMouse.ScreenCapture.CaptureScreen.CaptureCursorIcon(out int cursorX, out int cursorY);
                    if (icon != null)
                    {
                        try
                        {
                            graphics.DrawIcon(icon, cursorX, cursorY);
                        }
                        catch (Exception)
                        {
                            graphics.DrawEllipse(new Pen(Color.White), cursorX, cursorY, 10, 10);
                        }
                    }
                    else
                    {
                        graphics.DrawEllipse(new Pen(Color.White), cursorX, cursorY, 10, 10);
                    }

                    var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                    Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                    bitmap.UnlockBits(bits);
                }
            }
        }
        private static void GetScreenshotEmpty(byte[] buffer, int screenWidth, int screenHeight)
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
