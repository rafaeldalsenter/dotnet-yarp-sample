using Dotnet.YarpSample.Configs;
using Dotnet.YarpSample.Extensions;
using Yarp.ReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpForwarder()
    .AddHealthChecks();

var app = builder.Build();

app.MapGet("/", async (HttpContext context, IHttpForwarder forwarder, ILoggerFactory loggerFactory) => {

    var logger = loggerFactory.CreateLogger("Start");

    var urlForwarder = builder.Configuration.GetSection("UrlForwarder").Get<string>();

    if(string.IsNullOrWhiteSpace(urlForwarder))
    {
        logger.LogError("Url config not found");
        context.SetInternalServerError();
        return;
    }
    
    var result = await forwarder.SendAsync(context, urlForwarder, ForwarderConfig.DefaultInvoker, ForwarderConfig.DefaultForwarderRequest,
        (context, proxyRequest) => {
            proxyRequest.RequestUri = new Uri(urlForwarder);
            return default;
        });

    if(result != ForwarderError.None)
    {
        logger.LogError($"Error on forwarding: {context.GetForwarderErrorFeature()?.Exception}");
        context.SetInternalServerError();
    }

});


app.MapHealthChecks("/healthcheck");
app.Run();
