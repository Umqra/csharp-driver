#if NETSTANDARD2_0
using System;
using App.Metrics;
using Cassandra.Metrics.DriverAbstractions;

namespace Cassandra.Metrics.AppMetricsImpl
{
    static class AppMetricsExtensions
    {
        public static Unit ToAppMetricsUnit(this DriverMeasurementUnit measurementUnit)
        {
            switch (measurementUnit)
            {
                case DriverMeasurementUnit.Bytes:
                    return Unit.Bytes;
                case DriverMeasurementUnit.Errors:
                    return Unit.Errors;
                case DriverMeasurementUnit.Requests:
                    return Unit.Requests;
                case DriverMeasurementUnit.Connections:
                    return Unit.Connections;
                case DriverMeasurementUnit.None:
                    return Unit.None;
                default:
                    throw new ArgumentOutOfRangeException(nameof(measurementUnit), measurementUnit, null);
            }
        }
    }
}
#endif