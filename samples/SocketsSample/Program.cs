// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Internal.Protocol;
using Microsoft.AspNetCore.Sockets;
using Microsoft.Extensions.Logging.Abstractions;

namespace SocketsSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hubLifetimeManager = new DefaultHubLifetimeManager<Hub>(NullLogger<DefaultHubLifetimeManager<Hub>>.Instance);

            var protocol = new JsonHubProtocol();

            for (var i = 0; i < 1000; ++i)
            {
                var options = new PipeOptions();
                var pair = DuplexPipe.CreateConnectionPair(options, options);
                var connection = new DefaultConnectionContext(Guid.NewGuid().ToString(), pair.Application, pair.Transport);
                var hubConnection = new HubConnectionContext(connection, Timeout.InfiniteTimeSpan, NullLoggerFactory.Instance)
                {
                    Protocol = protocol
                };

                await hubLifetimeManager.OnConnectedAsync(hubConnection);

                _ = ConsumeAsync(connection.Application);
            }

            var hubContext = new HubContext<Hub>(hubLifetimeManager);

            Console.WriteLine("1000 connections waiting to send 1000 messages to");
            Console.ReadLine();

            for (int i = 0; i < 1000; i++)
            {
                await hubContext.Clients.All.SendAsync("Send", "Hello");
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static async Task ConsumeAsync(IDuplexPipe application)
        {
            while (true)
            {
                var result = await application.Input.ReadAsync();
                var buffer = result.Buffer;

                if (!buffer.IsEmpty)
                {
                    application.Input.AdvanceTo(buffer.End);
                }
                else if (result.IsCompleted)
                {
                    break;
                }
            }

            application.Input.Complete();
        }
    }
}
