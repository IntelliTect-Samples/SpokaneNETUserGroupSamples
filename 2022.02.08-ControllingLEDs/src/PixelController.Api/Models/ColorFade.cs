using System.Drawing;
using rpi_ws281x;

namespace PixelController.Api.Models;

public class ColorFade : IAnimation
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
                    Fade(device, color, token);
                }
            }
            device.Reset();
        }
    }

    private static void Fade(WS281x device, Color color, CancellationToken token)
    {
        if (!token.IsCancellationRequested)
        {
            var controller = device.GetController();

            controller.SetAll(color);

            device.Render();

            Thread.Sleep(2000);
            }
    }
}