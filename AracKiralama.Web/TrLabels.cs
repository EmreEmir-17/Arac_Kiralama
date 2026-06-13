using AracKiralama.Data.Enums;

namespace AracKiralama.Web;

/// <summary>Kullanıcı arayüzünde enum ve sabit metinlerin Türkçe karşılıkları.</summary>
public static class TrLabels
{
    public static string RentalState(RentalState s) => s switch
    {
        global::AracKiralama.Data.Enums.RentalState.Draft => "Taslak",
        global::AracKiralama.Data.Enums.RentalState.Confirmed => "Onaylı",
        global::AracKiralama.Data.Enums.RentalState.Active => "Aktif",
        global::AracKiralama.Data.Enums.RentalState.Completed => "Tamamlandı",
        global::AracKiralama.Data.Enums.RentalState.Cancelled => "İptal",
        _ => s.ToString()
    };

    public static string InvoiceStatus(InvoiceStatus s) => s switch
    {
        global::AracKiralama.Data.Enums.InvoiceStatus.Draft => "Taslak",
        global::AracKiralama.Data.Enums.InvoiceStatus.Issued => "Kesildi",
        global::AracKiralama.Data.Enums.InvoiceStatus.Paid => "Ödendi",
        global::AracKiralama.Data.Enums.InvoiceStatus.Cancelled => "İptal",
        _ => s.ToString()
    };

    public static string PaymentMethod(PaymentMethod m) => m switch
    {
        global::AracKiralama.Data.Enums.PaymentMethod.Cash => "Nakit",
        global::AracKiralama.Data.Enums.PaymentMethod.Card => "Kart",
        global::AracKiralama.Data.Enums.PaymentMethod.BankTransfer => "Havale / EFT",
        global::AracKiralama.Data.Enums.PaymentMethod.Other => "Diğer",
        _ => m.ToString()
    };

    public static string VehicleStatus(VehicleStatus s) => s switch
    {
        global::AracKiralama.Data.Enums.VehicleStatus.Available => "Müsait",
        global::AracKiralama.Data.Enums.VehicleStatus.Rented => "Kirada",
        global::AracKiralama.Data.Enums.VehicleStatus.Maintenance => "Bakımda",
        global::AracKiralama.Data.Enums.VehicleStatus.Inactive => "Pasif",
        _ => s.ToString()
    };

    public static string ReservationState(ReservationState s) => s switch
    {
        global::AracKiralama.Data.Enums.ReservationState.Pending => "Beklemede",
        global::AracKiralama.Data.Enums.ReservationState.Confirmed => "Onaylı",
        global::AracKiralama.Data.Enums.ReservationState.Cancelled => "İptal",
        global::AracKiralama.Data.Enums.ReservationState.ConvertedToRental => "Kiralamaya dönüştü",
        _ => s.ToString()
    };
}
