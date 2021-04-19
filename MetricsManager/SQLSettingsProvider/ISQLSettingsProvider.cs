using System;

namespace MetricsManager.SQLSettingsProvider
{
    public interface ISqlSettingsProvider
    {
        String GetConnectionString();
    }
}
