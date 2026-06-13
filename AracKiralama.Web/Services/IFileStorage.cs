namespace AracKiralama.Web.Services;

public interface IFileStorage
{
    Task<string> SaveAsync(Stream stream, string originalFileName, string subFolder, CancellationToken cancellationToken = default);
}
