using PushbulletOVRlay.Pushbullet;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Valve.VR;

namespace PushbulletOVRlay
{
    class Program
    {
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        static void Main()
        {
#if DEBUG
            AllocConsole();
#endif

            var e = EVRInitError.None;
            OpenVR.Init(ref e, EVRApplicationType.VRApplication_Overlay);

            ulong handle = 0;
            var error = OpenVR.Overlay.CreateOverlay("pushbullet", "Pushbullet", ref handle);
            Console.WriteLine($"CreateOverlay Result: {error.ToString()}");

            var conf = Common.Config.Read();
            var stream = new PushbulletStream(conf.PushbulletToken);

            stream.MessageReceived += (sender, push) =>
            {
                OpenVR.Overlay.SetOverlayName(handle, $"Pushbullet - {push.ApplicationName}");

                uint id = 0;

                var notifIcon = new NotificationBitmap_t();

                var bmp = push.IconAsBitmap();
                Utils.FixBitmapForOpenVR(ref bmp);
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                notifIcon.m_pImageData = bmpData.Scan0;
                notifIcon.m_nWidth = bmpData.Width;
                notifIcon.m_nHeight = bmpData.Height;
                notifIcon.m_nBytesPerPixel = 4;

                var notifError = OpenVR.Notifications.CreateNotification(handle, 0, EVRNotificationType.Transient, $"{push.Title.Replace("\n", " / ")}\n{push.Body.Replace("\n", " / ")}", EVRNotificationStyle.Application, ref notifIcon, ref id);
                Console.WriteLine($"CreateNotification Result: {notifError.ToString()}");
                Console.WriteLine($"Received mirror push: [{push.ApplicationName}] {push.Title}: {push.Body}");

                bmp.UnlockBits(bmpData);
            };

            stream.Start();

            var res = new ManualResetEvent(false);

            VREvent_t ev = new VREvent_t();
            var size = (uint)Marshal.SizeOf(typeof(VREvent_t));
            while (OpenVR.Overlay.PollNextOverlayEvent(handle, ref ev, size))
            {
                if (ev.eventType == (uint)EVREventType.VREvent_Quit)
                {
                    Console.WriteLine("SteamVR is quitting, shutting down...");
                    OpenVR.Shutdown();
                    res.Set();
                    Environment.Exit(0);
                }
            }

            res.WaitOne();
        }
    }
}
