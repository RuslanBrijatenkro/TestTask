using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal static class DublicateCalculator
    {
        public static Dictionary<int, int> GetDublicatedIntegers(this List<int> integers)
        {
            Dictionary<int, int> dublicatedIntegers = integers
                .GroupBy(number => number)
                .Where(count => count.Count() > 1)
                .ToDictionary(number => number.Key, count => count.Count());

            return dublicatedIntegers;
        }
    }
}
