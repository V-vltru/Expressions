using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebraicEquationsSystem
{
    public partial class LinearAlgebraicEquationSystem
    {
        public List<LAEVariable> CalculateMatrixMethod()
        {
            MatrixT<double> inversedMatrix = MatrixT<double>.GetInverseMatrix(this.Matrix);

            MatrixT<double> resultMatrix = inversedMatrix * new MatrixT<double>(this.RightPartEquations.ToArray());

            return null;
        }
    }
}
