/// <summary>
/// Namespace contains all models which are operated by <see cref="Expressions.Expression" /> class.
/// </summary>
namespace Expressions.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Variable included in the Expression
    /// Has two parameters: Variable name and value
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Variable" /> class.
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="value">Value of the variable</param>
        public Variable(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable" /> class.
        /// </summary>
        /// <param name="variable">The instance of <see cref="Variable" /> class which is supposed to be copied to the current one</param>
        public Variable(Variable variable)
        {
            this.Name = variable.Name;
            this.Value = variable.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Variable" /> class.
        /// Default constructor
        /// </summary>
        public Variable()
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

        /// <summary>
        /// Method defines what the parameter belongs to: Variable, Number or Nothing
        /// </summary>
        /// <param name="parameter">The parameter for definition</param>
        /// <param name="vars">List of variables to define whether the input parameter is a variable</param>
        /// <returns>The 'EssenceType' variable: Variable, Number or Nothing respectively</returns>
        public static EssenceType IsNumberOrVariable(string parameter, List<Variable> vars)
        {
            Variable foundVariable = (from g in vars
                                      where g.Name == parameter
                                      select g).FirstOrDefault();
            if (foundVariable != null)
            {
                return EssenceType.Variable;
            }

            if (double.TryParse(parameter, out double a))
            {
                return EssenceType.Number;
            }
            else
            {
                return EssenceType.Nothing;
            }
        }

        /// <summary>
        /// Gets value of variable by its name from the list
        /// </summary>
        /// <param name="varName">Name of variable</param>
        /// <param name="variables">List of variables to get from</param>
        /// <returns>The value of the variable in double format</returns>
        public static double GetVariableValue(string varName, List<Variable> variables)
        {
            double result;
            int count = (from g in variables
                         where g.Name == varName
                         select g.Value).Count();

            if (count == 0)
            {
                throw new Exception(string.Format("Variable {0} wasn't found in the list of variables", varName));
            }
            else if (count > 1)
            {
                throw new Exception(string.Format("There are several variables with this name: {0}", varName));
            }
            else
            {
                result = (from g in variables
                          where g.Name == varName
                          select g.Value).First();
            }

            return result;
        }
    }
}
