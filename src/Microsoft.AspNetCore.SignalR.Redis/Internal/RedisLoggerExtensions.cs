// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Microsoft.AspNetCore.SignalR.Redis.Internal
{
    internal static class RedisLoggerExtensions
    {
        // Category: RedisHubLifetimeManager<THub>
        private static readonly Action<ILogger, string, Exception> _connectingToEndpoints =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "ConnectingToEndpoints"), "Connecting to Redis endpoints: {Endpoints}.");

        private static readonly Action<ILogger, Exception> _connected =
            LoggerMessage.Define(LogLevel.Information, new EventId(2, "Connected"), "Connected to Redis.");

        private static readonly Action<ILogger, string, Exception> _subscribing =
            LoggerMessage.Define<string>(LogLevel.Trace, new EventId(3, "Subscribing"), "Subscribing to channel: {Channel}.");

        private static readonly Action<ILogger, string, Exception> _receivedFromChannel =
            LoggerMessage.Define<string>(LogLevel.Trace, new EventId(4, "ReceivedFromChannel"), "Received message from Redis channel {Channel}.");

        private static readonly Action<ILogger, string, Exception> _publishToChannel =
            LoggerMessage.Define<string>(LogLevel.Trace, new EventId(5, "PublishToChannel"), "Publishing message to Redis channel {Channel}.");

        private static readonly Action<ILogger, string, Exception> _unsubscribe =
            LoggerMessage.Define<string>(LogLevel.Trace, new EventId(6, "Unsubscribe"), "Unsubscribing from channel: {Channel}.");

        private static readonly Action<ILogger, Exception> _notConnected =
            LoggerMessage.Define(LogLevel.Warning, new EventId(7, "Connected"), "Not connected to Redis.");

        private static readonly Action<ILogger, Exception> _connectionRestored =
            LoggerMessage.Define(LogLevel.Information, new EventId(8, "ConnectionRestored"), "Connection to Redis restored.");

        private static readonly Action<ILogger, Exception> _connectionFailed =
            LoggerMessage.Define(LogLevel.Warning, new EventId(9, "ConnectionFailed"), "Connection to Redis failed.");

        private static readonly Action<ILogger, Exception> _failedWritingMessage =
            LoggerMessage.Define(LogLevel.Warning, new EventId(10, "FailedWritingMessage"), "Failed writing message.");

        private static readonly Action<ILogger, Exception> _internalMessageFailed =
            LoggerMessage.Define(LogLevel.Warning, new EventId(11, "InternalMessageFailed"), "Error processing message for internal server message.");

        public static void ConnectingToEndpoints(this ILogger logger, EndPointCollection endpoints)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                if (endpoints.Count > 0)
                {
                    _connectingToEndpoints(logger, string.Join(", ", endpoints.Select(e => EndPointCollection.ToString(e))), null);
                }
            }
        }

        public static void Connected(this ILogger logger)
        {
            _connected(logger, null);
        }

        public static void Subscribing(this ILogger logger, string channelName)
        {
            _subscribing(logger, channelName, null);
        }

        public static void ReceivedFromChannel(this ILogger logger, string channelName)
        {
            _receivedFromChannel(logger, channelName, null);
        }

        public static void PublishToChannel(this ILogger logger, string channelName)
        {
            _publishToChannel(logger, channelName, null);
        }

        public static void Unsubscribe(this ILogger logger, string channelName)
        {
            _unsubscribe(logger, channelName, null);
        }

        public static void NotConnected(this ILogger logger)
        {
            _notConnected(logger, null);
        }

        public static void ConnectionRestored(this ILogger logger)
        {
            _connectionRestored(logger, null);
        }

        public static void ConnectionFailed(this ILogger logger, Exception exception)
        {
            _connectionFailed(logger, exception);
        }

        public static void FailedWritingMessage(this ILogger logger, Exception exception)
        {
            _failedWritingMessage(logger, exception);
        }

        public static void InternalMessageFailed(this ILogger logger, Exception exception)
        {
            _internalMessageFailed(logger, exception);
        }
    }
}