using ListButWorse;

ListButWorse<int> ints = [];

for (int i = 0; i < 10; i++)
    ints.Add(i);

foreach (int i in ints)
    Console.WriteLine(i);

if (ints.Contains(5))
    Console.WriteLine("Got em");

for (int i = 0; i < 4; i++)
    ints.Remove(i);

foreach (int i in ints.Where(i => i % 2 == 0))
    Console.WriteLine(i);

for (int i = 0; i < 4; i++)
    ints.Insert(i, i);

foreach (int i in ints.Where(i => i % 2 == 0))
    Console.WriteLine(i);

Console.WriteLine(ints.Count);
Console.WriteLine(ints[2]);
