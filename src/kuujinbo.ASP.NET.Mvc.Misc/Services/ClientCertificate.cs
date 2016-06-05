﻿using System;
using System.Web;

namespace kuujinbo.ASP.NET.Mvc.Misc.Services
{
    /// <summary>
    /// wrapper for testing
    /// </summary>
    public interface IClientCertificate
    {
        byte[] Get(HttpRequestBase request);
    }

    public class ClientCertificate : IClientCertificate
    {
        // BIG-IP does **NOT** allow direct access to HttpRequestBase.ClientCertificate
        public const string CERT_HEADER = "ssl.client_cert";

        public virtual byte[] Get(HttpRequestBase request)
        {
            return request.IsLocal 
                ? request.ClientCertificate.Certificate
                : Convert.FromBase64String(request.Headers[CERT_HEADER]);
        }
    }
}