using System.Numerics;

namespace MatrixLibrary
{
    public partial class Matrix<T> where T : INumber<T>
    {
        /// <summary>
        /// Multiplies two matrices together. Assumes formatting is [rows, columns].
        /// </summary>
        /// <example>
        /// If the return value is the answer you expect from multiplying <paramref name="secondMatrix"/> by <paramref name="firstMatrix"/>
        /// instead of what you intended, this typically means you've formatted your matrices as
        /// [columns, rows]. The following code example should fix your issue.
        /// <code>
        /// Matrix.Multiply(Matrix.SwapDimensions(secondMatrix), Matrix.SwapDimensions(firstMatrix));
        /// </code>
        /// </example>
        /// <param name="firstMatrix">The first matrix to multiply.</param>
        /// <param name="secondMatrix">The second matrix to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the matrices have incompatible dimensions.</exception>
        public static T[,] Multiply(T[,] firstMatrix, T[,] secondMatrix)
        {
            ArgumentNullException.ThrowIfNull(firstMatrix);
            ArgumentNullException.ThrowIfNull(secondMatrix);

            int firstMatrixRowCount = firstMatrix.GetLength(0);
            int firstMatrixColumnCount = firstMatrix.GetLength(1);
            int secondMatrixRowCount = secondMatrix.GetLength(0);
            int secondMatrixColumnCount = secondMatrix.GetLength(1);

            if (firstMatrixColumnCount != secondMatrixRowCount)
                throw new InvalidOperationException($"Matrix size mismatch. First matrix columns {firstMatrixColumnCount}, second matrix rows {secondMatrixRowCount}.");

            T[,] resultMatrix = new T[firstMatrixRowCount, secondMatrixColumnCount];

            Parallel.For(0, firstMatrixRowCount, i =>
            {
                T[] firstMatrixRow = GetRow(firstMatrix, i);

                for (int j = 0; j < secondMatrixColumnCount; j++)
                {
                    T[] secondMatrixColumn = GetColumn(secondMatrix, j);

                    resultMatrix[i, j] = firstMatrixRow[0] * secondMatrixColumn[0];

                    for (int k = 1; k < firstMatrixRow.Length; k++)
                    {
                        resultMatrix[i, j] += firstMatrixRow[k] * secondMatrixColumn[k];
                    }
                }
            });

            return resultMatrix;
        }

        /// <summary>
        /// Multiplies two matrices together. Assumes formatting is [rows][columns].
        /// </summary>
        /// <example>
        /// If the return value is the answer you expect from multiplying <paramref name="secondMatrix"/> by <paramref name="firstMatrix"/>
        /// instead of what you intended, this typically means you've formatted your matrices as
        /// [columns][rows]. The following code example should fix your issue.
        /// <code>
        /// Matrix.Multiply(Matrix.SwapDimensions(secondMatrix), Matrix.SwapDimensions(firstMatrix));
        /// </code>
        /// </example>
        /// <param name="firstMatrix">The first matrix to multiply.</param>
        /// <param name="secondMatrix">The second matrix to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the matrices have incompatible dimensions.</exception>
        public static T[][] Multiply(T[][] firstMatrix, T[][] secondMatrix)
        {
            // Convert firstMatrix to a matrix of type T[,]
            ArgumentNullException.ThrowIfNull(firstMatrix);
            ArgumentNullException.ThrowIfNull(secondMatrix);
            T[,] convertedFirstMatrix = ConvertToTwoDimensionalArray(firstMatrix);
            T[,] convertedSecondMatrix = ConvertToTwoDimensionalArray(secondMatrix);
            T[,] convertedResultMatrix = Multiply(convertedFirstMatrix, convertedSecondMatrix);

            return ConvertToArrayOfArrays(convertedResultMatrix);
        }


        /// <summary>
        /// Multiplies <paramref name="matrix"/> by <paramref name="multiplier"/>.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="multiplier">The numeric value the matrix will be multiplied by.</param>
        /// <returns>A new matrix that is the product of the <paramref name="matrix"/> and the <paramref name="multiplier"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T[,] Multiply(T[,] matrix, T multiplier)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            ArgumentNullException.ThrowIfNull(multiplier);

            T[,] resultMatrix = new T[matrix.GetLength(0), matrix.GetLength(1)];

