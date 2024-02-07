using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string AbbreviateByteCount(long bytes)
        {
            if (bytes < 900) return string.Format($"{bytes}B");
            var kib = bytes / 1024d;
            if (kib < 900) return AbbrievateByteCount(kib, "KiB");
            var mib = kib / 1024d;
            if (mib < 900) return AbbrievateByteCount(mib, "MiB");
            var gib = mib / 1024d;
            if (gib < 900) return AbbrievateByteCount(gib, "GiB");
            var tib = gib / 1024d;
            return AbbrievateByteCount(tib, "TiB");
        }

        static string AbbrievateByteCount(double amount, string units)
        {
            if (amount < 1) return $"{amount:0.00} {units}";
            if (amount < 10) return $"{amount:0.0} {units}";
            return $"{amount:0} {units}";
        }

        public static string ConvertToHexString(string input)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(input);

            string hexedOutput = BitConverter.ToString(inputByteArray).Replace("-", string.Empty);

            return hexedOutput;
        }

        public static bool IsValidEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|io)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
    }
}
