using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Shared.Utilities.Logging
{
    public class CourseRequestEventLog
    {
        public string CourseName { get; set; }
        public string TransactionName { get; set; }
        public string TransactionTriggerPoint { get; set; }
        public DateTime TransactionTimestamp { get; set; }
    }
}
