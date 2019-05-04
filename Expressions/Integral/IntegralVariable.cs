namespace Integral
{
    using Expressions.Models;

    public class IntegralVariable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegralVariable" /> class.
        /// </summary>
        /// <param name="name">Name of the init variable</param>
        /// <param name="value">Value of init variable</param>
        public IntegralVariable(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegralVariable" /> class.
        /// </summary>
        /// <param name="integralVariable">Initial variable which is supposed to be copied to the current one</param>
        public IntegralVariable(IntegralVariable integralVariable)
        {
            this.Name = integralVariable.Name;
            this.Value = integralVariable.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegralVariable" /> class.
        /// </summary>
        public IntegralVariable()
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
        /// Method implicitly converts InitVariable instance to the Variable one
        /// </summary>
        /// <param name="integralVariable">InitVariable instance</param>
        public static implicit operator Variable(IntegralVariable integralVariable)
        {
            if (integralVariable != null)
            {
                Variable result = new Variable(integralVariable.Name, integralVariable.Value);

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
        public static implicit operator IntegralVariable(Variable variable)
        {
            if (variable != null)
            {
                IntegralVariable result = new IntegralVariable(variable.Name, variable.Value);

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
