using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace PushbulletOVRlay
{
    class Utils
    {
        // borrowed from https://git.io/Jv66q
        public static void FixBitmapForOpenVR(ref Bitmap bitmap)
        {
            // Flip R & B channels in bitmap so it displays correctly
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat
            );
            int length = Math.Abs(data.Stride) * bitmap.Height;
            unsafe
            {
                byte* rgbValues = (byte*)data.Scan0.ToPointer();
                for (int i = 0; i < length; i += bytesPerPixel)
                {
                    byte dummy = rgbValues[i];
                    rgbValues[i] = rgbValues[i + 2];
                    rgbValues[i + 2] = dummy;
                }
            }

            bitmap.UnlockBits(data);
        }
    }
}
