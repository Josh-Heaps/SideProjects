using System;
using System.Collections;
using System.Numerics;

namespace MatrixLibrary
{
    public partial class Matrix<T> : IEnumerable<T>, IEnumerable where T : INumber<T>
    {
        private T[,] _values;
        /// <summary>
        /// Get or set the values of the matrix. If the SwapDimensionsToggle is true, the dimensions of the matrix will be swapped.
        /// </summary>
        public T[,] Values
        {
            get
            {
                return SwapDimensionsToggle ? SwapDimensions(_values) : _values;
            }
            set
            {
                _values = value;
            }
        }

        /// <summary>
        /// Get or set the value at the specified index.
        /// </summary>
        /// <param name="i">X index of the matrix.</param>
        /// <param name="j">Y index of the matrix.</param>
        /// <returns>A type <typeparamref name="T"/> at the specified index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when i or j are not in range of the matrix.</exception>
        public T this[int i, int j]
        {
            get
            {
                if (i > Values.GetLength(0) || i < 0
                 || j > Values.GetLength(1) || j < 0)
                    throw new System.IndexOutOfRangeException();

                return Values[i, j];
            }
            set
            {
                if (i > Values.GetLength(0) || i < 0
                 || j > Values.GetLength(1) || j < 0)
                    throw new System.IndexOutOfRangeException();

                Values[i, j] = value;
            }
        }

        /// <summary>
        /// Multiplies two matrices together. Assumes formatting is [rows, columns].
        /// </summary>
        /// <example>
        /// If the return value is the answer you expect from multiplying <paramref name="matrixTwo"/> by <paramref name="matrixOne"/>
        /// instead of what you intended, this typically means you've formatted your matrices as
        /// [columns, rows]. The following code example should fix your issue.
        /// <code>
        /// Matrix.Multiply(Matrix.SwapDimensions(matrixTwo), Matrix.SwapDimensions(matrixOne));
        /// </code>
        /// </example>
        /// <param name="matrixOne">The first matrix to multiply.</param>
        /// <param name="matrixTwo">The second matrix to multiply.</param>
        /// <returns>The product of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the matrices have incompatible dimensions.</exception>
        public static Matrix<T> operator *(Matrix<T> matrixOne, Matrix<T> matrixTwo) => matrixOne.Multiply(matrixTwo);
        /// <summary>
        /// Multiplies <paramref name="matrix"/> by <paramref name="multiplier"/>.
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="multiplier">The numeric value the matrix will be multiplied by.</param>
        /// <returns>A new matrix that is the product of the <paramref name="matrix"/> and the <paramref name="multiplier"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Matrix<T> operator *(Matrix<T> matrix, T multiplier) => matrix.Multiply(multiplier);
        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="matrixOne">The first matrix to add.</param>
        /// <param name="matrixTwo">The second matrix to add.</param>
        /// <returns>The result of adding the two matrices together.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix<T> operator +(Matrix<T> matrixOne, Matrix<T> matrixTwo) => matrixOne.Add(matrixTwo);
        /// <summary>
        /// Adds <paramref name="scalar"/> to every value in <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The first matrix to add.</param>
        /// <param name="scalar">The scalar value to add to the matrix.</param>
        /// <returns>The result of adding the scalar to the matrix.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix<T> operator +(Matrix<T> matrix, T scalar) => matrix.Add(scalar);
        /// <summary>
        /// Adds <see cref="T.One"/> to every value in <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The first matrix to add.</param>
        /// <returns>The result of adding <see cref="T.One"/> to the matrix.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix<T> operator ++(Matrix<T> matrix) => matrix.Add(T.One);
        /// <summary>
        /// Subtracts the <paramref name="matrixTwo"/> from the <paramref name="matrixOne"/>.
        /// </summary>
        /// <param name="matrixOne">The matrix to subtract from.</param>
        /// <param name="matrixTwo">The matrix to subtract.</param>
        /// <returns>The result of subtracting the <paramref name="matrixTwo"/> from the <paramref name="matrixOne"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix<T> operator -(Matrix<T> matrixOne, Matrix<T> matrixTwo) => matrixOne.Subtract(matrixTwo);
        /// <summary>
        /// Subtracts the <paramref name="scalar"/> from the <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix to subtract from.</param>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <returns>The result of subtracting the <paramref name="scalar"/> from the <paramref name="matrix"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix<T> operator -(Matrix<T> matrix, T scalar) => matrix.Subtract(scalar);
        /// <summary>
        /// Subtracts <see cref="T.One"/> from the <paramref name="matrix"/>.
        /// </summary>
        /// <param name="matrix">The matrix to subtract from.</param>
        /// <returns>The result of subtracting <see cref="T.One"/> from the <paramref name="matrix"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Matrix<T> operator --(Matrix<T> matrix) => matrix.Subtract(T.One);
        
        /// <summary>
        /// The toggle for swapping the dimensions of the matrix.
        /// </summary>
        public bool SwapDimensionsToggle { get; set; }

