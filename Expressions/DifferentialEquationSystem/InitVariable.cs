namespace DifferentialEquationSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Expressions.Models;

    public class InitVariable
    {
        /// <summary>
        /// Gets or sets the name of the variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the variable
        /// </summary>
        public double Value { get; set; }

        public InitVariable(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }

        public static implicit operator Variable(InitVariable init)
        {
            Variable result = new Variable(init.Name, init.Value);

            return result;
        }
    }
}
