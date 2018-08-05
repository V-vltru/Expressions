namespace ExpressionUnitTesting
{
    using System.Collections.Generic;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Expressions;
    using Expressions.Models;
    using System.Xml.Linq;
    using System;
    using System.Globalization;

    [TestClass]
    public class ExpressionIntegrationTesting
    {
        [TestMethod]
        public void IntegrationTest1()
        {
            string expression = "(a + b) * c";
            List<Variable> vars = new List<Variable>
            {
                new Variable("a", 1),
                new Variable("b", 3),
                new Variable("c", 2)
            };

            double result = 8;

            Expression e = new Expression(expression, vars);

            Assert.AreEqual(result, e.GetResultValue(vars));
        }

        [TestMethod]
        [Ignore]
        [DeploymentItem("res\\ExpressionCases.xml", "res")]
        public void IntegrationTest()
        {
            string fileName = "res\\ExpressionCases.xml";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNodeList testsList = xmlDocument.SelectNodes("Expressions/Expression");

            foreach(XmlNode test in testsList)
            {
                string id = test.Attributes["id"].Value;

                List<Variable> vars = new List<Variable>();
                XmlNodeList varsList = test.SelectNodes("Parameters/Parameter");

                foreach (XmlNode var in varsList)
                {
                    //double.TryParse(var.Attributes["Value"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double value);
                    vars.Add(new Variable(var.Attributes["Name"].Value,
                        Convert.ToDouble(var.Attributes["Value"].Value, CultureInfo.CurrentCulture.NumberFormat)));
                }

                string expression = test.SelectSingleNode("Value").InnerText;
                double result = Convert.ToDouble(test.SelectSingleNode("Result").InnerText, CultureInfo.CurrentCulture.NumberFormat);
                //double.TryParse(test.SelectSingleNode("Result").InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out double result);

                Expression exp = new Expression(expression, vars);

                Assert.AreEqual(result, exp.GetResultValue(vars), 0.01, string.Format("Iteration: {0}", id));
            }
        }
    }
}
