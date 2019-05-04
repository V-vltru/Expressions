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
        }

        public LinearAlgebraicEquationSystem()
        {
        }

        public List<Expression> LeftPartEquations { get; set; }

        public List<double> RightPartEquations { get; set; }

        public List<Variable> Variables { get; set; }

        public List<Variable> Constants { get; set; }
    }
}
