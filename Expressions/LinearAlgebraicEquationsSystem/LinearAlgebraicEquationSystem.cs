using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressions;
using Expressions.Models;

namespace LinearAlgebraicEquationsSystem
{
    public class LinearAlgebraicEquationSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAlgebraicEquationSystem" /> class.
        /// </summary>
        /// <param name="leftPartEquations">Left part of linear algebraic equations.</param>
        /// <param name="rightPartEquations">Right part of linear algebraic equations.</param>
        /// <param name="variables">Variables of linear algebraic equations.</param>
        /// <param name="constants">Other constant parameters of linear algebraic equations.</param>
        public LinearAlgebraicEquationSystem(List<string> leftPartEquations, List<double> rightPartEquations, 
            List<LAEVariable> variables, List<LAEVariable> constants)
        {
            if (rightPartEquations != null && rightPartEquations.Count > 0)
            {
                this.RightPartEquations = rightPartEquations;
            }
            else
            {
                throw new ArgumentException("Right parts list is null or empty!");
            }

            List<Variable> allVariables = new List<Variable>();

            if (variables != null && variables.Count > 0)
            {
                this.Variables = variables.Cast<Variable>().ToList();
                allVariables.AddRange(this.Variables);
            }
            else
            {
                throw new ArgumentException("Variables list is null or empty!");
            }

            if (constants != null && constants.Count > 0)
            {
                this.Constants = constants.Cast<Variable>().ToList();
                allVariables.AddRange(this.Constants);
            }
           
            this.LeftPartEquations = new List<Expression>();
            foreach(string leftPart in leftPartEquations)
            {
                this.LeftPartEquations.Add(new Expression(leftPart, allVariables));
            }

            this.Matrix = LinearAlgebraicEquationSystem.SetMatrix(this.LeftPartEquations, this.Variables, this.Constants);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAlgebraicEquationSystem" /> class.
        /// </summary>
        public LinearAlgebraicEquationSystem()
        {
        }

        /// <summary>
        /// Gets or sets the left parts of equations.
        /// </summary>
        public List<Expression> LeftPartEquations { get; set; }

        /// <summary>
        /// Gets or sets the right parts of equations.
        /// </summary>
        public List<double> RightPartEquations { get; set; }

        /// <summary>
        /// Gets or sets equations system variables.
        /// </summary>
        public List<Variable> Variables { get; set; }

        /// <summary>
        /// Gets or sets equations system constants.
        /// </summary>
        public List<Variable> Constants { get; set; }

        /// <summary>
        /// Gets or sets the matrix of the linear algebraicequation system.
        /// </summary>
        public MatrixT<double> Matrix { get; set; }

        /// <summary>
        /// Method is used to chek if the input variables a correct and can be a solution for the system.
        /// </summary>
        /// <param name="allVariables">Input variables to check.</param>
        /// <returns>The the flag which represents if the proposed solution is correct.</returns>
        public bool LinearAlgebraicEquationSystemResult(List<LAEVariable> allVariables)
        {
            for(int i = 0; i < this.LeftPartEquations.Count; i++)
            {
                if(this.LeftPartEquations[i].GetResultValue(allVariables.Cast<Variable>().ToList()) != this.RightPartEquations[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckLinearAlgebraicEquationSystemCompatibility()
        {
            int matrixRank = MatrixT<double>.GetRank(this.Matrix);

            MatrixT<double> extendedMatrix = MatrixT<double>.ExtendMatrix(this.Matrix, this.RightPartEquations.ToArray());
            int extendedMatrixRank = MatrixT<double>.GetRank(extendedMatrix);

            if (matrixRank == extendedMatrixRank)
            {
                return true;
            }

            return false;
        }

        #region Helpers

        /// <summary>
        /// Method is used to set matrix of linear alebraic equation system.
        /// </summary>
        /// <param name="leftEquationsParts">parts of the left equations system.</param>
        /// <param name="variables">LAE system variables.</param>
        /// <param name="constants">LAE system constants.</param>
        /// <returns>matrix of the relevant linear alebraic equation system.</returns>
        private static MatrixT<double> SetMatrix(List<Expression> leftEquationsParts, List<Variable> variables, List<Variable> constants)
        {
            MatrixT<double> result = new MatrixT<double>(leftEquationsParts.Count, variables.Count);

            for (int i = 0; i < leftEquationsParts.Count; i++)
            {
                for (int j = 0; j < variables.Count; j++)
                {
                    List<Variable> currentVariables = variables;

                    LinearAlgebraicEquationSystem.SetVariablesWithValues(currentVariables, 0);
                    currentVariables[j].Value = 1.0;

                    if (constants != null && constants.Count > 0)
                    {
                        currentVariables.AddRange(constants);
                    }

                    result[i, j] = leftEquationsParts[i].GetResultValue(currentVariables);
                }
            }

            return result;
        }

        /// <summary>
        /// Method is used to set variable collection with one value.
        /// </summary>
        /// <param name="variables">Variables to set value.</param>
        /// <param name="value">Value to set.</param>
        private static void SetVariablesWithValues(List<Variable> variables, double value)
        {
            foreach(Variable variable in variables)
            {
                variable.Value = value;
            }
        }

        #endregion
    }
}
