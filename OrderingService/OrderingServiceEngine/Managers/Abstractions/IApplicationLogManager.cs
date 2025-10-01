using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers.Abstractions
{
    public interface IApplicationLogManager
    {
        public long InsertLog(APPLICATION_LOG_EVENT evnt, string? eventInfo = null, long? customerID = null);

        public List<ApplicationLogModel> GetLogs(long retrieverID, DateTime? startDate = null, DateTime? endDate = null, long? customerID = null, string? logEvent = null);
    }
}
