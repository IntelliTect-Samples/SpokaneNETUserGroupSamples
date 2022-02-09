using System.Drawing;
using rpi_ws281x;

namespace PixelControllerConsole;

    public class ColorWipe : IAnimation
    {
        const int Brightness = 125;
        public void Execute(CancellationToken token)
        {
            var ledCount = 300;
            var settings = Settings.CreateDefaultSettings();

            var controller = settings.AddController(ledCount, Pin.Gpio18, StripType.WS2812_STRIP);
            controller.Brightness = Brightness;

            using (var device = new WS281x(settings))
            {
                while (!token.IsCancellationRequested)
                {
                    Wipe(device, Color.DarkOrange, token);
                    Wipe(device, Color.Purple, token);
                    Wipe(device, Color.SaddleBrown, token);
                    Wipe(device, Color.ForestGreen, token);
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