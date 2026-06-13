using AracKiralama.Data;
using AracKiralama.Data.Entities;
using AracKiralama.Web;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace AracKiralama.Web.Services;

public sealed class ContractPdfService(IDbContextFactory<AppDbContext> dbFactory, IFileStorage files)
{
    public async Task<string> GenerateForRentalAsync(int rentalId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var rental = await db.Rentals
            .Include(x => x.Vehicle).ThenInclude(v => v.Branch)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == rentalId, cancellationToken)
            ?? throw new InvalidOperationException("Kiralama bulunamadı.");

        QuestPDF.Settings.License = LicenseType.Community;

        var ms = new MemoryStream();
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(32);
                page.Header().Text("Araç Kiralama Sözleşmesi").SemiBold().FontSize(18);
                page.Content().Column(col =>
                {
                    col.Spacing(8);
                    col.Item().Text($"No: RNT-{rental.Id}");
                    col.Item().Text($"Tarih (UTC): {DateTime.UtcNow:yyyy-MM-dd HH:mm}");
                    col.Item().LineHorizontal(1);
                    col.Item().Text($"Müşteri: {rental.Customer.FullName}");
                    col.Item().Text($"Araç: {rental.Vehicle.Plate} {rental.Vehicle.Brand} {rental.Vehicle.Model}");
                    col.Item().Text($"Şube: {rental.Vehicle.Branch.Name}");
                    col.Item().Text($"Başlangıç: {rental.StartUtc:u}");
                    col.Item().Text($"Bitiş: {rental.EndUtc:u}");
                    col.Item().Text($"Durum: {TrLabels.RentalState(rental.State)}");
                    if (rental.TotalPrice is not null)
                    {
                        col.Item().Text($"Tutar: {rental.TotalPrice:N2} TL");
                    }

                    col.Item().PaddingTop(16).Text(
                        "Bu belge elektronik ortamda üretilmiştir. Taraflar, kiralama koşullarını ve trafik mevzuatını kabul etmiş sayılır.");
                });
            });
        }).GeneratePdf(ms);
        ms.Position = 0;

        var path = await files.SaveAsync(ms, $"sozlesme-{rental.Id}.pdf", "contracts", cancellationToken);
        var relative = path.TrimStart('/');

        db.ContractDocuments.Add(new ContractDocument
        {
            RentalId = rentalId,
            PdfRelativePath = relative,
            CreatedUtc = DateTime.UtcNow,
            TemplateVersion = "v1"
        });
        await db.SaveChangesAsync(cancellationToken);

        return path;
    }
}
