using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressions;
using Expressions.Models;

namespace LinearAlgebraicEquationsSystem
{
    public class LAEVariable
    {
        // <summary>
        /// Initializes a new instance of the <see cref="LAEVariable" /> class.
        /// </summary>
        /// <param name="name">Name of the init variable</param>
        /// <param name="value">Value of init variable</param>
        public LAEVariable(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LAEVariable" /> class.
        /// </summary>
        /// <param name="initVariable">Initial variable which is supposed to be copied to the current one</param>
        public LAEVariable(LAEVariable initVariable)
        {
            this.Name = initVariable.Name;
            this.Value = initVariable.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LAEVariable" /> class.
        /// </summary>
        public LAEVariable()
        {
        }

        /// <summary>
        /// Gets or sets the name of the variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the variable
        /// </summary>
        public double Value { get; set; }

        public static List<Variable> ConvertLAEVariablesToVariables(List<LAEVariable> lAEVariables)
        {
            List<Variable> result = new List<Variable>();

            foreach (LAEVariable lAEVariable in lAEVariables)
            {
                result.Add(new Variable(lAEVariable.Name, lAEVariable.Value));
            }

            return result;
        }

        public static List<LAEVariable> ConvertVariablesToLAEVariables(List<Variable> variables)
        {
            List<LAEVariable> result = new List<LAEVariable>();

            foreach (Variable variable in variables)
            {
                result.Add(new LAEVariable(variable.Name, variable.Value));
            }

            return result;
        }

        /// <summary>
        /// Method implicitly converts InitVariable instance to the Variable one
        /// </summary>
        /// <param name="initVariable">InitVariable instance</param>
        public static implicit operator Variable(LAEVariable initVariable)
        {
            if (initVariable != null)
            {
                Variable result = new Variable(initVariable.Name, initVariable.Value);

                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method implicitly converts Variable instance to the InitVariable one
        /// </summary>
        /// <param name="variable">Variable instance</param>
        public static implicit operator LAEVariable(Variable variable)
        {
            if (variable != null)
            {
                LAEVariable result = new LAEVariable(variable.Name, variable.Value);

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
