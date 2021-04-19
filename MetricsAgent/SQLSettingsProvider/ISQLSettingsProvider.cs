using System;

namespace MetricsAgent.SQLSettingsProvider
{
    public interface ISqlSettingsProvider
    {
        String GetConnectionString();
    }
}
