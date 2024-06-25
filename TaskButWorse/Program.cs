using TaskButWorse;

WorseTask<int> additionTask = WorseTask<int>.Run(() =>
{
    int result = 0;

    for (int i = 0; i < 10; i++)
    {
        result += i;
        Thread.Sleep(2);
    }

    return result;
});

WorseTask<int> multiplicationTask = WorseTask<int>.Run(() =>
{
    int result = 1;

    for (int i = 1; i <= 10 ; i++)
    {
        result *= i;
        Thread.Sleep(10);
    }

    return result;
});

Console.WriteLine("Other work");
var result = await WorseTask<int>.WhenAny(additionTask, multiplicationTask);
var slowResult = result == additionTask ? multiplicationTask : additionTask;
Console.WriteLine("Multiply finished first: " + (result == multiplicationTask).ToString());
Console.WriteLine(await result!);
Console.WriteLine(await slowResult);