            Parallel.For(0, matrix.GetLength(0), i =>
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    T cellValue = matrix[i, j];
                    resultMatrix[i, j] = cellValue * (dynamic)multiplier;
                }
            });

            return resultMatrix;
        }

        /// <summary>
        /// Multiplies <paramref name="matrix"/> by <paramref name="multiplier"/>.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="multiplier">The numeric value the matrix will be multiplied by.</param>
        /// <returns>A new matrix that is the product of the <paramref name="matrix"/> and the <paramref name="multiplier"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T[][] Multiply(T[][] matrix, T multiplier)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            ArgumentNullException.ThrowIfNull(multiplier);
            return ConvertToArrayOfArrays(Multiply(ConvertToTwoDimensionalArray(matrix), multiplier));
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="firstMatrix">The first matrix to add.</param>
        /// <param name="secondMatrix">The second matrix to add.</param>
        /// <returns>The result of adding the two matrices together.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[,] Add(T[,] firstMatrix, T[,] secondMatrix)
        {
            ArgumentNullException.ThrowIfNull(firstMatrix);
            ArgumentNullException.ThrowIfNull(secondMatrix);

            int firstMatrixRowCount = firstMatrix.GetLength(0);
            int firstMatrixColumnCount = firstMatrix.GetLength(1);
            int secondMatrixRowCount = secondMatrix.GetLength(0);
            int secondMatrixColumnCount = secondMatrix.GetLength(1);

            if (firstMatrixRowCount != secondMatrixRowCount || firstMatrixColumnCount != secondMatrixColumnCount)
                throw new InvalidOperationException("Matrix size mismatch. Matrices must be the exact same size.");

            T[,] resultMatrix = new T[firstMatrixRowCount, firstMatrixColumnCount];

            Parallel.For(0, firstMatrixRowCount, i =>
            {
                for (int j = 0; j < firstMatrixColumnCount; j++)
                {
                    resultMatrix[i, j] = firstMatrix[i, j] + secondMatrix[i, j];
                }
            });

            return resultMatrix;
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="firstMatrix">The first matrix to add.</param>
        /// <param name="secondMatrix">The second matrix to add.</param>
        /// <returns>The result of adding the two matrices together.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[][] Add(T[][] firstMatrix, T[][] secondMatrix)
        {
            ArgumentNullException.ThrowIfNull(firstMatrix);
            ArgumentNullException.ThrowIfNull(secondMatrix);
            T[,] convertedFirstMatrix = ConvertToTwoDimensionalArray(firstMatrix);
            T[,] convertedSecondMatrix = ConvertToTwoDimensionalArray(secondMatrix);
            T[,] convertedResultMatrix = Add(convertedFirstMatrix, convertedSecondMatrix);

            return ConvertToArrayOfArrays(convertedResultMatrix);
        }

        /// <summary>
        /// Adds <paramref name="scalar"/> to every value in <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The first matrix to add.</param>
        /// <param name="scalar">The scalar value to add to the matrix.</param>
        /// <returns>The result of adding the scalar to the matrix.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[,] Add(T[,] matrix, T scalar)
        {
            ArgumentNullException.ThrowIfNull(matrix);

            int firstMatrixRowCount = matrix.GetLength(0);
            int firstMatrixColumnCount = matrix.GetLength(1);

            T[,] resultMatrix = new T[firstMatrixRowCount, firstMatrixColumnCount];

            for (int i = 0; i < firstMatrixRowCount; i++)
            {
                for (int j = 0; j < firstMatrixColumnCount; j++)
                {
                    resultMatrix[i, j] = matrix[i, j] + scalar;
                }
            }

            return resultMatrix;
        }

        /// <summary>
        /// Adds <paramref name="scalar"/> to every value in <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The first matrix to add.</param>
        /// <param name="scalar">The scalar value to add to the matrix.</param>
        /// <returns>The result of adding the scalar to the matrix.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[][] Add(T[][] matrix, T scalar)
        {
            ArgumentNullException.ThrowIfNull(matrix);

            return ConvertToArrayOfArrays(Add(ConvertToTwoDimensionalArray(matrix), scalar));
        }

        /// <summary>
        /// Subtracts the <paramref name="secondMatrix"/> from the <paramref name="firstMatrix"/>.
        /// </summary>
        /// <param name="firstMatrix">The matrix to subtract from.</param>
        /// <param name="secondMatrix">The matrix to subtract.</param>
        /// <returns>The result of subtracting the <paramref name="secondMatrix"/> from the <paramref name="firstMatrix"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[,] Subtract(T[,] firstMatrix, T[,] secondMatrix)
        {
            ArgumentNullException.ThrowIfNull(firstMatrix);
            ArgumentNullException.ThrowIfNull(secondMatrix);

            if (firstMatrix.GetLength(0) != secondMatrix.GetLength(0)
             || firstMatrix.GetLength(1) != secondMatrix.GetLength(1))
                throw new InvalidOperationException("Matrix size mismatch. Matrices must be the exact same size.");

            return Add(firstMatrix, Multiply(secondMatrix, -T.One));
        }

        /// <summary>
        /// Subtracts the <paramref name="secondMatrix"/> from the <paramref name="firstMatrix"/>.
        /// </summary>
        /// <param name="firstMatrix">The matrix to subtract from.</param>
        /// <param name="secondMatrix">The matrix to subtract.</param>
        /// <returns>The result of subtracting the <paramref name="secondMatrix"/> from the <paramref name="firstMatrix"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[][] Subtract(T[][] firstMatrix, T[][] secondMatrix)
        {
            ArgumentNullException.ThrowIfNull(firstMatrix);
            ArgumentNullException.ThrowIfNull(secondMatrix);
            T[,] convertedFirstMatrix = ConvertToTwoDimensionalArray(firstMatrix);
            T[,] convertedSecondMatrix = ConvertToTwoDimensionalArray(secondMatrix);
            T[,] convertedResultMatrix = Subtract(convertedFirstMatrix, convertedSecondMatrix);

            return ConvertToArrayOfArrays(convertedResultMatrix);
        }

        /// <summary>
        /// Subtracts the <paramref name="scalar"/> from the <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix to subtract from.</param>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <returns>The result of subtracting the <paramref name="scalar"/> from the <paramref name="matrix"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[,] Subtract(T[,] matrix, T scalar)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            return Add(matrix, -scalar);
        }

        /// <summary>
        /// Subtracts the <paramref name="scalar"/> from the <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix to subtract from.</param>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <returns>The result of subtracting the <paramref name="scalar"/> from the <paramref name="matrix"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static T[][] Subtract(T[][] matrix, T scalar)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            ArgumentNullException.ThrowIfNull(scalar);
            T[,] convertedMatrix = ConvertToTwoDimensionalArray(matrix);
            T[,] convertedResultMatrix = Subtract(convertedMatrix, scalar);

            return ConvertToArrayOfArrays(convertedResultMatrix);
        }

        /// <summary>
        /// Swaps the rows and columns of <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix you want to swap the dimensions of.</param>
        /// <returns>A copy of <paramref name="matrix"/> with the dimensions swapped.</returns>
        public static T[,] SwapDimensions(T[,] matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            T[,] swappedMatrix = new T[columns, rows];

            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < columns; j++)
                {
                    swappedMatrix[j, i] = matrix[i, j];
                }
            });

            return swappedMatrix;
        }

        /// <summary>
        /// Swaps the rows and columns of <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix you want to swap the dimensions of.</param>
        /// <returns>A copy of <paramref name="matrix"/> with the dimensions swapped.</returns>
        public static T[][] SwapDimensions(T[][] matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            return ConvertToArrayOfArrays(SwapDimensions(ConvertToTwoDimensionalArray(matrix)));
        }

        private static T[,] ConvertToTwoDimensionalArray(T[][] matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            T[,] result = new T[matrix.Length, matrix[0].Length];

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    result[i, j] = matrix[i][j];
                }
            }

            return result;
        }

        private static T[][] ConvertToArrayOfArrays(T[,] matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            T[][] result = new T[matrix.GetLength(0)][];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                result[i] = new T[matrix.GetLength(1)];
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i][j] = matrix[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Pulls out a column from <paramref name="matrix"/> at index <paramref name="columnNumber"/>. 
        /// </summary>
        /// <param name="matrix">Matrix to get the column from.</param>
        /// <param name="columnNumber">The index of the column to grab.</param>
        /// <returns>An <see cref="Array"/> of type <typeparamref name="T"/>.</returns>
        public static T[] GetColumn(T[,] matrix, int columnNumber)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            var length = matrix.GetLength(0);
            var resultMatrix = new T[length];

            for (int i = 0; i < length; i++)
            {
                resultMatrix[i] = matrix[i, columnNumber];
            }

            return resultMatrix;
        }

        /// <summary>
        /// Pulls out a row from <paramref name="matrix"/> at index <paramref name="columnNumber"/>. 
        /// </summary>
        /// <param name="matrix">Matrix to get the row from.</param>
        /// <param name="rowNumber">The index of the row to grab.</param>
        /// <returns>An <see cref="Array"/> of type <typeparamref name="T"/>.</returns>
        public static T[] GetRow(T[,] matrix, int rowNumber)
        {
            ArgumentNullException.ThrowIfNull(matrix);
            var length = matrix.GetLength(1);
            var resultMatrix = new T[length];

            for (int i = 0; i < length; i++)
            {
                resultMatrix[i] = matrix[rowNumber, i];
            }

            return resultMatrix;
        }
    }
}
