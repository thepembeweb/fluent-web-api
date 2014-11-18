using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using FluentWebApi.Configuration;
using ExceptionCatchBlocks = System.Web.Http.ExceptionHandling.ExceptionCatchBlocks;
using ExceptionContext = System.Web.Http.ExceptionHandling.ExceptionContext;
using IExceptionHandler = System.Web.Http.ExceptionHandling.IExceptionHandler;
using IExceptionLogger = System.Web.Http.ExceptionHandling.IExceptionLogger;
using FluentWebApi.Controllers;

namespace FluentWebApi.Services
{
    internal class FluentApiHttpHandler : HttpControllerDispatcher
    {
        private IExceptionLogger _exceptionLogger;
        private IExceptionHandler _exceptionHandler;

        public FluentApiHttpHandler(HttpConfiguration configuration)
            : base(configuration)
        {
            
        }

        internal IExceptionLogger ExceptionLogger
        {
            get
            {
                if (_exceptionLogger == null)
                {
                    _exceptionLogger = ExceptionServices.GetLogger(Configuration);
                }

                return _exceptionLogger;
            }
        }

        /// <remarks>This property is internal and settable only for unit testing purposes.</remarks>
        internal IExceptionHandler ExceptionHandler
        {
            get
            {
                if (_exceptionHandler == null)
                {
                    _exceptionHandler = ExceptionServices.GetHandler(Configuration);
                }

                return _exceptionHandler;
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            IHttpRouteData routeData = request.GetRouteData();
            if (routeData != null && routeData.Route.DataTokens != null)
            {
                object isEnabled;
                if (routeData.Route.DataTokens.TryGetValue(Route.FluentApiEnabled, out isEnabled) && (bool)isEnabled)
                {
                    var controllerType = routeData.Route.DataTokens[Route.ControllerType] as Type;
                    var controllerName = routeData.Route.DataTokens[Route.ControllerName] as string;

                    if (controllerType != null && controllerName != null)
                    {
                        //Found a dynamic api controller type!
                        var controllerDescriptor = new HttpControllerDescriptor(Configuration, controllerName, controllerType);
                        return SendFluentApiAsync(request, cancellationToken, controllerDescriptor);
                    }
                }
            }

            // Fallback to the normal procedure
            return base.SendAsync(request, cancellationToken);
        }

        internal async Task<HttpResponseMessage> SendFluentApiAsync(HttpRequestMessage request, CancellationToken cancellationToken, HttpControllerDescriptor controllerDescriptor)
        {
            ExceptionDispatchInfo exceptionInfo;
            HttpControllerContext controllerContext = null;

            try
            {
                IHttpController controller = controllerDescriptor.CreateController(request);
                if (controller == null)
                {
                    var httpError = new HttpError(string.Format(CultureInfo.CurrentCulture, "No HTTP resource was found that matches the request URI '{0}'.", request.RequestUri));
                    if (request.ShouldIncludeErrorDetail())
                    {
                        httpError.Add(HttpErrorKeys.MessageDetailKey, "No controller was created to handle this request.");
                    }

                    return request.CreateErrorResponse(HttpStatusCode.NotFound, httpError);
                }

                controllerContext = CreateControllerContext(request, controllerDescriptor, controller);
                return await controller.ExecuteAsync(controllerContext, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Propogate the canceled task without calling exception loggers or handlers.
                throw;
            }
            catch (HttpResponseException httpResponseException)
            {
                return httpResponseException.Response;
            }
            catch (Exception exception)
            {
                exceptionInfo = ExceptionDispatchInfo.Capture(exception);
            }

            Debug.Assert(exceptionInfo.SourceException != null);

            ExceptionContext exceptionContext = new ExceptionContext(
                exceptionInfo.SourceException,
                ExceptionCatchBlocks.HttpControllerDispatcher,
                request)
            {
                ControllerContext = controllerContext,
            };

            await ExceptionLogger.LogAsync(exceptionContext, cancellationToken);
            HttpResponseMessage response = await ExceptionHandler.HandleAsync(exceptionContext, cancellationToken);

            if (response == null)
            {
                exceptionInfo.Throw();
            }

            return response;
        }

        private static HttpControllerContext CreateControllerContext(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            IHttpController controller)
        {
            Contract.Assert(request != null);
            Contract.Assert(controllerDescriptor != null);
            Contract.Assert(controller != null);

            HttpConfiguration controllerConfiguration = controllerDescriptor.Configuration;

            // Set the controller configuration on the request properties
            HttpConfiguration requestConfig = request.GetConfiguration();
            if (requestConfig == null)
            {
                request.SetConfiguration(controllerConfiguration);
            }
            else
            {
                if (requestConfig != controllerConfiguration)
                {
                    request.SetConfiguration(controllerConfiguration);
                }
            }

            HttpRequestContext requestContext = request.GetRequestContext();

            // if the host doesn't create the context we will fallback to creating it.
            if (requestContext == null)
            {
                requestContext = new RequestBackedHttpRequestContext(request)
                {
                    // we are caching controller configuration to support per controller configuration.
                    Configuration = controllerConfiguration,
                };

                // if the host did not set a request context we will also set it back to the request.
                request.SetRequestContext(requestContext);
            }

            return new HttpControllerContext(requestContext, request, controllerDescriptor, controller);
        }
    }
}