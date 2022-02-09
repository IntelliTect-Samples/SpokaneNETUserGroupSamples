using System.Drawing;
using rpi_ws281x;

namespace PixelControllerConsole;

public class RainbowColorAnimation : IAnimation
    {
        private static int colorOffset = 0;
        const int Brightness = 125;

        public void Execute(CancellationToken token)
        {
            var ledCount = 300;
            var settings = Settings.CreateDefaultSettings();

            var controller = settings.AddController(ledCount, Pin.Gpio18, StripType.WS2812_STRIP);
            controller.Brightness = Brightness;
            
            using (var device = new WS281x(settings))
            {
                var colors = GetAnimationColors();
                while (!token.IsCancellationRequested)
                {
                    for (int i = 0; i < controller.LEDCount; i++)
                    {
                        var colorIndex = (i + colorOffset) % colors.Count;
                        controller.SetLED(i, colors[colorIndex]);
                    }
                    device.Render();
                    colorOffset = (colorOffset + 1) % colors.Count;

                    Thread.Sleep(500);
                }
                device.Reset();
            }
        }

        public static List<Color> GetAnimationColors()
        {
            var result = new List<Color>();

            result.Add(Color.DarkOrange);
            result.Add(Color.Purple);
            result.Add(Color.SaddleBrown);
            result.Add(Color.ForestGreen);

            return result;
        }

    }