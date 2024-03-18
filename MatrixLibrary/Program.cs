using MatrixLibrary;
using System.Diagnostics;

int matrixSize = 10;
int incrementer = 0;

//double[,] arrayOne = new double[matrixSize, matrixSize];
//double[,] arrayTwo = new double[matrixSize, matrixSize];

//for (int i = 0; i < matrixSize; i++)
//{
//    for (int j = 0; j < matrixSize; j++)
//    {
//        arrayOne[i, j] = j + incrementer;

//        arrayTwo[i, j] = i == j ? 1 : 0;
//    }

//    incrementer += matrixSize;
//}


Matrix<double> matrixOne = new(matrixSize, matrixSize);
//new double[4, 6]
//{
//    { 1.111, 2, 3, 4, 5, 6 },
//    { 12, 11.111, 10, 9, 8, 7 },
//    { 13, 14, 15, 16, 17, 18 },
//    { 24, 23, 22, 21, 20, 19 }
//});

//long[,] matrixTwoArray = new long[6, 6];

//for (int i = 0; i < 6; i++)
//    for (int j = 0; j < 6; j++)
//        matrixTwoArray[i, j] = i == j ? 1 : 0;

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

//new double[6, 3]
//{
//    { 2, 4, 6 },
//    { 12, 10, 8 },
//    { 14, 16, 18 },
//    { 24, 22, 20 },
//    { 26, 28, 30 },
//    { 36, 34, 32 }
//});

//int incrementer = 1;
//int matrixSize = 500;

//int[,] matrixOne = new int[matrixSize, matrixSize];

//int[,] matrixTwo = new int[matrixSize, matrixSize];

//for (int i = 0; i < matrixSize; i++)
//{
//    for (int j = 0; j < matrixSize; j++)
//    {
//        matrixOne[i, j] = j + incrementer;
//        matrixTwo[i, j] = i == j ? 1 : 0;
//    }

//    incrementer += matrixSize;
//}

//long[,] matrixOneLong = new long[2, 2];
//long[,] matrixTwoLong = new long[2, 2];

//matrixOneLong[0, 0] = 1;
//matrixOneLong[0, 1] = 2;
//matrixOneLong[1, 0] = 3;
//matrixOneLong[1, 1] = 4;

//matrixTwoLong[0, 0] = 2;
//matrixTwoLong[0, 1] = 0;
//matrixTwoLong[1, 0] = 1;
//matrixTwoLong[1, 1] = 2;

//decimal[,] matrixOneDouble = new decimal[2, 2];
//decimal[,] matrixTwoDouble = new decimal[2, 2];

//matrixOneDouble[0, 0] = 1;
//matrixOneDouble[0, 1] = 2;
//matrixOneDouble[1, 0] = 3;
//matrixOneDouble[1, 1] = 4;

//matrixTwoDouble[0, 0] = 2;
//matrixTwoDouble[0, 1] = 0;
//matrixTwoDouble[1, 0] = 1;
//matrixTwoDouble[1, 1] = 2;

/*
 * Matrix One:
 * [1 2]
 * [3 4]
 * 
 * Matrix Two:
 * [2 0]
 * [1 2]
 */

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

    Matrix<double> resultMatrixLong = matrixOne * matrixTwo;

    stopwatch.Stop();

    string[,] resultMatrix = resultMatrixLong.GetFormattedValues(4);

    Console.WriteLine("Elapsed Ticks: " + stopwatch.ElapsedTicks);
    
    if (printResults)
        PrintResult(resultMatrix);

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