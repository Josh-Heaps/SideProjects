﻿using System.Collections;
using System.Numerics;

namespace MatrixLibrary
{
    public partial class Matrix<T> : IEnumerable<T>, IEnumerable where T : INumber <T>
    {
        private T[,] _values;
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

        public static Matrix<T> operator *(Matrix<T> matrix, T multiplier) => matrix.Multiply(multiplier);
        public static Matrix<T> operator *(Matrix<T> matrixOne, Matrix<T> matrixTwo) => matrixOne.Multiply(matrixTwo);
        public static Matrix<T> operator +(Matrix<T> matrixOne, Matrix<T> matrixTwo) => matrixOne.Add(matrixTwo);

        public bool SwapDimensionsToggle { get; set; }

        public Matrix(T[,] matrix)
        {
            _values = matrix;
        }

        public Matrix(T[][] matrix)
        {
            _values = ConvertToTwoDimensionalArray(matrix);
        }

        public Matrix(int rows, int columns)
        {
            _values = new T[rows, columns];
        }

        public Matrix<T> Multiply(Matrix<T> matrix)
        {
            Matrix<T> result = new(Multiply(this.Values, matrix.Values))
            {
                SwapDimensionsToggle = this.SwapDimensionsToggle
            };

            return Matrix<T>.CreateMatrixCopy(result);
        }

        public Matrix<T> Multiply(T multiplier)
        {
            Matrix<T> result = new(Multiply(this.Values, multiplier))
            {
                SwapDimensionsToggle = this.SwapDimensionsToggle
            };

            return Matrix<T>.CreateMatrixCopy(result);
        }

        public Matrix<T> Add(Matrix<T> matrix)
        {
            Matrix<T> result = new(Add(this.Values, matrix.Values))
            {
                SwapDimensionsToggle = this.SwapDimensionsToggle
            };

            return Matrix<T>.CreateMatrixCopy(result);
        }

        public T[] GetRow(int index) => GetRow(Values, index);
        
        public T[] GetColumn(int index) => GetColumn(Values, index);

        public T[][] ConvertToArrayOfArrays() => ConvertToArrayOfArrays(Values);

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

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void ForEach(Action<T> action)
        {
            foreach (T value in this)
                action(value);
        }

        public List<T> GetEachByPredicate(Predicate<T> predicate)
        {
            List<T> values = [];

            foreach (T value in this)
                if (predicate(value))
                    values.Add(value);

            return values;
        }

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

        private static Matrix<T> CreateMatrixCopy(Matrix<T> matrix)
        {
            T[,] values = matrix.SwapDimensionsToggle ? SwapDimensions(matrix.Values) : matrix.Values;

            Matrix<T> result = new(values)
            {
                SwapDimensionsToggle = matrix.SwapDimensionsToggle
            };

            return result;
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private Indexer index;
            private readonly Matrix<T> matrix;

            public Enumerator(Matrix<T> matrix)
            {
                index = new(0, 0);
                Current = matrix[index.X, index.Y];
                this.matrix = matrix;
            }

            public readonly void Dispose() { }

            public bool MoveNext()
            {
                if (index.X == index.MaxX && index.Y == index.MaxY)
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

            private class Indexer
            {
                public int X { get; set; }
                public int Y { get; set; }
                public int MaxX { get; set; }
                public int MaxY { get; set; }
                public (int,int) Index
                {
                    get
                    {
                        return (X, Y);
                    }
                    set
                    {
                        X = value.Item1;
                        Y = value.Item2;
                    }
                }

                public static Indexer operator ++(Indexer index)
                {
                    if (index.X == index.MaxX)
                    {
                        index.X = 0;
                        index.Y++;
                    }
                    else
                    {
                        index.X++;
                    }

                    if (index.Y == index.MaxY)
                        index.Index = (0, 0);

                    return index;
                }

                public Indexer(int x, int y)
                {
                    X = x;
                    Y = y;
                }
            }
        }
    }
}
