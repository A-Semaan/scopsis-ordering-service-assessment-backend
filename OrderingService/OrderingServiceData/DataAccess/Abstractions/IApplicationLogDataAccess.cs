using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess.Abstractions
{
    public interface IApplicationLogDataAccess
    {
        public long InsertLog(ApplicationLog applicationLog);

        public List<ApplicationLog> GetLogs(DateTime? startDate = null, DateTime? endDate = null, long? customerID = null, APPLICATION_LOG_EVENT? logEvent = null);
    }
}
