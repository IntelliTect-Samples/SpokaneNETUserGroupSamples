using PixelControllerConsole;

var cts = new CancellationTokenSource();
var token = cts.Token;

var animations = GetAnimations();

var input = 0;

Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

do
{
    Console.Clear();
    Console.WriteLine("What do you want to test:");
    Console.WriteLine();
    Console.WriteLine("0 - Exit");
    Console.WriteLine("1 - Color Wipe Animation");
    Console.WriteLine("2 - Rainbow Color Animation");
    Console.WriteLine("3 - Color Fade");
    Console.WriteLine();
    Console.WriteLine("Press Ctrl+C to abort current test.");
    Console.WriteLine();
    Console.Write("What is your choice: ");
    input = Int32.Parse(Console.ReadLine());

    if (animations.ContainsKey(input))
    {
        Task.Run(() => animations[input].Execute(token), token).Wait();
        cts = new CancellationTokenSource();
        token = cts.Token;
    }
} while (input != 0);

static Dictionary<int, IAnimation> GetAnimations()
{
    var result = new Dictionary<int, IAnimation>();

    result[1] = new ColorWipe();
    result[2] = new RainbowColorAnimation();
    result[3] = new ColorWipe();

    return result;
}