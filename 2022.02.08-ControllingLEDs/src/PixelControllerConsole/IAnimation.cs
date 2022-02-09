namespace PixelControllerConsole;

public interface IAnimation
{
    void Execute(CancellationToken token);
}