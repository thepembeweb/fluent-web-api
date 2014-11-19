using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace FluentWebApi.Controllers
{
    internal sealed class RequestBackedHttpRequestContext : HttpRequestContext
    {
        private HttpRequestMessage _request;

        private X509Certificate2 _certificate;
        private bool _certificateSet;

        private HttpConfiguration _configuration;
        private bool _configurationSet;

        private bool _includeErrorDetail;
        private bool _includeErrorDetailSet;

        private bool _isLocal;
        private bool _isLocalSet;

        private IHttpRouteData _routeData;
        private bool _routeDataSet;

        private UrlHelper _url;
        private bool _urlSet;

        private string _virtualPathRoot;
        private bool _virtualPathRootSet;

        public RequestBackedHttpRequestContext()
        {
            // We don't have to override Principal since the base class provides the simple property.
            Principal = Thread.CurrentPrincipal;
        }

        public RequestBackedHttpRequestContext(HttpRequestMessage request)
            : this()
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            _request = request;
        }

        public HttpRequestMessage Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
            }
        }

        public override X509Certificate2 ClientCertificate
        {
            get
            {
                if (_certificateSet)
                {
                    return _certificate;
                }
                else if (_request != null)
                {
                    return _request.GetClientCertificate();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _certificate = value;
                _certificateSet = true;
            }
        }

        public override HttpConfiguration Configuration
        {
            get
            {
                if (_configurationSet)
                {
                    return _configuration;
                }
                else if (_request != null)
                {
                    return _request.GetConfiguration();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _configuration = value;
                _configurationSet = true;
            }
        }

        public override bool IncludeErrorDetail
        {
            get
            {
                if (_includeErrorDetailSet)
                {
                    return _includeErrorDetail;
                }
                else if (_request != null)
                {
                    return _request.ShouldIncludeErrorDetail();
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _includeErrorDetail = value;
                _includeErrorDetailSet = true;
            }
        }

        public override bool IsLocal
        {
            get
            {
                if (_isLocalSet)
                {
                    return _isLocal;
                }
                else if (_request != null)
                {
                    return _request.IsLocal();
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _isLocal = value;
                _isLocalSet = true;
            }
        }

        public override IHttpRouteData RouteData
        {
            get
            {
                if (_routeDataSet)
                {
                    return _routeData;
                }
                else if (_request != null)
                {
                    return _request.GetRouteData();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _routeData = value;
                _routeDataSet = true;
            }
        }

        public override UrlHelper Url
        {
            get
            {
                if (_urlSet)
                {
                    return _url;
                }
                else if (_request != null)
                {
                    return new UrlHelper(_request);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _url = value;
                _urlSet = true;
            }
        }

        public override string VirtualPathRoot
        {
            get
            {
                if (_virtualPathRootSet)
                {
                    return _virtualPathRoot;
                }

                HttpConfiguration configuration = Configuration;

                if (configuration != null)
                {
                    return configuration.VirtualPathRoot;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _virtualPathRoot = value;
                _virtualPathRootSet = true;
            }
        }
    }
}