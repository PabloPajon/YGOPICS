using System.Security.Cryptography;
using System.Text;

namespace YGOPICS.Helpers
{
    public class CypherHelper
    {
        public static String CifrateHash(string pass)
        {
            UnicodeEncoding converter = new UnicodeEncoding();
            byte[] input = converter.GetBytes(pass);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(input);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }
    }
}
