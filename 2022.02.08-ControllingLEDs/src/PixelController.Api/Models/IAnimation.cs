namespace PixelController.Api.Models;

public interface IAnimation
{
    void Execute(Settings settings, CancellationToken token);
}