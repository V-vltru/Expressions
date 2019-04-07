using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Integral;

namespace WebApplication1.Controllers
{
    public class IntegralController : Controller
    {
        // GET: Integral
        [HttpPost]
        public string Calculate(IntegralInput input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (CalculationType type in input.Types)
            {
                Integral.Integral integral = new Integral.Integral(input.Integrand, input.StartValue, 
                    input.EndValue, input.IterationsNumber, input.ParameterName);

                double result = integral.Calculate(type);

                stringBuilder.AppendLine($"{type.ToString()}: {result}");
            }

            return stringBuilder.ToString();
        }
    }
}