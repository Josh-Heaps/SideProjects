using WorseTask;

Console.WriteLine("Start task");

WorseTask<int> task = WorseTask<int>.Run(() =>
{
    int result = 0;

    for (int i = 0; i < 10; i++)
    {
        result += i;
        Thread.Sleep(100);
    }

    return result;
});

Console.WriteLine("Other work");
Console.WriteLine(await task);

