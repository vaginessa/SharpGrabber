﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using DotNetTools.SharpGrabber.Grabbed;
using DotNetTools.SharpGrabber.Internal;

namespace DotNetTools.SharpGrabber
{
    public class GrabberServices : IGrabberServices
    {
        public static readonly GrabberServices Default = new();

        private readonly Func<HttpClient> _httpClientProvider;

        public GrabberServices(Func<HttpClient> httpClientProvider = null, IMimeService mime = null)
        {
            _httpClientProvider = httpClientProvider ?? GetGlobalHttpClient;
            Mime = mime ?? DefaultMimeService.Instance;
        }
        public IMimeService Mime { get; }

        public HttpClient GetClient()
            => _httpClientProvider();

        private static HttpClient _globalHttpClient;

        private static HttpClient GetGlobalHttpClient()
        {
            if (_globalHttpClient == null)
                lock (typeof(GrabberBase))
                {
                    if (_globalHttpClient == null)
                    {
                        var defaultProvider = new DefaultGlobalHttpProvider();
                        _globalHttpClient = defaultProvider.GetClient();
                    }
                }
            return _globalHttpClient;
        }
    }
}
