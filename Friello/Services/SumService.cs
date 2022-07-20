using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Services
{
    public class SumService : ISum
    {
        public int Sum(int num1, int num2)
        {
            return num1+num2;
        }
    }
}
