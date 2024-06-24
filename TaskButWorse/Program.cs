// See https://aka.ms/new-console-template for more information
using TaskButWorse;
WorseTask t = new();
t.Run(() =>
{
    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(10);
        Console.WriteLine(i);
    }
});

await t;
Console.WriteLine("Next stuff");