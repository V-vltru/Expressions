namespace LinearAlgebraicEquationsSystem
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class MatrixT<T>
    {
        /// <summary>
        /// Gets or sets the flag which describes should matrix operations to be performed in parallel mode.
        /// </summary>
        public static bool Paral { get; set; }

        /// <summary>
        /// Gets or sets matrix items.
        /// </summary>
        public T[,] Elements { get; set; }

        /// <summary>
        /// Gets or sets amount of rows.
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Gets or sets amount of columns.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// Gets or sets a total amount of matrix items.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Indexator which provides the access to matrix items;
        /// </summary>
        /// <param name="i">Index by rows.</param>
        /// <param name="j">Index by columns.</param>
        /// <returns>Matrix element according its indexes.</returns>
        public T this[int i, int j] //индексатор
        {
            get
            {
                return Elements[i, j];
            }
            set
            {
                Elements[i, j] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixT<typeparamref name="T"/>" /> class.
        /// </summary>
        /// <param name="rows">Amount of rows.</param>
        /// <param name="cols">Amount of columns.</param>
        public MatrixT(int rows, int cols)
        {
            this.Rows = rows;
            this.Columns = cols;
            Elements = new T[this.Rows, this.Columns];
            Length = rows * cols;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixT<typeparamref name="T"/>" /> class.
        /// </summary>
        /// <param name="_elements">Matrix elements</param>
        public MatrixT(T[,] _elements)
        {
            this.Elements = _elements;

            this.Rows = this.Elements.GetLength(0);
            this.Columns = this.Elements.GetLength(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixT<typeparamref name="T"/>" /> class.
        /// </summary>
        /// <param name="_elements">Array of elements.</param>
        public MatrixT(T[] _elements)
        {
            this.Elements = new T[_elements.Length, 1];

            for (int i = 0; i < _elements.Length; i++)
            {
                this.Elements[i, 1] = _elements[i];
            }

            this.Rows = _elements.Length;
            this.Columns = 1;
        }

        /// <summary>
        /// Method describes two matrixes equation process.
        /// </summary>
        /// <param name="obj">Matrix to compare.</param>
        /// <returns>Flag if two matrixes are equal.</returns>
        public override bool Equals(object obj)
        {
            MatrixT<T> inputMatrix = (MatrixT<T>)obj;

            if (this.Columns != inputMatrix.Columns || this.Rows != inputMatrix.Rows)
            {
                return false;
            }

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if ((dynamic)this.Elements[i, j] != (dynamic)inputMatrix[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Summarizing operator overloading.
        /// </summary>
        /// <param name="A">Left operand.</param>
        /// <param name="B">Right operand.</param>
        /// <returns>The result of summarizing operator.</returns>
        public static MatrixT<T> operator +(MatrixT<T> A, MatrixT<T> B)
        {
            if (Paral)
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), A.Elements.GetLength(1)]);
                Parallel.For(0, A.Elements.GetLength(0), (i) =>
                {
                    for (int j = 0; j < A.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)(A.Elements[i, j]) + (dynamic)(B.Elements[i, j]);
                    }
                });

                return ans;
            }
            else
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), A.Elements.GetLength(1)]);
                for (int i = 0; i < A.Elements.GetLength(0); i++)
                {
                    for (int j = 0; j < A.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)(A.Elements[i, j]) + (dynamic)(B.Elements[i, j]);
                    }
                }

                return ans;
            }
        }

        /// <summary>
        /// Multiplication operator overloading.
        /// </summary>
        /// <param name="A">Left operand.</param>
        /// <param name="B">Right operand.</param>
        /// <returns>The result of multiplication operand.</returns>
        public static MatrixT<T> operator *(MatrixT<T> A, MatrixT<T> B)
        {
            if (Paral)
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), B.Elements.GetLength(1)]);
                Parallel.For(0, A.Elements.GetLength(0), (i) =>
                {
                    for (int j = 0; j < B.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)0;
                        for (int k = 0; k < A.Elements.GetLength(1); k++)
                        {
                            ans.Elements[i, j] += (dynamic)A.Elements[i, k] * (dynamic)B.Elements[k, j];
                        }
                    }
                });

                return ans;
            }
            else
            {
                MatrixT<T> ans = new MatrixT<T>(new T[A.Elements.GetLength(0), B.Elements.GetLength(1)]);
                for (int i = 0; i < A.Elements.GetLength(0); i++)
                {
                    for (int j = 0; j < B.Elements.GetLength(1); j++)
                    {
                        ans.Elements[i, j] = (dynamic)0;
                        for (int k = 0; k < A.Elements.GetLength(1); k++)
                        {
                            ans.Elements[i, j] += (dynamic)A.Elements[i, k] * (dynamic)B.Elements[k, j];
                        }
                    }
                }

                return ans;
            }
        }

        /// <summary>
        /// Multiplication by a number overloading.
        /// </summary>
        /// <param name="a">Number to multiply a matrix.</param>
        /// <param name="B">Matrix which will be multiplied.</param>
        /// <returns>The result of multiplication by a number.</returns>
        public static MatrixT<T> operator *(dynamic a, MatrixT<T> B)
        {
            Type type = a.GetType();
            bool isNumber = (type.IsPrimitive && type != typeof(bool) && type != typeof(char));

            if (isNumber)
            {
                MatrixT<T> result = new MatrixT<T>(B.Rows, B.Columns);

                for (int i = 0; i < B.Rows; i++)
                {
                    for (int j = 0; j < B.Columns; j++)
                    {
                        result[i, j] = B[i, j] * a;
                    }
                }

                return result;
            }

            throw new ArgumentException($"Cannot multiply the matrix by a number {a}.");
        }

        /// <summary>
        /// Method is used to get matrix rang.
        /// </summary>
        /// <param name="matrix">Initial matrix to get its rang.</param>
        /// <returns>Matrix rang.</returns>
        public static int GetRang(MatrixT<T> matrix)
        {
            int rank = 0;
            int q = 1;

            while (q <= MatrixT<int>.GetMinValue(matrix.Elements.GetLength(0), matrix.Elements.GetLength(1)))
            {
                MatrixT<T> matbv = new MatrixT<T>(q, q);

                for (int i = 0; i < (matrix.Elements.GetLength(0) - (q - 1)); i++)
                {
                    for (int j = 0; j < (matrix.Elements.GetLength(1) - (q - 1)); j++)
                    {
                        for (int k = 0; k < q; k++)
                        {
                            for (int c = 0; c < q; c++)
                            {
                                matbv[k, c] = matrix[i + k, j + c];
                            }
                        }

                        if (MatrixT<T>.GetMatrixDeterminant(matbv) != 0)
                        {
                            rank = q;
                        }
                    }               
                }

                q++;
            }

            return rank;
        }

        /// <summary>
        /// Method is used to get a determinant of the required matrix.
        /// </summary>
        /// <param name="matrix">Initial matrix to get determinant.</param>
        /// <returns>Matrix determinant.</returns>
        public static double GetMatrixDeterminant(MatrixT<T> matrix)
        {
            if (matrix.Elements.Length == 1)
            {
                return (dynamic)matrix[0, 0];
            }

            if (matrix.Elements.Length == 4)
            {
                return (dynamic)matrix[0, 0] * (dynamic)matrix[1, 1] - (dynamic)matrix[0, 1] * (dynamic)matrix[1, 0];
            }

            double sign = 1;
            double result = 0;

            for(int i = 0; i < matrix.Elements.GetLength(1); i++)
            {
                T[,] minor = MatrixT<T>.GetMinor(matrix.Elements, i);
                result += sign * (dynamic)matrix[0, i] * GetMatrixDeterminant(new MatrixT<T>(minor));

                sign = -sign;
            }

            return result;
        }

        /// <summary>
        /// Method is used to add one column to a matrix.
        /// </summary>
        /// <param name="matrix">Initial matrix.</param>
        /// <param name="extension">Extra column which will be added.</param>
        /// <returns>The extended matrix.</returns>
        public static MatrixT<T> ExtendMatrix(MatrixT<T> matrix, T[] extension)
        {
            MatrixT<T> result = new MatrixT<T>(matrix.Rows, matrix.Columns + 1);

            for(int i = 0; i < matrix.Rows; i++)
            {
                for(int j = 0; j < matrix.Columns; j++)
                {
                    result[i, j] = matrix[i, j];
                }

                result[i, result.Columns - 1] = extension[i];
            }

            return result;
        }

        /// <summary>
        /// Method is used to take inversed matrix.
        /// </summary>
        /// <param name="matrix">Initial matrix.</param>
        /// <returns>Inversed matrix.</returns>
        public static MatrixT<T> GetInverseMatrix3(MatrixT<T> matrix)
        {
            if (matrix.Columns == matrix.Rows)
            {
                if (MatrixT<T>.GetMatrixDeterminant(matrix) != 0.0)
                {
                    MatrixT<T> matrixCopy = new MatrixT<T>(matrix.Rows, matrix.Columns);
                    
                    for (int i = 0; i < matrix.Rows; i++)
                    {
                        for (int j = 0; j < matrix.Columns; j++)
                        {
                            matrixCopy[i, j] = matrix[i, j];
                        }
                    }

                    MatrixT<T> reverseMatrix = new MatrixT<T>(matrix.Rows, matrix.Columns);
                    MatrixT<T>.SetBaseMatrix(reverseMatrix);

                    for (int k = 0; k < matrix.Rows; k++)
                    {
                        T div = matrixCopy[k, k];
                        for(int m = 0; m < matrix.Columns; m++)
                        {
                            matrixCopy[k, m] /= (dynamic)div;
                            reverseMatrix[k, m] /= (dynamic)div;
                        }

                        for(int i = k + 1; i < matrix.Rows; i++)
                        {
                            T multi = matrixCopy[i, k];
                            for(int j = 0; j < matrix.Columns; j++)
                            {
                                matrixCopy[i, j] -= (dynamic)multi * (dynamic)matrixCopy[k, j];
                                reverseMatrix[i, j] -= (dynamic)multi * (dynamic)reverseMatrix[i, j];
                            }
                        }
                    }

                    for (int kk = matrix.Rows - 1; kk > 0; kk--)
                    {
                        matrixCopy[kk, matrix.Columns - 1] /= (dynamic)matrixCopy[kk, kk];
                        reverseMatrix[kk, matrix.Columns - 1] /= (dynamic)matrixCopy[kk, kk];

                        for (int i = kk - 1; i + 1 > 0; i--)
                        {
                            T multi2 = matrixCopy[i, kk];
                            for (int j = 0; j < matrix.Columns; j++)
                            {
                                matrixCopy[i, j] -= (dynamic)multi2 * (dynamic)matrixCopy[kk, j];
                                reverseMatrix[i, j] -= (dynamic)multi2 * (dynamic)reverseMatrix[kk, j];
                            }
                        }
                    }

                    return MatrixT<T>.TransponeMatrix(reverseMatrix);
                }
            }

            return null;
        }

        /// <summary>
        /// Method is used to take inversed matrix.
        /// </summary>
        /// <param name="matrix">Initial matrix.</param>
        /// <returns>Inversed matrix.</returns>
        public static MatrixT<T> GetInverseMatrix(MatrixT<T> matrix)
        {            
            double determinant = MatrixT<T>.GetMatrixDeterminant(matrix);

            if (determinant != 0)
            {
                MatrixT<T> reversedMatrix = new MatrixT<T>(matrix.Rows, matrix.Columns);

                for (int i = 0; i < matrix.Rows; i++)
                {
                    for (int j = 0; j < matrix.Columns; j++)
                    {
                        MatrixT<T> tempMatrix = MatrixT<T>.GetMinor(matrix, i, j);
                        reversedMatrix[i, j] = (dynamic)Math.Pow(-1.0, i + j + 2) * (dynamic)MatrixT<T>.GetMatrixDeterminant(tempMatrix) / (dynamic)determinant;
                    }
                }

                return MatrixT<T>.TransponeMatrix(reversedMatrix);
            }

            return null;
        }

        /// <summary>
        /// Method is used to transpone the required matrix.
        /// </summary>
        /// <param name="matrix">Initial matrix.</param>
        /// <returns>Transponed matrix of the initial one.</returns>
        public static MatrixT<T> TransponeMatrix(MatrixT<T> matrix)
        {
            MatrixT<T> result = new MatrixT<T>(matrix.Columns, matrix.Rows);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        #region Private helpers

        private static T[,] GetMinor(T[,] matrix, int n)
        {
            T[,] result = new T[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i - 1, j] = matrix[i, j];
                }

                for(int j = n + 1; j < matrix.GetLength(0); j++)
                {
                    result[i - 1, j - 1] = matrix[i, j];
                }
            }

            return result;
        }

        private static T GetMinValue(T firstItem, T secondItem)
        {
            if ((dynamic)firstItem >= (dynamic)secondItem)
            {
                return secondItem;
            }

            return firstItem;
        }

        private static void SetBaseMatrix(MatrixT<T> matrix)
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = (dynamic)1;
                    }
                    else
                    {
                        matrix[i, j] = (dynamic)0;
                    }
                }
            }
        }

        private static MatrixT<T> GetMinor(MatrixT<T> matrix, int indRow, int indCol)
        {
            MatrixT<T> result = new MatrixT<T>(matrix.Rows - 1, matrix.Columns - 1);

            int ki = 0;

            for (int i = 0; i < matrix.Rows; i++)
            {
                if (i != indRow)
                {
                    for (int j = 0, kj = 0; j < matrix.Columns; j++)
                    {
                        if (j != indCol)
                        {
                            result[ki, kj] = matrix[i, j];
                            kj++;
                        }
                    }

                    ki++;
                }
            }

            return result;
        }

        #endregion

        public static void PrintMatrix(MatrixT<T> matrix, StreamWriter streamWriter = null)
        {
            if (streamWriter == null)
            {
                streamWriter = new StreamWriter(Console.OpenStandardOutput());
                streamWriter.AutoFlush = true;
                Console.SetOut(streamWriter);
            }

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    streamWriter.Write($"{matrix[i, j]} ");
                }

                streamWriter.WriteLine();
            }
        }
    }
}