        /// <summary>
        /// Create a new matrix with the specified values.
        /// </summary>
        /// <param name="matrix">A two dimensional array</param>
        public Matrix(T[,] matrix, bool swapDimensions = false)
        {
            _values = matrix;
            SwapDimensionsToggle = swapDimensions;
        }

        /// <summary>
        /// Create a new matrix with the specified values.
        /// </summary>
        /// <param name="matrix">An array of arrays.</param>
        public Matrix(T[][] matrix, bool swapDimensions = false)
        {
            _values = ConvertToTwoDimensionalArray(matrix);
            SwapDimensionsToggle = swapDimensions;
        }

        /// <summary>
        /// Create a new matrix with the specified dimensions.
        /// </summary>
        /// <param name="rows">Number of rows in the matrix.</param>
        /// <param name="columns">Number of columns in the matrix.</param>
        public Matrix(int rows, int columns)
        {
            _values = new T[rows, columns];
        }

        /// <summary>
        /// Multiplies <see langword="this"/> by <paramref name="matrix"/>. Assumes formatting is [rows, columns].
        /// </summary>
        /// <example>
        /// If the return value is the answer you expect from multiplying <see langword="this"/> by <paramref name="matrix"/>
        /// instead of what you intended, this typically means you've formatted your matrices as
        /// [columns, rows]. The following code example should fix your issue.
        /// <code>
        /// Matrix.Multiply(Matrix.SwapDimensions(matrixTwo), Matrix.SwapDimensions(matrixOne));
        /// </code>
        /// </example>
        /// <param name="matrix">The matrix to multiply with the current instance of <see langword="this"/>.</param>
        /// <returns>The product of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the matrices have incompatible dimensions.</exception>
        public Matrix<T> Multiply(Matrix<T> matrix) =>
            Matrix<T>.CreateMatrixCopy(new(Multiply(Values, matrix.Values), SwapDimensionsToggle));

        /// <summary>
        /// Multiplies <see langword="this"/> by <paramref name="multiplier"/>.
        /// </summary>
        /// <param name="multiplier">The numeric value the matrix will be multiplied by.</param>
        /// <returns>A new matrix that is the product of the <see langword="this"/> and the <paramref name="multiplier"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Matrix<T> Multiply(T multiplier) =>
            Matrix<T>.CreateMatrixCopy(new(Multiply(Values, multiplier), SwapDimensionsToggle));

        /// <summary>
        /// Adds <paramref name="matrix"/> to <see langword="this"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix{T}"/> to add to <see langword="this"/>.</param>
        /// <returns>A <see langword="new"/> <see cref="Matrix{T}"/>, where the values are the sum of the two matrices.</returns>
        public Matrix<T> Add(Matrix<T> matrix) =>
            Matrix<T>.CreateMatrixCopy(new(Add(Values, matrix.Values), SwapDimensionsToggle));

        /// <summary>
        /// Adds <paramref name="scalar"/> to every value in <see langword="this"/>.
        /// </summary>
        /// <param name="scalar">The <typeparamref name="T"/> to add to <see langword="this"/>.</param>
        /// <returns>A <see langword="new"/> <see cref="Matrix{T}"/>, where the values are the sum of <paramref name="scalar"/> and the values in <see langword="this"/>.</returns>
        public Matrix<T> Add(T scalar) =>
            Matrix<T>.CreateMatrixCopy(new(Add(Values, scalar), SwapDimensionsToggle));

        /// <summary>
        /// Subtracts <paramref name="matrix"/> from <see langword="this"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix{T}"/> to subtract from <see langword="this"/>.</param>
        /// <returns>A <see langword="new"/> <see cref="Matrix{T}"/>, where the values are the difference of the two matrices.</returns>
        public Matrix<T> Subtract(Matrix<T> matrix) =>
            Matrix<T>.CreateMatrixCopy(new(Subtract(Values, matrix.Values), SwapDimensionsToggle));

        /// <summary>
        /// Subtracts <paramref name="scalar"/> from every value in <see langword="this"/>.
        /// </summary>
        /// <param name="scalar">The <typeparamref name="T"/> to subtract from <see langword="this"/>.</param>
        /// <returns>A <see langword="new"/> <see cref="Matrix{T}"/>, where the values are the difference of <paramref name="scalar"/> and the values in <see langword="this"/>.</returns>
        public Matrix<T> Subtract(T scalar) =>
            Matrix<T>.CreateMatrixCopy(new(Subtract(Values, scalar), SwapDimensionsToggle));

        /// <summary>
        /// Gets the row at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the desired row.</param>
        /// <returns>The <see cref="Array"/> at <paramref name="index"/>.</returns>
        public T[] GetRow(int index) => GetRow(Values, index);

        /// <summary>
        /// Gets the column at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index of the desired column.</param>
        /// <returns>The <see cref="Array"/> at <paramref name="index"/>.</returns>
        public T[] GetColumn(int index) => GetColumn(Values, index);

