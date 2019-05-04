using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DifferentialEquationSystem;

namespace WebApplication1.Models
{
    public class DEInput
    {
        public List<DEVariable> Constants { get; set; }

        public DEVariable TimeVariable { get; set; }

        public double TEnd { get; set; }

        public double Tau { get; set; }

        public List<string> Expressions { get; set; }

        public List<DEVariable> LeftVariables { get; set; }
    }
}