namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Expressions;
    using Expressions.Models;

    public partial class DifferentialEquationSystem
    {
        public List<Variable> EulerCalculation(List<List<Variable>> variablesAtAllStep = null)
        {
            List<Variable> currentLeftVariables = new List<Variable>();
            List<Variable> nextLeftVariables = new List<Variable>();

            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, currentLeftVariables);
            DifferentialEquationSystemHelpers.CopyVariables(this.LeftVariables, nextLeftVariables);

            if (variablesAtAllStep != null)
            {

            }

            return currentLeftVariables;
        }
    }
}
