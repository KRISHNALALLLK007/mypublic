using System;

namespace Sample.Shared.Utilities.Audit
{
    /// <summary>
    /// This enum will contains all the non-auditable attributes
    /// </summary>
    public enum NonAuditableAttribute 
    {
        CreatedBy,
        UpdatedBy,
        UpdatedDate,
        CreatedDate
    }
}
