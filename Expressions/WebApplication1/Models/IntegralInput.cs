using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integral;

namespace WebApplication1.Models
{
    public class IntegralInput
    {
        public string Integrand { get; set; }

        public double StartValue { get; set; }

        public double EndValue { get; set; }

        public int IterationsNumber { get; set; }

        public string ParameterName { get; set; }

        public List<Integral.CalculationType> Types { get; set; }
    }
}