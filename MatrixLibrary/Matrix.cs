using System.Numerics;

namespace MatrixLibrary
{
    public partial class Matrix<T> where T : INumber <T>
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

        public bool SwapDimensionsToggle { get; set; }

        public Matrix(T[,] matrix)
        {
            _values = matrix;
        }

        public Matrix(T[][] matrix)
        {
            _values = ConvertToTwoDimensionalArray(matrix);
        }

        public Matrix<T> Multiply(Matrix<T> matrix)
        {
            Matrix<T> result = new(Multiply(this.Values, matrix.Values))
            {
                SwapDimensionsToggle = this.SwapDimensionsToggle
            };

            return CreateMatrixCopy(result);
        }

        public Matrix<T> Multiply(T multiplier)
        {
            Matrix<T> result = new(Multiply(this.Values, multiplier))
            {
                SwapDimensionsToggle = this.SwapDimensionsToggle
            };

            return CreateMatrixCopy(result);
        }

        public Matrix<T> Add(Matrix<T> matrix)
        {
            Matrix<T> result = new(Add(this.Values, matrix.Values))
            {
                SwapDimensionsToggle = this.SwapDimensionsToggle
            };

            return CreateMatrixCopy(result);
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

        private Matrix<T> CreateMatrixCopy(Matrix<T> matrix)
        {
            T[,] values = matrix.SwapDimensionsToggle ? SwapDimensions(matrix.Values) : matrix.Values;

            Matrix<T> result = new(values)
            {
                SwapDimensionsToggle = matrix.SwapDimensionsToggle
            };

            return result;
        }
    }
}
