using System;
using System.Collections.Generic;

namespace MetricsAgent.DAL
{
    public interface IRepository<T> where T : class
    {
        List<T> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime);

        void Create(T item);
        
    }

}
