namespace AracKiralama.Web.Services;

public sealed class WebRootFileStorage(IWebHostEnvironment env) : IFileStorage
{
    public async Task<string> SaveAsync(Stream stream, string originalFileName, string subFolder, CancellationToken cancellationToken = default)
    {
        var ext = Path.GetExtension(Path.GetFileName(originalFileName));
        var unique = $"{Guid.NewGuid():N}{ext}";
        var relative = $"uploads/{subFolder}/{unique}".Replace('\\', '/');
        var physical = Path.Combine(env.WebRootPath, "uploads", subFolder, unique);
        Directory.CreateDirectory(Path.GetDirectoryName(physical)!);
        await using var fs = File.Create(physical);
        await stream.CopyToAsync(fs, cancellationToken);
        return "/" + relative;
    }
}
