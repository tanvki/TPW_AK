using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etap0
{
    public class Calculator
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
        }

        public double add(double a, double b)
        {
            return a + b;
        }

        public double subtract(double a, double b)
        {
            return a - b;
        }

        public double multiply(double a, double b)
        {
            return a * b;
        }

        public double divide(double a, double b)
        {
            return a / b;
        }
    }
}
