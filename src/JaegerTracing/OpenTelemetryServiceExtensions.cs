﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JaegerTracing
{
    public static class OpenTelemetryServiceExtensions
    {
        public static IServiceCollection AddJaegerTracing(this IServiceCollection services, IConfiguration configuration)
        {
            var jaegerOptions = configuration.GetSection("Jaeger").Get<JaegerOptions>();
            
            var name = configuration.GetValue<string>("Jaeger:ServiceName");
            var host = configuration.GetValue<string>("Jaeger:Host");
            var port = configuration.GetValue<int>("Jaeger:Port");
            
            if (name is {Length: > 0} && host is {Length: > 0} && port > 0)
            {
                services.AddOpenTelemetryTracing(builder =>
                {
                    builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(name))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddJaegerExporter(options =>
                        {
                            options.AgentHost = host;
                            options.AgentPort = port;
                        });

                    if (jaegerOptions.Sources is {Length: > 0})
                    {
                        builder.AddSource(jaegerOptions.Sources);
                    }
                });
            }

            return services;
        }
    }
}
