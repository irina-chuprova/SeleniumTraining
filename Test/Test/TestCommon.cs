using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class TestCommon
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        public static string GenerateRandomString(int length)
        {
            string result = null;
            List<string> characters = new List<string>();
            for (int i = 97; i < 123; i++) // a-z
            {
                characters.Add(((char)i).ToString());
            }

            for (int i = 0; i < length; i++)
            {
                result += characters[random.Next(0, characters.Count)];
            }
            return result;
        }
    }
}
