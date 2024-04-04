internal class Program
{
    public static void Main()
    {
        var mainClass = new MainClass();
        mainClass.Run();
    }
}

internal class MainClass
{
    private ChildClass child;

    public void Run()
    {
        child = new ChildClass();
        child._onTimeOut += OnExceptionOccured;

        while (true)
        {
            child.StartTimer();
            var task = Task.Run(() => Thread.Sleep(500));
            task.Wait();
        }
    }

    private void OnExceptionOccured(object? sender, UnhandledExceptionEventArgs args)
    {
        Console.WriteLine(args.ExceptionObject.ToString());
    }
}

internal class ChildClass
{
    public async void StartTimer()
    {
        await Task.Run(() => Thread.Sleep(250));
        _onTimeOut?.Invoke(this, new UnhandledExceptionEventArgs(new TimeoutException("Time ran out"), false));
    }

    public EventHandler<UnhandledExceptionEventArgs>? _onTimeOut;
}