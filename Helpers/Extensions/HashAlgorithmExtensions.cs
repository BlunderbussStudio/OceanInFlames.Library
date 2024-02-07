using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class HashAlgorithmExtensions
    {
        public static string ComputeHashString(this HashAlgorithm algo, byte[] buffer)
        {
            var hash = algo.ComputeHash(buffer);
            var text = Convert.ToHexString(hash);
            return text;
        }
    }
}
