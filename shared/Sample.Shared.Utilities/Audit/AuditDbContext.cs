using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sample.Shared.Utilities.Data;
using System.Linq;

namespace Sample.Shared.Utilities.Audit
{
    public class AuditDbContext : DbContext
    {
        private const string KeySeperator = ";";
        public AuditDbContext(DbContextOptions options)
           : base(options)
        {
        }

        /// <summary>
        /// To save the db operation with audit log implimentaion.
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public int SaveChanges(IUserSession userSession)
        {
            int affectedRows = 0;
            var nonAuditableEntities = Enum.GetValues(typeof(NonAuditableEntities)).Cast<NonAuditableEntities>().Select(v => v.ToString()).ToList();
            var addedEntityEntries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Added && !nonAuditableEntities.Contains(p.Entity.GetType().Name)).ToList();
            var modifiedEntityEntries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified && !nonAuditableEntities.Contains(p.Entity.GetType().Name)).ToList();
            var deletedEntityEntries = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Deleted && !nonAuditableEntities.Contains(p.Entity.GetType().Name)).ToList();
            var auditLogs = GetModifiedAuditList(userSession.UserId, modifiedEntityEntries, deletedEntityEntries);

            // Saving user requested db operation into the db.
            affectedRows = base.SaveChanges();
            auditLogs.AddRange(GetAddedAuditList(userSession.UserId, addedEntityEntries));
            if(auditLogs!=null && auditLogs.Any())
            {
                Set<AuditLog>().AddRange(auditLogs.Cast<AuditLog>());

                // Saving audit log informations into the db.
                base.SaveChanges();
            }
            return affectedRows;
        }
        private List<IAuditLog> GetModifiedAuditList(string userId,
                IEnumerable<EntityEntry> modifiedEntityEntries,
                IEnumerable<EntityEntry> deletedEntityEntries)
        {
            var auditLogs = new List<IAuditLog>();
            if (modifiedEntityEntries != null && modifiedEntityEntries.Any())
            {
                foreach (var auditRecordsForEntityEntry in modifiedEntityEntries.Select(changedEntity => GenerateChangeLogs(changedEntity, userId, EntityState.Modified)))
                {
                    auditLogs.AddRange(auditRecordsForEntityEntry);
                }
            }
            if (deletedEntityEntries != null && deletedEntityEntries.Any())
            {
                foreach (var auditRecordsForEntityEntry in deletedEntityEntries.Select(changedEntity => GenerateChangeLogs(changedEntity, userId, EntityState.Deleted)))
                {
                    auditLogs.AddRange(auditRecordsForEntityEntry);
                }
            }
            return auditLogs;
        }

        private List<IAuditLog> GetAddedAuditList(string userId, IEnumerable<EntityEntry> addedEntityEntries)
        {
            var auditLogs = new List<IAuditLog>();
            if(addedEntityEntries!=null)
            {
                foreach (var auditRecordsForEntityEntry in addedEntityEntries.Select(changedEntity => GenerateChangeLogs(changedEntity, userId, EntityState.Added)))
                {
                    auditLogs.AddRange(auditRecordsForEntityEntry);
                }
            }
            return auditLogs;
        }

        /// <summary>
        /// To return the formatted list of IAuditLog entity.
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="userId"></param>
        /// <param name="loginSessionId"></param>
        /// <param name="entityState"></param>
        /// <returns></returns>
        private IEnumerable<IAuditLog> GenerateChangeLogs(EntityEntry entityEntry,
            string userId,
            EntityState entityState)
        {
            var returnValue = new List<IAuditLog>();

            // Collecting primary key value and name of a single entity entry
            var keyRepresentation = GetPrimaryKeyInfo(entityEntry, KeySeperator);

            // Collecting properties of the single entity entry.
            var auditedPropertyNames = entityEntry.Entity.GetType().GetProperties().Select(info => info.Name).ToList();

            // Collecting all the pre-defined non-auditable properties.
            var nonAuditablePropertyNames = Enum.GetValues(typeof(NonAuditableAttribute)).Cast<NonAuditableAttribute>().Select(v => v.ToString()).ToList();

            // Removing non-auditable properties.
            auditedPropertyNames = auditedPropertyNames.Where(x => !nonAuditablePropertyNames.Contains(x)).ToList();

            // Looping each properties of the single entity entry
            foreach (var propertyEntry in entityEntry.Metadata.GetProperties().Where(x => auditedPropertyNames.Contains(x.Name)).Select(property => entityEntry.Property(property.Name)))
            {
                var originalValue = Convert.ToString(entityEntry.GetDatabaseValues().GetValue<object>(propertyEntry.Metadata.Name));

                // Current value will be empty for delete opeations
                if (entityState == EntityState.Modified)
                {
                    string currentValue = null;
                    if (propertyEntry.CurrentValue != null)
                    {
                        // Collecting current value
                        currentValue = Convert.ToString(propertyEntry.CurrentValue);
                    }

                    // Skip : If original value and old value are same
                    if (originalValue.Equals(currentValue))
                    {
                        continue;
                    }
                }
                returnValue.Add(new AuditLog
                {
                    PrimaryKeyNames = keyRepresentation.Key,
                    PrimaryKeyValues = keyRepresentation.Value,
                    OriginalValue = entityState != EntityState.Added ? Convert.ToString(originalValue) : null,
                    NewValue = entityState == EntityState.Modified || entityState == EntityState.Added ? Convert.ToString(propertyEntry.CurrentValue) : null,
                    PropertyName = propertyEntry.Metadata.Name,
                    TimeStamp = DateTime.UtcNow,
                    EventType = entityState.ToString(),
                    UserId = userId,
                    TableName = entityEntry.Entity.GetType().Name,
                });
            }

            return returnValue;
        }
        /// <summary>
        /// To return a dictionary with key as PrimaryKeyName and value as PrimaryKey.
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetPrimaryKeyInfo(EntityEntry entityEntry, string seperator)
        {
            var keyProperties = entityEntry.Metadata.GetProperties().Where(x => x.IsPrimaryKey()).ToList();
            if (keyProperties == null)
            {
                throw new ArgumentException("The key doesn't exist");
            }
            var keyPropertyEntries = keyProperties.Select(keyProperty => entityEntry.Property(keyProperty.Name)).ToList();
            var keyNameString = new StringBuilder();
            foreach (var keyProperty in keyProperties)
            {
                keyNameString.Append(keyProperty.Name);
                keyNameString.Append(seperator);
            }
            keyNameString.Remove(keyNameString.Length - 1, 1);
            var keyValueString = new StringBuilder();
            foreach (var keyPropertyEntry in keyPropertyEntries)
            {
                keyValueString.Append(keyPropertyEntry.CurrentValue);
                keyValueString.Append(seperator);
            }
            keyValueString.Remove(keyValueString.Length - 1, 1);
            var key = Convert.ToString(keyNameString);
            var value = Convert.ToString(keyValueString);
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
