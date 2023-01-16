using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Services.ExternalServices;
using WebAPI.Utils;

namespace WebAPI
{
    public static class HttpContextExtensions
    {
        public static bool TryGetAuthToken(this HttpContext context, out string authToken)
        {
            var headers = context.Request.Headers;
            if (headers.ContainsKey("auth-token"))
            {
                authToken = headers["auth-token"];

                if (!Guid.TryParse(authToken, out _))
                {
                    if (!authToken.Contains(':'))
                    {
                        return false;
                    }

                    var tokenParts = authToken.Split(':');
                    authToken = Encoding.UTF8.GetString(
                        AesConverter.Deserialize(
                            Convert.FromBase64String(tokenParts[0]),
                            Convert.FromBase64String(tokenParts[1])
                        )
                    );
                    if (authToken.Length == 38)
                    {
                        authToken = authToken.Substring(1, 36);
                    }
                }

                return true;
            }

            authToken = "";

            // {"data":"","key":""}

            return false;
        }
    }
}