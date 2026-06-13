using AracKiralama.Data.Enums;

namespace AracKiralama.Data.Entities;

public class StoredDocument
{
    public int Id { get; set; }
    public DocumentEntityKind EntityKind { get; set; }
    public int EntityId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public DateTime UploadedUtc { get; set; }
    public string? UploadedByUserId { get; set; }
}
