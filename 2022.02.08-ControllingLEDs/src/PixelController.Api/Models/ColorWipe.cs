using System.Drawing;
using rpi_ws281x;

namespace PixelController.Api.Models;

    public class ColorWipe : IAnimation
    {
        const int Brightness = 125;
        public void Execute(Settings settings, CancellationToken token)
        {
            var lightSettings = rpi_ws281x.Settings.CreateDefaultSettings();
            var controller = lightSettings.AddController(settings.LEDCount, Pin.Gpio18, StripType.WS2812_STRIP);
            controller.Brightness = settings.Brightness;

            using (var device = new WS281x(lightSettings))
            {
                while (!token.IsCancellationRequested)
                {
                    foreach (var color in settings.Colors)
                    {
                        Wipe(device, color, token);
                    }
                }
                device.Reset();
            }
        }

        private static void Wipe(WS281x device, Color color, CancellationToken token)
        {
            var controller = device.GetController();
            for (int i = 0; i < controller.LEDCount; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                controller.SetLED(i, color);
                device.Render();

                var waitPeriod = (int)Math.Max(500.0 / controller.LEDCount, 5.0);
                Thread.Sleep(waitPeriod);
            }
        }
    }