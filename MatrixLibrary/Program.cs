using MatrixLibrary;
using System.Diagnostics;

int matrixSize = 10;
int incrementer = 0;

Matrix<double> matrixOne = new(matrixSize, matrixSize);
Matrix<double> matrixTwo = new(matrixSize, matrixSize);

for (int i = 0; i < matrixSize; i++)
{
    for (int j = 0; j < matrixSize; j++)
    {
        matrixOne[i, j] = j + incrementer;

        matrixTwo[i, j] = i == j ? 1 : 0;
    }

    incrementer += matrixSize;
}

Stopwatch stopwatch = Stopwatch.StartNew();
Run(true);
stopwatch.Stop();
Console.WriteLine("Test run over. Starting actual test.");

for (int k = 0; k < 10; k++)
{
    Run();
    Thread.Sleep(100);
}

void Run(bool printResults = false)
{
    stopwatch.Restart();

    Matrix<double> resultMatrix = matrixOne * matrixTwo;

    stopwatch.Stop();

    string[,] resultStrings = resultMatrix.GetFormattedValues(4);

    Console.WriteLine("Elapsed Time (HH:MM:SS.SSSSSSS): " + stopwatch.Elapsed);
    
    if (printResults)
        PrintResult(resultStrings);


    Console.WriteLine();
}

void PrintResult(string[,] resultMatrix)
{
    for (int i = 0; i < resultMatrix.GetLength(0); i++)
    {
        for (int j = 0; j < resultMatrix.GetLength(1); j++)
        {
            Console.Write(resultMatrix[i, j] + " ");
        }

        Console.WriteLine();
    }
}