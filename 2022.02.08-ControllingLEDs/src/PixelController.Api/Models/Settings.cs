using System.Drawing;

namespace PixelController.Api.Models;

public class Settings
{
    public byte Brightness { get; set; }
    public Pattern Pattern { get; set; }
    public List<Color> Colors { get; set; } = new List<Color>();
    public int Duration { get; set; }
    public int Steps { get; set; }
    public int LEDCount { get; set; } = 300;
}