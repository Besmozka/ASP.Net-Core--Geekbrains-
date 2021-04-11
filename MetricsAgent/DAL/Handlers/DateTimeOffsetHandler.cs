using System;
using System.Data;
using Dapper;

namespace MetricsAgent.DAL.Handlers
{
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
            => DateTimeOffset.FromUnixTimeSeconds((int)value);

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            parameter.Value = value;
        }
    }
}
