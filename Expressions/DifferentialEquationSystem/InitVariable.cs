namespace DifferentialEquationSystem
{
    using Expressions.Models;

    public class DEVariable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DEVariable" /> class.
        /// </summary>
        /// <param name="name">Name of the init variable</param>
        /// <param name="value">Value of init variable</param>
        public DEVariable(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DEVariable" /> class.
        /// </summary>
        /// <param name="initVariable">Initial variable which is supposed to be copied to the current one</param>
        public DEVariable(DEVariable initVariable)
        {
            this.Name = initVariable.Name;
            this.Value = initVariable.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DEVariable" /> class.
        /// </summary>
        public DEVariable()
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
        /// <param name="initVariable">InitVariable instance</param>
        public static implicit operator Variable(DEVariable initVariable)
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
        public static implicit operator DEVariable(Variable variable)
        {
            if (variable != null)
            {
                DEVariable result = new DEVariable(variable.Name, variable.Value);

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
