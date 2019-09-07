using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class MachineKeyController : ApiController
    {
        private static string XML_CODE = "<machineKey validation=\"{0}\" decryption=\"{1}\" validationKey=\"{2}\" decryptionKey=\"{3}\" />";

        // GET: api/MachineKey
        public IHttpActionResult Get(string validation = "HMACSHA256", string decryption = "AES256")
        {
            int validationKeyLength, decryptionKeyLength;

            switch (validation)
            {
                case "MD5":
                    validationKeyLength = 128;
                    break;
                case "SHA1":
                    validationKeyLength = 160;
                    break;
                case "3DES":
                    validationKeyLength = 192;
                    break;
                case "AES":
                case "HMACSHA256":
                    validationKeyLength = 256;
                    break;
                case "HMACSHA384":
                    validationKeyLength = 384;
                    break;
                case "HMACSHA512":
                    validationKeyLength = 512;
                    break;
                default:
                    return BadRequest("Invalid parameter 'validation'");
            }

            switch (decryption)
            {
                case "DES":
                    decryptionKeyLength = 64;
                    break;
                case "3DES":
                    decryptionKeyLength = 192;
                    break;
                case "AES128":
                    decryption = "AES";
                    decryptionKeyLength = 128;
                    break;
                case "AES192":
                    decryption = "AES";
                    decryptionKeyLength = 192;
                    break;
                case "AES256":
                    decryption = "AES";
                    decryptionKeyLength = 256;
                    break;
                default:
                    return BadRequest("Invalid parameter 'decryption'");
            }

            var output = string.Format(XML_CODE,
                validation,
                decryption,
                ChaosHelper.GenerateKey(validationKeyLength / 8, ChaosHelper.KeyEncoding.UpperHex),
                ChaosHelper.GenerateKey(decryptionKeyLength / 8, ChaosHelper.KeyEncoding.UpperHex));

            return Ok(output);
        }
    }
}
