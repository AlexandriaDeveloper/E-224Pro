using System;
using Core.Models;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace Infrastructure.Interceptors;

public class AuditLogsInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        try
        {
            if (eventData.Context is not null)
            {
                BeforeSavingChange(eventData);
            }
            var tr = await base.SavingChangesAsync(eventData, result, cancellationToken);
            return tr;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }

    }
    private void BeforeSavingChange(DbContextEventData eventData)
    {
        var context = eventData.Context;
        context.ChangeTracker.DetectChanges();
        var auditEntries = new List<AudityEntry>();
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            {
                continue;
            }
            var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();
            if (string.IsNullOrEmpty(primaryKey) && entry.Entity is BaseEntity baseEntity)
            {
                primaryKey = baseEntity.TempId.ToString();
            }
            var auditEntry = new AudityEntry(entry);
            auditEntry.EntityName = entry.Entity.GetType().Name;
            auditEntry.UserId = "test";
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues.Add(propertyName, property.CurrentValue);
                    continue;
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.Action = AuditLogAction.Insert;
                        auditEntry.NewValue[propertyName] = property.CurrentValue;
                        if (property.Metadata.IsPrimaryKey())
                        {
                            auditEntry.KeyValues.Remove(propertyName);
                            auditEntry.KeyValues.Add(propertyName, 0);
                        }

                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.Action = AuditLogAction.Update;
                            auditEntry.OldValue[propertyName] = property.OriginalValue;
                            auditEntry.NewValue[propertyName] = property.CurrentValue;

                        }
                        break;
                    case EntityState.Deleted:
                        auditEntry.Action = AuditLogAction.Delete;
                        auditEntry.OldValue[propertyName] = property.OriginalValue;

                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries)
        {
            context.Add(auditEntry.ToAudit());
        }
    }

}

/*



*/