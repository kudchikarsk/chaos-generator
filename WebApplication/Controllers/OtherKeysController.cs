using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class OtherKeysController : ApiController
    {
        // GET: api/OtherKeys
        public IHttpActionResult Get(int count = 20, int length = 64, string encoding = "Hex")
        {
            var encodingFlag = (ChaosHelper.KeyEncoding)Enum.Parse(typeof(ChaosHelper.KeyEncoding), encoding);

            if (count < 1 || count > 50) count = 20;
            if (length < 16 || length > 8192) length = 64;

            var output = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                output.AppendLine(ChaosHelper.GenerateKey(length, encodingFlag));
            }

            return Ok(output.ToString());
        }
    }
}
