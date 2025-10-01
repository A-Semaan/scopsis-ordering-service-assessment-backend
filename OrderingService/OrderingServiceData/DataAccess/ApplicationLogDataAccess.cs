using Microsoft.EntityFrameworkCore;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess
{
    public class ApplicationLogDataAccess : IApplicationLogDataAccess
    {
        private readonly OrderingServiceDbContext _dbContext;

        public ApplicationLogDataAccess(OrderingServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ApplicationLog> GetLogs(DateTime? startDate = null, DateTime? endDate = null, long? customerID = null, APPLICATION_LOG_EVENT? logEvent = null)
        {
            return _dbContext.ApplicationLogs.Where(log =>
                (startDate == null || log.LogDate >= startDate) &&
                (endDate == null || log.LogDate <= endDate) &&
                (customerID == null || (log.Customer != null && log.Customer.ID == customerID)) &&
                (logEvent == null || log.Event == logEvent)
            ).Include(log => log.Customer).ToList();
        }

        public long InsertLog(ApplicationLog applicationLog)
        {
            _dbContext.ApplicationLogs.Add(applicationLog);

            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to insert application log.");
            }

            return applicationLog.LogID;
        }
    }
}
