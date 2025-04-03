
using Core.Constants;
using Core.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
namespace Infrastructure.Helper;

public class AudityEntry
{

    public EntityEntry Entry { get; set; }
    public string UserId { get; set; }
    public string EntityName { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> OldValue { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> NewValue { get; } = new Dictionary<string, object>();
    public AuditLogAction Action { get; set; }
    public List<string> ChangedColumns { get; } = new List<string>();
    public AudityEntry(EntityEntry entry)
    {
        Entry = entry;
    }
    public AuditLog ToAudit()
    {
        var audit = new AuditLog();
        audit.UserId = UserId;
        audit.Action = Action.ToString();
        audit.EntityName = EntityName;
        audit.Timestamp = DateTime.Now;
        audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
        audit.OldValue = OldValue.Count == 0 ? null : JsonConvert.SerializeObject(OldValue);
        audit.NewValue = NewValue.Count == 0 ? null : JsonConvert.SerializeObject(NewValue);
        audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
        audit.CreatedDate = DateTime.Now;
        audit.CreatedBy = CurrentUser.UserId;
        return audit;


    }

}