        /// <summary>
        /// Converts the matrix to an array of arrays.
        /// </summary>
        /// <returns>An array of arrays that holds the values of the matrix.</returns>
        public T[][] ConvertToArrayOfArrays() => ConvertToArrayOfArrays(Values);

        /// <summary>
        /// Converts the matrix to a 2 dimensional array of strings, formatted to the specified decimal place.
        /// </summary>
        /// <param name="decimalPlace">Max number of digits after the decimal.</param>
        /// <returns>A 2 dimensional array of formatted values.</returns>
        public string[,] GetFormattedValues(int decimalPlace = 2)
        {
            int maxLength = 0;
            bool hasDecimal = false;
            string[,] result = new string[Values.GetLength(0), Values.GetLength(1)];

            for (int i = 0; i < Values.GetLength(0); i++)
                for (int j = 0; j < Values.GetLength(1); j++)
                    if ((Values[i, j]?.ToString() ?? "").Contains('.'))
                        hasDecimal = true;

            for (int i = 0; i < Values.GetLength(0); i++)
            {
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    string text = Values[i, j]?.ToString() ?? "";
                    bool tempHasDecimal = text.Contains('.');

                    if (tempHasDecimal && text.Length > text.LastIndexOf('.') + decimalPlace)
                        text = text[..(text.LastIndexOf('.') + decimalPlace)];
                    if (tempHasDecimal && text.Length < text.LastIndexOf('.') + decimalPlace)
                        while (text.Length < text.LastIndexOf('.') + decimalPlace)
                            text += "0";
                    if (hasDecimal && !text.Contains('.'))
                    {
                        text += ".";
                        while (text.Length < text.LastIndexOf('.') + decimalPlace)
                            text += "0";
                    }

                    int length = text.Length;
                    result[i, j] = text;

                    if (length > maxLength) maxLength = length;
                }
            }

            for (int i = 0; i < Values.GetLength(0); i++)
            {
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    while (result[i, j].Length < maxLength)
                        result[i, j] = " " + result[i, j];
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Performs the specified action on each element of the matrix.
        /// </summary>
        /// <param name="action">A <see langword="delegate"/> that takes a parameter, and returns nothing.</param>
        public void ForEach(Action<T> action)
        {
            foreach (T value in this)
                action(value);
        }

        /// <summary>
        /// Returns every element that matches the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The condition to check each value against.</param>
        /// <returns>Every element that matches the specified <paramref name="predicate"/>.</returns>
        public List<T> GetEachByPredicate(Predicate<T> predicate)
        {
            List<T> values = [];

            foreach (T value in this)
                if (predicate(value))
                    values.Add(value);

            return values;
        }

        /// <summary>
        /// Converts the matrix to a 2 <see cref="List{T}"/> of <see cref="List{T}"/>s.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="List{T}"/>s.</returns>
        public List<List<T>> ConvertTo2DList()
        {
            List<List<T>> list = [];

            for (int i = 0; i < Values.GetLength(0); i++)
            {
                List<T> innerList = [];

                for (int j = 0; j < Values.GetLength(1); j++)
                    innerList.Add(Values[i, j]);

                list.Add(innerList);
            }

            return list;
        }

        /// <summary>
        /// Creates a copy of the <paramref name="matrix"/>, without copying the reference.
        /// </summary>
        /// <param name="matrix">The matrix to copy.</param>
        /// <returns>A copy of <paramref name="matrix"/>.</returns>
        private static Matrix<T> CreateMatrixCopy(Matrix<T> matrix) =>
            new(matrix.SwapDimensionsToggle ? SwapDimensions(matrix.Values) : matrix.Values, matrix.SwapDimensionsToggle);
        

        /// <summary>
        /// Used to get the <see cref="IEnumerable"/> of the matrix.
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private Indexer index;
            private readonly Matrix<T> matrix;

            public Enumerator(Matrix<T> matrix)
            {
                index = new(0, 0)
                {
                    MaxX = matrix.Values.GetLength(0) - 1,
                    MaxY = matrix.Values.GetLength(1) - 1
                };

                Current = matrix[index.X, index.Y];
                this.matrix = matrix;
            }

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                if (index.X > index.MaxX)
                    return false;

                Current = matrix[index.X, index.Y];
                index++;
                return true;
            }

            public void Reset()
            {
                index = new(0, 0);
                Current = matrix[index.X, index.Y];
            }

            public T Current { get; private set; }

            readonly object System.Collections.IEnumerator.Current => Current;
        }
    }

    internal class Indexer(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public (int x, int y) Index
        {
            get
            {
                return (X, Y);
            }
            set
            {
                X = value.x;
                Y = value.y;
            }
        }

        public static Indexer operator ++(Indexer index)
        {
            if (index.Y == index.MaxX)
            {
                index.Y = 0;
                index.X++;
            }
            else
            {
                index.Y++;
            }

            return index;
        }
    }
}
