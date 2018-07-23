namespace DifferentialEquationSystem
{
    using Expressions.Models;

    public class InitVariable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitVariable" /> class.
        /// </summary>
        /// <param name="name">Name of the init variable</param>
        /// <param name="value">Value of init variable</param>
        public InitVariable(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitVariable" /> class.
        /// </summary>
        /// <param name="initVariable">Initial variable which is supposed to be copied to the current one</param>
        public InitVariable(InitVariable initVariable)
        {
            this.Name = initVariable.Name;
            this.Value = initVariable.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitVariable" /> class.
        /// </summary>
        public InitVariable()
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
        public static implicit operator Variable(InitVariable initVariable)
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
        public static implicit operator InitVariable(Variable variable)
        {
            if (variable != null)
            {
                InitVariable result = new InitVariable(variable.Name, variable.Value);

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
