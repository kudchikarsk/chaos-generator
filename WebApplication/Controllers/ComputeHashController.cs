using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class ComputeHashController : ApiController
    {
        // GET: api/ComputeHash
        public IHttpActionResult Get(string s = "", string encoding = "utf-8")
        {
            // Get data
            var data = Encoding.GetEncoding(encoding).GetBytes(s);
            var output = new StringWriter();
            // Show computed hashes
            ShowComputedHash<MD5CryptoServiceProvider>("MD5", data, output);
            ShowComputedHash<SHA1Managed>("SHA-1", data, output);
            ShowComputedHash<SHA256Managed>("SHA-256", data, output);
            ShowComputedHash<SHA384Managed>("SHA-384", data, output);
            ShowComputedHash<SHA512Managed>("SHA-512", data, output);
            ShowComputedHash<RIPEMD160Managed>("RIPEMD-160", data, output);

            return Ok(output.ToString());
        }

        private static void ShowComputedHash<T>(string algDisplayName, byte[] data, System.IO.TextWriter w) where T : HashAlgorithm, new()
        {
            using (var alg = new T())
            {
                var hash = alg.ComputeHash(data);
                w.WriteLine("{0}:", algDisplayName);
                foreach (ChaosHelper.KeyEncoding item in Enum.GetValues(typeof(ChaosHelper.KeyEncoding)))
                {
                    w.WriteLine("{0,-20}{1}", item, ChaosHelper.FormatByteArray(hash, item));
                }
                w.WriteLine();
            }
        }
    }
}
