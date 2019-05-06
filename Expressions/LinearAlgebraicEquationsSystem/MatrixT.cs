namespace LinearAlgebraicEquationsSystem
{
    using System.Threading.Tasks;

    public class MatrixT<T>
    {
        public static bool Paral { get; set; }

        public T[,] Elements { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public int Length { get; set; }

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

        public MatrixT(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            Elements = new T[this.Rows, this.Cols];
            Length = rows * cols;
        }

        public MatrixT(T[,] _elements)
        {
            this.Elements = _elements;

            this.Rows = this.Elements.GetLength(0);
            this.Cols = this.Elements.GetLength(1);
        }

        // Перегружаем бинарный оператор +
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

        // Перегружаем бинарный оператор *
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

        public static int GetRank(MatrixT<T> matrix)
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

        public static double GetMatrixDeterminant(MatrixT<T> matrix)
        {
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

        public static MatrixT<T> ExtendMatrix(MatrixT<T> matrix, T[] extension)
        {
            MatrixT<T> result = new MatrixT<T>(matrix.Rows, matrix.Cols + 1);

            for(int i = 0; i < matrix.Rows; i++)
            {
                for(int j = 0; j < matrix.Cols; j++)
                {
                    result[i, j] = matrix[i, j];
                }

                result[i, result.Cols - 1] = extension[i];
            }

            return result;
        }

        public static MatrixT<T> GetInverseMatrix(MatrixT<T> matrix)
        {
            if (matrix.Cols == matrix.Rows)
            {
                if (MatrixT<T>.GetMatrixDeterminant(matrix) != 0.0)
                {
                    MatrixT<T> matrixCopy = new MatrixT<T>(matrix.Rows, matrix.Cols);
                    
                    for (int i = 0; i < matrix.Rows; i++)
                    {
                        for (int j = 0; j < matrix.Cols; j++)
                        {
                            matrixCopy[i, j] = matrix[i, j];
                        }
                    }

                    MatrixT<T> reverseMatrix = new MatrixT<T>(matrix.Rows, matrix.Cols);
                    MatrixT<T>.SetBaseMatrix(reverseMatrix);

                    for (int k = 0; k < matrix.Rows; k++)
                    {
                        T div = matrixCopy[k, k];
                        for(int m = 0; m < matrix.Cols; m++)
                        {
                            matrixCopy[k, m] /= (dynamic)div;
                            reverseMatrix[k, m] /= (dynamic)div;
                        }

                        for(int i = k + 1; i < matrix.Rows; i++)
                        {
                            T multi = matrixCopy[i, k];
                            for(int j = 0; j < matrix.Cols; j++)
                            {
                                matrixCopy[i, j] -= (dynamic)multi * (dynamic)matrixCopy[k, j];
                                reverseMatrix[i, j] -= (dynamic)multi * (dynamic)reverseMatrix[i, j];
                            }
                        }
                    }

                    for (int kk = matrix.Rows - 1; kk > 0; kk--)
                    {
                        matrixCopy[kk, matrix.Cols - 1] /= (dynamic)matrixCopy[kk, kk];
                        reverseMatrix[kk, matrix.Cols - 1] /= (dynamic)matrixCopy[kk, kk];

                        for (int i = kk - 1; i + 1 > 0; i--)
                        {
                            T multi2 = matrixCopy[i, kk];
                            for (int j = 0; j < matrix.Cols; j++)
                            {
                                matrixCopy[i, j] -= (dynamic)multi2 * (dynamic)matrixCopy[kk, j];
                                reverseMatrix[i, j] -= (dynamic)multi2 * (dynamic)reverseMatrix[kk, j];
                            }
                        }
                    }

                    return reverseMatrix;
                }
            }

            return null;
        }

        #region Helpers

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
                for (int j = 0; j < matrix.Cols; j++)
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

        #endregion
    }
}
