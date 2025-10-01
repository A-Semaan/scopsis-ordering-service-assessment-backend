using AutoMapper;
using OrderingServiceData.DataAccess;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers
{
    public class ApplicationLogManager : IApplicationLogManager
    {
        private readonly IApplicationLogDataAccess _applicationLogDataAccess;
        private readonly IMapper _mapper;


        public ApplicationLogManager(IApplicationLogDataAccess applicationLogDataAccess, IMapper mapper)
        {
            _applicationLogDataAccess = applicationLogDataAccess;
            _mapper = mapper;
        }

        public List<ApplicationLogModel> GetLogs(long retrieverID, DateTime? startDate = null, DateTime? endDate = null, long? customerID = null, string? logEvent = null)
        {
            List<ApplicationLog> logs = _applicationLogDataAccess.GetLogs(startDate, endDate, customerID, (APPLICATION_LOG_EVENT)Enum.Parse(typeof(APPLICATION_LOG_EVENT), logEvent));

            InsertLog(APPLICATION_LOG_EVENT.LOGS_ACCESSED, null, retrieverID);

            return _mapper.Map<List<ApplicationLogModel>>(logs);
        }

        public long InsertLog(APPLICATION_LOG_EVENT evnt, string? eventInfo = null, long? customerID = null)
        {
            ApplicationLog temp = new ApplicationLog
            {
                LogDate = DateTime.UtcNow,
                Event = evnt,
                EventInfo = eventInfo,
                CustomerID = customerID
            };

            return _applicationLogDataAccess.InsertLog(temp);
        }
    }
}
