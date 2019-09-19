using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Shared.Utilities.Audit
{
    public class AuditLog : IAuditLog
    {
        public long Id { get; set; }
        public string UserId { get; set; }

        [MaxLength(100)]
        public string TableName { get; set; }

        [MaxLength(100)]
        public string EventType { get; set; }

        [MaxLength(100)]
        public string PrimaryKeyNames { get; set; }

        [MaxLength(100)]
        public string PrimaryKeyValues { get; set; }

        public string NewValue { get; set; }
        public string OriginalValue { get; set; }

        [MaxLength(100)]
        public string PropertyName { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}