using System.Diagnostics;
using System.Net;
using Yarp.ReverseProxy.Forwarder;

namespace Dotnet.YarpSample.Configs;

internal static class ForwarderConfig
{
    public static HttpMessageInvoker DefaultInvoker
        => new HttpMessageInvoker(new SocketsHttpHandler(){
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false,
            ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current)
        });

    public static ForwarderRequestConfig DefaultForwarderRequest
        => new ForwarderRequestConfig{ ActivityTimeout = TimeSpan.FromSeconds(100) };
}