using WorseTask;

int result = await WorseTask<int>.Run(() =>
{
    int result = 0;
    for (int i = 0; i < 10; i++)
    {
        result += i;
        Thread.Sleep(100);
    }
    return result;
});

Console.WriteLine(result);
Console.WriteLine("Next stuff");

