using System;
using System.Data;
using Dapper;

namespace MetricsManager.DAL.Handlers
{
    public class UriHandler : SqlMapper.TypeHandler<Uri>
    {
        public override Uri Parse(object value)
            => new Uri(value.ToString());

        public override void SetValue(IDbDataParameter parameter, Uri value)
        {
            parameter.Value = value;
        }
    }
}
