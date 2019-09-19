using System;

namespace Sample.Shared.Utilities.Audit
{
    public interface IAuditLog
    {
        long Id { get; set; }
        string UserId { get; set; }
        string TableName { get; set; }
        string EventType { get; set; }
        string PrimaryKeyNames { get; set; }
        string PrimaryKeyValues { get; set; }
        string NewValue { get; set; }
        string OriginalValue { get; set; }
        string PropertyName { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
