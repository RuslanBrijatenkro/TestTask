using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class ListCreator : IRandom
    {
        public static ListCreator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ListCreator();
                return _instance;
            }
            set { }
        }

        public List<int> GetListOfIntegers(int length)
        {
            Random random = new Random();
            List<int> integersList = new List<int>(length);

            for (int i = 0; i < length; i++)
            {
                integersList.Add(random.Next(0, 9));
            }
            return integersList;
        }

        private static ListCreator _instance;
    }
}
