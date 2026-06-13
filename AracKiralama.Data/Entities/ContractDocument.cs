namespace AracKiralama.Data.Entities;

public class ContractDocument
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public Rental Rental { get; set; } = null!;
    public string PdfRelativePath { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; }
    public string TemplateVersion { get; set; } = "v1";
}
