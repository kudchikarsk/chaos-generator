using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class PasswordsController : ApiController
    {
        // GET: api/Passwords
        public IHttpActionResult Get(int count = 20, int length = 12, string chars = "ulns")
        {
            if (count < 1 || count > 50) count = 20;
            if (length < 5 || length > 20) length = 12;

            ChaosHelper.PasswordChars allowedChars = 0;
            if (chars.IndexOf('u') > -1) allowedChars |= ChaosHelper.PasswordChars.UpperCaseLetters;
            if (chars.IndexOf('l') > -1) allowedChars |= ChaosHelper.PasswordChars.LowerCaseLetters;
            if (chars.IndexOf('n') > -1) allowedChars |= ChaosHelper.PasswordChars.Numbers;
            if (chars.IndexOf('s') > -1) allowedChars |= ChaosHelper.PasswordChars.Special;
            if (allowedChars == 0) allowedChars = ChaosHelper.PasswordChars.All;

            var output = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                output.AppendLine(ChaosHelper.GeneratePassword(length, allowedChars));
            }

            return Ok(output.ToString());
        }
    }
}
