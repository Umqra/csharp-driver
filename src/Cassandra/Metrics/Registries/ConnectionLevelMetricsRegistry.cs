using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra.Metrics.DriverAbstractions;
using Cassandra.Metrics.StubImpl;
using Cassandra.Requests;

namespace Cassandra.Metrics.Registries
{
    internal class ConnectionLevelMetricsRegistry : IConnectionLevelMetricsRegistry
    {
        public static readonly IConnectionLevelMetricsRegistry
            EmptyInstance = new ConnectionLevelMetricsRegistry(EmptyDriverMetricsProvider.Instance, EmptyDriverMetricsProvider.Instance);

        public ConnectionLevelMetricsRegistry(IDriverMetricsProvider hostMetricsProvider, IDriverMetricsProvider sessionMetricsProvider)
        {
            BytesSentForHost = hostMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceivedForHost = hostMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);
            BytesSentForSession = sessionMetricsProvider.Counter("bytes-sent", DriverMeasurementUnit.Bytes);
            BytesReceivedForSession = sessionMetricsProvider.Counter("bytes-received", DriverMeasurementUnit.Bytes);

            ConnectionInitErrors = hostMetricsProvider.WithContext("errors")
                                                      .WithContext("connection")
                                                      .Counter("init", DriverMeasurementUnit.Errors);
            AuthenticationErrors = hostMetricsProvider.WithContext("errors")
                                                      .WithContext("connection")
                                                      .Counter("auth", DriverMeasurementUnit.Errors);

            _messageTotalLatency = hostMetricsProvider.Timer("messages", DriverMeasurementUnit.Requests);
            _cqlMessageTotalLatency = hostMetricsProvider.Timer("cql-messages", DriverMeasurementUnit.Requests);
            _messagesLatencyByRequestType = new Dictionary<DriverRequestType, IDriverTimer>();
            foreach (var requestType in Enum.GetValues(typeof(DriverRequestType)).Cast<DriverRequestType>())
            {
                _messagesLatencyByRequestType[requestType] = hostMetricsProvider.WithContext("types")
                                                                                .Timer(requestType.ToString(), DriverMeasurementUnit.Requests);
            }
        }

        public IDriverTimeHandler RecordRequestLatency(IRequest request)
        {
            return request.RequestType.IsCqlRequest()
                ? new CompositeTimeHandler(
                    _messageTotalLatency.StartRecording(),
                    _cqlMessageTotalLatency.StartRecording(),
                    _messagesLatencyByRequestType[request.RequestType].StartRecording()
                )
                : new CompositeTimeHandler(
                    _messageTotalLatency.StartRecording(),
                    _messagesLatencyByRequestType[request.RequestType].StartRecording()
                );
        }

        private IDriverCounter BytesSentForHost { get; }
        private IDriverCounter BytesReceivedForHost { get; }
        private IDriverCounter BytesSentForSession { get; }
        private IDriverCounter BytesReceivedForSession { get; }
        private readonly Dictionary<DriverRequestType, IDriverTimer> _messagesLatencyByRequestType;
        private readonly IDriverTimer _messageTotalLatency;
        private readonly IDriverTimer _cqlMessageTotalLatency;


        public void RecordBytesSent(long bytes)
        {
            BytesSentForHost.Increment(bytes);
            BytesSentForSession.Increment(bytes);
        }

        public void RecordBytesReceived(long bytes)
        {
            BytesReceivedForHost.Increment(bytes);
            BytesReceivedForSession.Increment(bytes);
        }

        public IDriverCounter ConnectionInitErrors { get; }
        public IDriverCounter AuthenticationErrors { get; }
    }
}