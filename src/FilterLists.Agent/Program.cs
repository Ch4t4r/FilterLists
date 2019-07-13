﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using FilterLists.Agent.Features.Lists;
using FilterLists.Agent.Features.Urls;
using FilterLists.Agent.Infrastructure.ApplicationInsights;
using FilterLists.Agent.Infrastructure.DependencyInjection;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.Extensions.DependencyInjection;

namespace FilterLists.Agent
{
    public static class Program
    {
        private static IServiceProvider _serviceProvider;
        private static QuickPulseTelemetryModule _quickPulseTelemetryModule;

        public static async Task Main(string[] args)
        {
            Setup();

            var parser = _serviceProvider.GetService<Parser>();
            var mediator = _serviceProvider.GetService<IMediator>();
            await parser.ParseArguments<CommandLineOptions>(args).MapResult(async o =>
                {
                    if (o.ArchiveLists)
                        await mediator.Send(new ArchiveLists.Command());
                    if (o.ValidateUrls)
                        await mediator.Send(new ValidateAllUrls.Command());
                },
                e => Task.FromResult(0)
            );

            Teardown();
        }

        private static void Setup()
        {
            _serviceProvider = ServiceProviderBuilder.Build();

            var telemetryConfiguration = _serviceProvider.GetService<TelemetryConfiguration>();
            _quickPulseTelemetryModule = QuickPulseTelemetryModuleBuilder.Build(telemetryConfiguration);
        }

        private static void Teardown()
        {
            _quickPulseTelemetryModule.Dispose();

            var telemetryClient = _serviceProvider.GetService<TelemetryClient>();
            telemetryClient.Flush();
            Thread.Sleep(5000);
        }
    }
}